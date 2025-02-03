using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollector : MonoBehaviour
{
    // Start is called before the first frame update
    public bool yellowShroom = false;
    public bool purpleShroom = false;
    public int sunflowerCount = 0;
    public bool completed = false;
    public bool Trigger4On = false;
    public bool Trigger2On = false;
    public bool Trigger1On = false;

    public GameObject confetti1;
    public GameObject confetti2;
    public GameObject confetti3;

    public GameObject bossTeleportation;
    public ExitQuestState state = ExitQuestState.None;
    public enum ExitQuestState
    {
        None, YellowShroom, PurpleShroom, Sunflower, Completed
    }
    void Start()
    {
        bossTeleportation.SetActive(false);
        state = ExitQuestState.None;
    }

    // Update is called once per frame
    void Update()
    {
        CheckExitQuest();
        if (state == ExitQuestState.Completed)
        {
            bossTeleportation.SetActive(true);
            CompletedExitQuest();
        }
    }

    //Checks if the item was acquired or not
    public void CheckExitQuest()
    {
        switch (state)
        {
            case ExitQuestState.None:
                NoneExitQuest();
                break;
            case ExitQuestState.YellowShroom:
                YellowShroomExitQuest();
                break;
            case ExitQuestState.PurpleShroom:
                PurpleShroomExitQuest();
                break;
            case ExitQuestState.Sunflower:
                SunflowerExitQuest();
                break;
            case ExitQuestState.Completed:
                Debug.Log("Completed");
                CompletedExitQuest();
                break;
        }
    }

    public void NoneExitQuest()
    {
        if (state == ExitQuestState.None)
        {
            Debug.Log("Go find a yellow shroom pls :D");
            state = ExitQuestState.YellowShroom;
        }
    }

    public void YellowShroomExitQuest()
    {
        if (yellowShroom && Trigger4On)
        {
            Debug.Log("Thanks for returning the yellow mushroom.");
            Trigger4On = false;
        }
        else
        {
            Debug.Log("Gimme my yellow shroom >:(");
        }
    }

    public void PurpleShroomExitQuest()
    {
        if (purpleShroom && Trigger2On)
        {
            Debug.Log("Thanks for returning the purple mushroom.");

            Trigger2On = false;
        }
        else
        {
            Debug.Log("Gimme my purple shroom >:(");
        }
    }

    public void SunflowerExitQuest()
    {
        if (sunflowerCount == 3 && Trigger1On)
        {
            Debug.Log("Thanks for returning the 3 sunflowers.");

            state = ExitQuestState.Completed;

            Trigger1On = false;
        }
        else
        {
            Debug.Log("Gimme my sunflowers >:(");
        }
    }

    public void CompletedExitQuest()
    {
        bossTeleportation.SetActive(true);
    }


    public void PickUpItems(GameObject other)
    {
        switch (state)
        {
            case ExitQuestState.YellowShroom:
                PickUpYellowShroom(other);
                break;
            case ExitQuestState.PurpleShroom:
                PickUpPurpleShroom(other);
                break;
            case ExitQuestState.Sunflower:
                PickUpSunflower(other);
                break;
        }
    }

    public void PickUpYellowShroom(GameObject other)
    {
        if (other.name == "YellowShroom")
        {
            Debug.Log("Picked up yellow shroom");
        }

        confetti1.SetActive(true);
        yellowShroom = true;

        Destroy(other);
    }

    public void PickUpPurpleShroom(GameObject other)
    {
        if (other.name == "PurpleShroom")
        {
            Debug.Log("Picked up purple shroom");
        }

        purpleShroom = true;
        confetti2.SetActive(true);

        Destroy(other);
    }

    public void PickUpSunflower(GameObject other)
    {
        if (other.name == "Sunflower")
        {
            Debug.Log("Picked up sunflower");
        }

        if (sunflowerCount == 3)
        {
            confetti3.SetActive(true);
        }

        sunflowerCount++;

        Destroy(other);
    }


    //finds if the trigger in front of the quest guy was passed through
    private void OnTriggerEnter(Collider other)
    {
        //Check to see if player entered the exit quest trigger
        if (other.name == "ExitQuestTrigger4" && yellowShroom)
        {
            Trigger4On = true;
            state = ExitQuestState.PurpleShroom;
            Debug.Log("get the purple shroom!");
        }

        if (other.name == "ExitQuestTrigger2" && purpleShroom)
        {
            Trigger2On = true;
            state = ExitQuestState.Sunflower;
            Debug.Log("get the sunflower!");

            if (sunflowerCount == 3)
            {
                bossTeleportation.SetActive(true);
                state = ExitQuestState.Completed;
            }
        }

        if (other.name == "ExitQuestTrigger3")
        {

            CheckExitQuest();
        }

        Debug.Log("mushroom");
        if (other.gameObject.tag == "Sunflower" || other.gameObject.tag == "Shroom")
        {
            PickUpItems(other.gameObject);
        }
    }
}
