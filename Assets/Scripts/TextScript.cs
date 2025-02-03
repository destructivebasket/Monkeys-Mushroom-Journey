using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{

    public GameObject UIObject1;
    public GameObject UIObject2;
    public GameObject cube;

    public GameObject monkey;
    public ObjectCollector state;
    // Start is called before the first frame update
    void Start()
    {
        UIObject1.SetActive(false);
        UIObject2.SetActive(false);
    }

    // Update is called once per frame

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Monkey")
        {
            if (!state.GetComponent<ObjectCollector>().yellowShroom)
            {
                UIObject1.SetActive(true);
            }
            else if (state.GetComponent<ObjectCollector>().yellowShroom)
            {
                UIObject2.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!state.GetComponent<ObjectCollector>().yellowShroom)
        {
            UIObject1.SetActive(false);
        }
        else if (state.GetComponent<ObjectCollector>().yellowShroom)
        {
            UIObject2.SetActive(false);
        }
    }
}
