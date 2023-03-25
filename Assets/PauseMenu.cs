using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameisPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameisPaused) Resume();
            else Paused();
        }
    }

    void Paused()
    {
        //PlayerMovement.instance.enable = false; --> erreur compil ici, check pour vérifier
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameisPaused = true;
    }
    
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameisPaused = false;
    }

    public void LoadMainMenu()
    {
        PhotonNetwork.Disconnect();
        gameisPaused = false;
        pauseMenuUI.SetActive(false);
        PhotonNetwork.LoadLevel("MainMenu");
    }


}
