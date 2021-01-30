using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdDialogue : MonoBehaviour
{

    [SerializeField]
    private DialogueManager manager;
    [SerializeField]
    private GameManager gm;
    [SerializeField]
    private AudioManager am;
    private bool isStart = false;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (!gm.getIsDialogueOn())
            {
                Destroy(this.gameObject);
                isStart = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            am.play("Crow");
            manager.startDialogue("Eagle", true);
            isStart = true;
        }
    }
}
