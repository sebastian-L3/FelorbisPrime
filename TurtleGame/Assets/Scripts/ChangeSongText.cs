using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSongText : MonoBehaviour
{
    public AudioClip[] Aclips;
    public string[] Nclips;
    public Text Text;
    private AudioSource audiosource;
    int randomNumber = 0;
    public float timer = 1f;

    private Animator an;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = FindObjectOfType<AudioSource>();
        audiosource.loop = false;
        an = GetComponent<Animator>();
    }

    private void GetRandom()
    {
        randomNumber = Random.Range(0, Aclips.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (!audiosource.isPlaying && timer<=0)
        {
            GetRandom();
            audiosource.clip = Aclips[randomNumber];
            Text.text = "Playing " + Nclips[randomNumber];
            audiosource.Play();
            timer = 5f;
            an.SetTrigger("spawn");

        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            
        }
    }
}
