using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFlip : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime =0.1f;
    public float currentWait;
    public BoxCollider2D bxc;
    public bool touchingPlayer;
    
    void Start()
    {
        bxc = GetComponent<BoxCollider2D>();
        effector = GetComponent<PlatformEffector2D>();
    }

    
    void Update()
    {
        touchingPlayer = bxc.IsTouchingLayers(13);

        if (currentWait <= 0 && effector.rotationalOffset != 0 && !bxc.IsTouchingLayers(13))
        {
            effector.rotationalOffset = 0;
            currentWait = waitTime;
        }
        else if (currentWait > 0)
        {
            currentWait -= Time.deltaTime;
        }

        if (Input.GetButton("Crouch") && Input.GetButton("Jump"))
        {
                currentWait = waitTime;
                Debug.Log("yes");
                effector.rotationalOffset = 180;
        } 
        

    }
}
