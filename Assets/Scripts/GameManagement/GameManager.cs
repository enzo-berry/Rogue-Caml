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
        //Since this script is static it could be better to retrieve it dynamicly:
        //To dynamicly retrieve this script you can call
        //GameObject.Find("GameManager").GetComponent("GameManager");

        [Tooltip("The prefab to use for representing the player")]
        public static GameManager Instance { get; private set; }
        public GameObject playerPrefab;
        public GameObject weaponPrefab;

        private void Awake()
        {
            //if first time we load
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }//If try to get loaded again.
            else
            {
                Destroy(gameObject);
            }
        }

        //Since we use GameManager in everyscene we make condition to know from where it is instanciated, will be splitted in two different scripts later on.
        void Start()
        {
            //If we are not connected to server. Server != Rooms.
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        void Update()
        {

        }

        #region GameFunctions

        //comment me this function for IDE

        /// <summary>
        /// Teleports all players to new room.
        /// CAN ONLY BE CALLED BY MASTER CLIENT !
        /// </summary>
        /// <param name="roomname">The room to teleport.</param>
        /// <returns>true if called by MasterClient, false if not</returns>
        public bool SwitchRoom(string roomname)
        {
            if (PhotonNetwork.IsMasterClient){
                PhotonNetwork.LoadLevel(roomname);
                return true;
            }
            
            return false;
        }

        #endregion

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