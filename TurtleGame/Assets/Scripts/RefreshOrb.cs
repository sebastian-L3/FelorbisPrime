using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshOrb : MonoBehaviour
{
    private float respawnTime = 3f; //respawn saat sudah dipakai 1 kali
    private bool isEnable = true;
    private Renderer renderer;
    private Collider2D orbCollider;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        orbCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Timer();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isEnable)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("refresh orb");
                GeneralMovement gm = collision.gameObject.GetComponent<GeneralMovement>();

                gm.RefreshOrb(); // lakukan fungsi di karakter masing masing 

                Disenabled();
            }
        }
    }

    private void Disenabled()
    {
        isEnable = false;
        renderer.enabled = false;
        orbCollider.enabled = false;
    }

    private void Enable()
    {
        isEnable = true;
        renderer.enabled = true;
        orbCollider.enabled = true;
    }

    private void Timer()
    {
        if (!isEnable) // timer jika mati untuk respawn
        {
            if (respawnTime <= 0f) // jika udh hbs timer
            {
                respawnTime = 3f;
                Enable();
            }
            respawnTime -= Time.deltaTime;
        }
    }
}
