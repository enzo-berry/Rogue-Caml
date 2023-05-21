using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RogueCaml
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        //stats
        public static int level = 0;
        public static int difficulty = 50;

        //keybinds
        public static Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();

        //objects
        public static GameManager Instance { get; private set; }
        public GameObject playerPrefab;


        //making GameManeger permanenent between scenes
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
            //Screen.SetResolution(Screen.width, Screen.height, true);


            //defining controls
            keybinds.Add("attack", KeyCode.Mouse0);
            keybinds.Add("pickup", KeyCode.LeftControl);
            keybinds.Add("drop", KeyCode.A);
            keybinds.Add("interact", KeyCode.E);

            //If we are not connected to server. Server != Rooms.
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.AutomaticallySyncScene = true;
            }
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
                PhotonNetwork.LoadLevel("waiting_scene");
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

        public void ConnectPlayer(string pseudo, string roomGame)
        {
            PhotonNetwork.NickName = pseudo == null ? "Player" : pseudo;
            Debug.Log($"Connecting to room {roomGame} with pseudo {PhotonNetwork.NickName}");
            PhotonNetwork.JoinOrCreateRoom(roomGame, new RoomOptions() { MaxPlayers = 4 }, TypedLobby.Default);
        }

        /// <summary>
        /// Delets a gameobject from the network.
        /// This method sends a RPC the GameObject owner in order to delete it.
        /// </summary>
        /// <param name="go">The GameObject to destroy, will be casted to a PhotonView.</param>
        /// <returns>true if success, false if not</returns>
        public bool DestroyObject(GameObject go)
        {
            PhotonView pv = go.GetComponent<PhotonView>();
            if (pv == null)
            {
                Debug.LogError("GameObject does not have a PhotonView !");
                return false;

            }
            //if PhotonView is mine, no need to send a RPC to localclient
            if (pv.IsMine)
            {
                PhotonNetwork.Destroy(pv);
                return true;
            }



            photonView.RPC("DestroyObjectRPC", pv.Owner, pv.ViewID);
            return true;
        }

        [PunRPC]
        private void DestroyObjectRPC(int viewID)
        {
            PhotonView targetPhotonView = PhotonView.Find(viewID);
            if (targetPhotonView != null)
            {
                PhotonNetwork.Destroy(targetPhotonView);
            }
        }

        #endregion

        #region GameFunctions

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
                //fetch all players scripts
                PlayerManager[] players = FindObjectsOfType<PlayerManager>();

                //send them an RPC to teleport them to the new room
                foreach (PlayerManager player in players)
                {
                    player.photonView.RPC("ClearWeapon", RpcTarget.All);
                }

            }

            return false;
        }

        public bool NextLevel()
        {
            return PhotonNetwork.IsMasterClient && SwitchRoom($"level_{++level}");
        }

        public void StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                NextLevel();
            }
        }

        #endregion


        #region Private Methods

        #endregion

    }


}