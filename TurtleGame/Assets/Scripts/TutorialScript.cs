using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{

    public bool enterTutorialZone;

    public GameObject tutorial;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (enterTutorialZone)
        {
            tutorial.SetActive(true);
        }
        else
        {
            tutorial.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            enterTutorialZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enterTutorialZone = false;
    }

}
