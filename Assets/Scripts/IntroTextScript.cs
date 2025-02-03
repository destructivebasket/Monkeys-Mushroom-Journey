using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroTextScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject introText;
    void Start()
    {
        introText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Monkey")
        {
            introText.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        introText.SetActive(false);
    }
}
