using Photon.Pun;
using Photon.Realtime;
using RogueCaml;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public GameManager gameManager;
    public TMP_InputField inputField;
    public Button playButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonHandler()
    {
        playButton.interactable = false;
        string pseudo = inputField.text;
        gameManager.ConnectPlayer(pseudo);
    }

    public void ExitButtonHandler()
    {
        gameManager.QuitGame();
    }

}