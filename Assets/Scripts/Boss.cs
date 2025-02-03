using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    //ai pathfinding
    UnityEngine.AI.NavMeshAgent agent;

    //waypoints, able to change in inspector
    public GameObject[] waypoints = new GameObject[4];


    //enums for state machine

    public Animator animator;
    public enum GuardState
    {
        Waypoint, MoveToWaypoint, Chase, StopChase, MoveToBuilding, Attack
    }

    //guard range sight
    public int viewRange = 20;
    public int attackRange = 5;

    public GuardState state = GuardState.Waypoint; // state
    public int currentWaypoint = 0; // starting waypoint
    public GameObject building; // building
    public GameObject target; // chase target
    public bool buildingInRange; //distance checks
    public bool playerInRange;
    public bool playerInAttackRange;
    public float health;
    public float maxHealth;

    public Slider slider;

    public bool isDead = false;
   

    
    void Start()
    {
        //initialize
        health = maxHealth;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();

        building = GameObject.Find("Building");// find object instead of inspector

        state = GuardState.Waypoint;

        //sets so it goes towards 1
        currentWaypoint = -1;
    }

    void Update()
    {
        if(!isDead)
        {
         CheckGameState();
        }
        else
        {
             slider.value = CalculateHealth();
             agent.enabled = false;
             agent.speed = 0;
        }

    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }

    public void TakeDamage()
    {
        health--;
        if(health <= 0)
        {
            animator.SetTrigger("Death");
            isDead = true;
        }
        slider.value = CalculateHealth();
    }

    void CheckGameState()
    {
        //check once per frame


        buildingInRange = BuildingInRange();
        playerInRange = PlayerInRange();
        playerInAttackRange = PlayerInAttackRange();
        //global changes    
        if(playerInAttackRange)
        {
            state = GuardState.Attack;
        }
        else if(!buildingInRange && state != GuardState.MoveToBuilding)
        {
           // Debug.Log("Stop Chase");
            state = GuardState.StopChase;
        }
        else if(buildingInRange && playerInRange)
        {
            state = GuardState.Chase;
        }

        switch(state)
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
                MoveToWayPoint();
                break;
            case GuardState.Chase:
                ChasePlayer();
                break;
            default:
                Debug.Log("Error : invalid state");
                break;
        }
    }

    

    public bool BuildingInRange()
    {
        //position difference between guard and building
        Vector3 difference = building.transform.position - transform.position;
       //squared faster
        return difference.sqrMagnitude < (viewRange * viewRange);
    }
    
    //changes target
    public GameObject FindClosestTarget(string tag, float maxDistance)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;

        float distance = maxDistance * maxDistance;
        Vector3 position = transform.position;

        foreach(GameObject obj in gameObjects)
        {
            Vector3 difference = obj.transform.position - position;
            float curDistance = difference.sqrMagnitude;

            if(curDistance < distance)
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
        int randInt = Random.Range(0, waypoints.Length);
        currentWaypoint = randInt;

        agent.SetDestination(waypoints[currentWaypoint].transform.position);

        state = GuardState.MoveToWaypoint;
    }


    public void ChasePlayer()
    {
        if(target != null)
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {
            state = GuardState.Waypoint;
        }
    }

    public void MoveToWayPoint()
    {

    }

    public void StopChase()
    {
        agent.SetDestination(building.transform.position);
        state = GuardState.MoveToBuilding;
      
    }

    public void MoveToBuilding()
    {
        if(buildingInRange)
        {
            state = GuardState.Waypoint;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Waypoint")
        {

            if(state == GuardState.MoveToWaypoint)
            {
                state = GuardState.Waypoint;
            }
        }
        else if(other.tag == "Jab")
        {
            TakeDamage();
        }
    }

    public bool PlayerInAttackRange()
    {
        target = FindClosestTarget("Player", attackRange);

        return target != null;
    }

     
     public void Attack()
     {
        if(target == null)
        {
            state = GuardState.Waypoint;

            return;
        }

            agent.ResetPath();
            transform.LookAt(target.transform);
            agent.SetDestination(target.transform.position);

    }


    }