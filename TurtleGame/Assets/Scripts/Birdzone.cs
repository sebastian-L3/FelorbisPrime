using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birdzone : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsBirdzone;

    public bool enterBirdzone;
    public GameObject bird;
    public BirdBoss birdScript;
    public GameObject player1;
    public GameObject player2;
    public bool isTurtle=false;
    public float checkerRadius;
    AudioManager AM;
    // Start is called before the first frame update
    void Start()
    {
        birdScript = bird.GetComponent<BirdBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        enterBirdzone = Physics2D.OverlapCircle(player1.transform.position, checkerRadius, whatIsBirdzone);
            if (enterBirdzone) isTurtle = true;
        if (!enterBirdzone)
        {
            enterBirdzone = Physics2D.OverlapCircle(player2.transform.position, checkerRadius, whatIsBirdzone);
            if (enterBirdzone) isTurtle = false;
        }
        if (enterBirdzone)
        {
            AM.play("Crow");
            bird.SetActive(true);
            if (isTurtle) birdScript.player = player1;
            else birdScript.player = player2;
        }
        else
        {
            birdScript.reachedHome = false;
        }
    }


}
