using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingMusicDetector : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private ChangeSongText cs;

    [SerializeField]
    private AudioManager am;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(cs.gameObject);
            am.play("Ending");
        }
    }
}
