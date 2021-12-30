using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuCanvas;

    
  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused && (GameObject.FindWithTag("GameOver").gameObject.transform.GetChild(10).gameObject.activeSelf == false))
            {
                Resume();
            }
            else if(GameObject.FindWithTag("GameOver").gameObject.transform.GetChild(10).gameObject.activeSelf==false)
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    

}