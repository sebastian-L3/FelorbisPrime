using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSongText : MonoBehaviour
{
    public AudioClip[] Aclips;
    public string[] Nclips;
    public Text Text;
    public AudioSource audiosource;
    int randomNumber = 0;
    public float timer = 1f;

    private Animator an;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
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
            Debug.Log("If ENtered SOng");
            GetRandom();
            audiosource.clip = Aclips[randomNumber];
            Text.text = "Playing " + Nclips[randomNumber];
            Debug.Log("If ENtered SOng1 " + audiosource.clip + " " + Text.text);
            audiosource.Play();
            timer = 5f;
            an.SetTrigger("spawn");
            Debug.Log("If ENtered SOng2");
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            
        }
    }
}
