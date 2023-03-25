using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

namespace RogueCaml
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        public GameObject weaponPrefab;
        private void Awake()
        {
            // Make this GameObject persistent across all scenes
            DontDestroyOnLoad(gameObject);
        }

        //Since we use GameManager in everyscene we make condition to know from where it is instanciated, will be splitted in two different scripts later on.
        void Start()
        {
            //If we are not connected to server. Server != Rooms.
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.AutomaticallySyncScene = true;
            }
        }

        void Update()
        {

        }

        #region Photon Callbacks

        //When connected to server
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
        }

        public override void OnJoinedRoom()
        {
            //If not master player will automaticly join scene because of AutomaticallySyncScene bool.
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("level_1");
            }
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");
        }

        #endregion


        #region Public Methods
        public void QuitGame()
        {
            PhotonNetwork.Disconnect();
            Application.Quit();
        }

        public void LeaveRoom()
        {
            Debug.Log("Leaving room.");
            PhotonNetwork.LeaveRoom();
        }

        public void ConnectPlayer(string pseudo)
        {
            //if (PlayerController.LocalPlayerInstance == null)
            //{
            //    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            //    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            //    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector2(0f, 0f), Quaternion.identity, 0);
            //    //PhotonNetwork.Instantiate(this.weaponPrefab.name, new Vector2(0f, 0f), Quaternion.identity, 0);
            //}
            PhotonNetwork.NickName = pseudo==null ? "Player" : pseudo;
            Debug.Log($"Connecting to room with pseudo {PhotonNetwork.NickName}");
            PhotonNetwork.JoinOrCreateRoom("default", new RoomOptions() { MaxPlayers = 4}, TypedLobby.Default);
        }
        #endregion


        #region Private Methods

        #endregion

    }


}