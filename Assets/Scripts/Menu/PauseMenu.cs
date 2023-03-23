using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool is_paused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //touche ECHAP pour déclencher le "PauseMenu"
        {
            if (is_paused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }

    void Paused()
    {
        //avec arret 
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        is_paused = true;
        
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        is_paused = false;
    }
    public void MainMenuButton()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            string MenuName = "StartMenu";
            Debug.Log($"We load the '{MenuName}' ");
            PhotonNetwork.LoadLevel(MenuName);
            PhotonNetwork.JoinRoom(MenuName);
            //PhotonNetwork.Disconnect(); <-- à corriger pour déconnecter le joueur quand il quitte le jeu

        }
    }
}
