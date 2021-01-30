using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDialogue : MonoBehaviour
{
    [SerializeField]
    private DialogueManager manager;
    [SerializeField]
    private GameManager gm;
    private bool isStart = false;
    // Start is called before the first frame update
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
            manager.startDialogue("Ending", true);
            isStart = true;
        }
    }
}
