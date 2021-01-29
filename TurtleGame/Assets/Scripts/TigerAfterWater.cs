using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerAfterWater : MonoBehaviour
{
    [SerializeField]
    private DialogueManager manager;
    [SerializeField]
    private GameManager gm;
    private bool isStart = false;

    
    private GameObject turtle;

    void Start()
    {
        turtle = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (!gm.getIsDialogueOn())
            {
                isStart = false;
                Destroy(this.gameObject);
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            manager.startDialogue("TigerWater", true);
            turtle.GetComponent<SpriteRenderer>().enabled = true;
            isStart = true;
        }
    }
}
