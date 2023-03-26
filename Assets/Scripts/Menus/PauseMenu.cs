using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (LevelManager.gameisPaused) Resume();
            else Paused();
        }
    }

    void Paused()
    {
        PlayerManager.movement = Vector2.zero;
        pauseMenu.SetActive(true);
        LevelManager.gameisPaused = true;
    }
    
    public void Resume()
    {
        pauseMenu.SetActive(false);
        LevelManager.gameisPaused = false;
    }

    public void LoadMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        LevelManager.gameisPaused = false;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }


}
