using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public string sceneNameToLogin; 
    public TMP_InputField inputField;
    public Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    public void ConnectPlayerToLevel()
    {
        playButton.interactable = false;
        PhotonNetwork.NickName = inputField.text == "" ? "Player " : inputField.text;
        //To change default scene -> OnJoinedRoom() wich is overidden.
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(sceneNameToLogin);
    }

    //Gets called by exit button, parametered in UI.
    public void QuitGame()
    {
        Application.Quit();
    }
}
