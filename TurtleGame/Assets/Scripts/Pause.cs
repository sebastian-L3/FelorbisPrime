using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
    }

        public void resume()
        {
            Debug.Log("resume");
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        void pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }
        
        public void restart()
        {
            Scene currscene = SceneManager.GetActiveScene();
            string currSceneName = currscene.name;
            switch (currSceneName)
            {
            case "SampleScene":
                SceneManager.LoadScene("SampleScene");
                break;
            case "Scene2":
                SceneManager.LoadScene("Scene2");
                break;
            }
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

        public void Quitgame()
        {
        Application.Quit();
        }
}
