using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isEnabled = false;

    private GameObject pauseMenuUI;

    void Start()
    {
        pauseMenuUI = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //touche ECHAP pour déclencher le "PauseMenu"
        {
            if (isEnabled)
            {
                Disable();
            }
            else
            {
                Enable();
            }
        }
    }

    void Enable()
    {
        //avec arret 
        pauseMenuUI.SetActive(true);
        isEnabled = true;
        
    }
    public void Disable()
    {
        pauseMenuUI.SetActive(false);
        isEnabled = false;
    }
}
