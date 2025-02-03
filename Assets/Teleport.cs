using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    // Assign the target zone in the Inspector
  //  public GameObject gameObject;

    public bool isTeleport;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Teleport"))
        {
            // Ensure zone is assigned
          

                // Teleport the current GameObject to the zone's position
                isTeleport = true;
                
               // gameObject.GetComponent<Rigidbody>().position = zone.transform.position;

        }
    }

    void Update()
    {
        if(isTeleport)
        {
            isTeleport = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
           // gameObject.transform.position = zone.transform.position;
        }
    }
}
