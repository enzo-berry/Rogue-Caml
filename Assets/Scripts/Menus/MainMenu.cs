using Photon.Pun;
using Photon.Realtime;
using RogueCaml;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public GameManager gameManager;
    public TMP_InputField inputField;
    public Button playButton;

    static public MainMenu Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayButtonHandler()
    {
        string pseudo = inputField.text;
        if (pseudo == "") Debug.Log("No pseudo entered !");
        else
        {
            Debug.Log($"Connect process for {pseudo}");
            playButton.interactable = false;
            gameManager.ConnectPlayer(pseudo, "default");
        }
    }

    public void ExitButtonHandler()
    {
        gameManager.QuitGame();
    }

}
