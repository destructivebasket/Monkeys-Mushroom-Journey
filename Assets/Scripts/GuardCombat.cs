using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCombat : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    public GameObject[] waypoints = new GameObject[2];
    public GameObject prefabPaintball;
    public GameObject muzzle;

    public enum GuardState
    {
        Waypoint, MoveToWaypoint, Chase, StopChase, MoveToBuilding, Attack
    }

    public int viewRange = 20;
    public int attackRange = 15;
    public float fireRate = 2f;
    public float ballSpeed = 2000f;

    public GuardState state = GuardState.Waypoint;
    public int currentWaypoint = 0;
    public GameObject building;
    public GameObject target;
    public bool buildingInRange;
    public bool playerInRange;
    public bool playerInAttackRange;
    public float lastFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        building = GameObject.Find("Building");
        state = GuardState.Waypoint;
        currentWaypoint = -1;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameState();
    }

    void CheckGameState()
    {
        lastFire += Time.deltaTime;
        buildingInRange = BuildingInRange();
        playerInAttackRange = PlayerInAttackRange();
        playerInRange = PlayerInRange();
        if (playerInAttackRange)
        {
            state = GuardState.Attack;
        }
        else if (!buildingInRange && state != GuardState.MoveToBuilding)
        {
            state = GuardState.StopChase;
        }
        else if (buildingInRange && playerInRange)
        {
            state = GuardState.Chase;
        }
        switch (state)
        {
            case GuardState.Attack:
                Attack();
                break;
            case GuardState.StopChase:
                StopChase();
                break;
            case GuardState.Waypoint:
                Waypoint();
                break;
            case GuardState.MoveToBuilding:
                MoveToBuilding();
                break;
            case GuardState.MoveToWaypoint:
                MoveToWaypoint();
                break;
            case GuardState.Chase:
                ChasePlayer();
                break;
            default:
                Debug.Log("Error: invalid state");
                break;
        }
    }

    public bool BuildingInRange()
    {
        Vector3 difference = building.transform.position - transform.position;
        return difference.sqrMagnitude < (viewRange * viewRange);
    }

    public GameObject FindClosestTarget(string tag, float maxDistance)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;

        float distance = maxDistance * maxDistance;
        Vector3 position = transform.position;

        foreach (GameObject obj in gameObjects)
        {
            Vector3 difference = obj.transform.position - position;
            float curDistance = difference.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = obj;
                distance = curDistance;
            }
        }
        return closest;
    }

    public bool PlayerInRange()
    {
        target = FindClosestTarget("Player", viewRange);
        return target != null;
    }

    public void Waypoint()
    {
        currentWaypoint += 1;
        if (currentWaypoint >= waypoints.Length)
        {
            currentWaypoint = 0;
        }
        agent.SetDestination(waypoints[currentWaypoint].transform.position);
        state = GuardState.MoveToWaypoint;
    }

    public void MoveToWaypoint() { }

    public void ChasePlayer()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {
            state = GuardState.Waypoint;
        }
    }

    public void StopChase()
    {
        agent.SetDestination(building.transform.position);
        state = GuardState.MoveToBuilding;
    }

    public void MoveToBuilding()
    {
        if (buildingInRange)
        {
            state = GuardState.Waypoint;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint")
        {
            if (state == GuardState.MoveToWaypoint)
            {
                state = GuardState.Waypoint;
            }
        }
    }

    public bool PlayerInAttackRange()
    {
        target = FindClosestTarget("Player", attackRange);
        return target != null;
    }

    public void Attack()
    {
        if (target == null)
        {
            state = GuardState.Waypoint;
            return;
        }
        agent.ResetPath();
        transform.LookAt(target.transform);
        if (lastFire >= fireRate)
        {
            GameObject ball = Object.Instantiate(prefabPaintball, muzzle.transform.position, Quaternion.identity);
            Rigidbody rigidBody = ball.GetComponent<Rigidbody>();
            rigidBody.AddForce(transform.forward * ballSpeed);
            lastFire = 0;
        }
    }
}
