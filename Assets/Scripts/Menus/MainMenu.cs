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
            NetworkManager.Instance.ConnectPlayer(pseudo, "default");
        }
    }

    public void ExitButtonHandler()
    {
        GameManager.Instance.QuitGame();
    }

}
