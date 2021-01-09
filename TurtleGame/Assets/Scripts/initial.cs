using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class initial : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject Music;
    public GameObject EVS;

    public Scene currentScene;
    public string cSceneName;

    void Start()
    {
        Canvas = GameObject.FindWithTag("UI");
        Music = GameObject.FindWithTag("Music");
        EVS = GameObject.FindWithTag("EventSystem");

        DontDestroyOnLoad(Canvas);
        DontDestroyOnLoad(Music);
        DontDestroyOnLoad(EVS);

        DontDestroyOnLoad(this.gameObject);

        //currentScene = SceneManager.GetActiveScene();
        //cSceneName = currentScene.name;

        //SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {
        //if(cSceneName=="First Scene") {
         //   SceneManager.LoadScene("SampleScene");
         //  currentScene = SceneManager.GetActiveScene();
          //  cSceneName = currentScene.name;
        //}
    }
}
