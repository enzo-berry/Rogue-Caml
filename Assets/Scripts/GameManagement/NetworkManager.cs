using Photon.Pun;
using Photon.Realtime;
using RogueCaml;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueCaml
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public GameObject PlayerPrefab;
        public static NetworkManager Instance;

        //Create a PhotonDuplicateView, dont now how to fix X(, but works good.
        private void Awake()
        {

            if (Instance != null)
            {
                PhotonNetwork.Destroy(Instance.gameObject);
            }

            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            if (PlayerPrefab == null)
            {
                Debug.LogError("Network manager attributes missing, check editor");
            }

            ConnectoToPhoton();
        }

        private void Update()
        {
                
        }


        #region NetworkFunctions

        public void LeaveRoom()
        {
            Debug.Log("Leaving room.");
            PhotonNetwork.LeaveRoom();
        }

        public void ConnectPlayer(string pseudo, string roomGame)
        {
            PhotonNetwork.NickName = pseudo == null ? "Player" : pseudo;
            Debug.Log($"Connecting to room {roomGame} with pseudo {PhotonNetwork.NickName}");
            PhotonNetwork.JoinOrCreateRoom(roomGame, new RoomOptions() { MaxPlayers = 4 }, TypedLobby.Default);
        }

        private void ConnectoToPhoton()
        {
            //If we are not connected to server. Server != Rooms.
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.AutomaticallySyncScene = true;
            }
        }

        #endregion

        #region Overrides

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            MainMenu.Instance.EnablePlayButton();
        }

        public override void OnJoinedRoom()
        {
            //If not master player will automaticly join scene because of AutomaticallySyncScene bool.
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("waiting_scene");
            }
            PhotonNetwork.Instantiate(PlayerPrefab.name, Vector3.zero, Quaternion.identity);
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected from server for reason: " + cause.ToString());
            SceneManager.LoadScene("MainMenu");
            GameManager.Level = 0;
            ConnectoToPhoton();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to Join Room: " + message);
        }

        #endregion

        #region RPC

        [PunRPC]
        private void DestroyObjectRPC(int viewID)
        {
            PhotonView targetPhotonView = PhotonView.Find(viewID);
            if (targetPhotonView != null)
            {
                PhotonNetwork.Destroy(targetPhotonView);
            }
        }


        //RPC for syncing Game Manager stats
        [PunRPC]
        private void SyncStatsRPC(int level, int difficulty, bool friendlyFire)
        {
            GameManager.Level = level;
            GameManager.Difficulty = difficulty;
            GameManager.FriendlyFire = friendlyFire;
        }

        #endregion

    }
}
