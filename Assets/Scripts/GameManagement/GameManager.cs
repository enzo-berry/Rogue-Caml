using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine.Audio;
using Debug = UnityEngine.Debug;

namespace RogueCaml
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        //musics and sound settings
        public AudioSource mainMenu;
        public AudioSource waitRoom;
        public AudioSource firstLevel;
        public AudioSource secondLevel;
        public AudioSource thirdLevel;
        
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
                MainMenu.Instance.playButton.interactable = false;
                DontDestroyOnLoad(gameObject);
            }//If try to get loaded again.
            else
            {
                Destroy(gameObject);
            }
        }

        //Since we use GameManager in every scene we make condition to know from where it is instanciated, will be splitted in two different scripts later on.
        void Start()
        {
            AudioListener.volume = (float)0.5;

            //defining controls
            keybinds.Add("attack", KeyCode.Mouse0);
            keybinds.Add("pickup", KeyCode.LeftControl);
            keybinds.Add("drop", KeyCode.A);
            keybinds.Add("interact", KeyCode.E);

            ConnectoToPhoton();
        }

        #region Photon Callbacks

        //When connected to server
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            MainMenu.Instance.playButton.interactable = true;
        }

        public override void OnJoinedRoom()
        {
            //If not master player will automaticly join scene because of AutomaticallySyncScene bool.
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("waiting_scene");
                PlayMusic(level);
            }
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);            
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected from server for reason: " + cause.ToString());
            SceneManager.LoadScene("MainMenu");
            level = 0;
            PlayMusic(level);
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
                    //Cleans ObjectsInContact with each player
                    player.photonView.RPC("ClearObjectsInContact", RpcTarget.All);
                }

            }

            return false;
        }

        public bool NextLevel()
        {
            level++;
            PlayMusic(level);
            return PhotonNetwork.IsMasterClient && SwitchRoom($"level_{level}");
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


        #region AudioManager
        
        /// <summary>
        /// Starts the music when the client master changes scene
        /// CALL AT EACH SCENE CHANGE OF THE MASTER CLIENT ONLY
        /// </summary>
        /// <param name="currentLevel">the current value of level</param>
        /// <returns>void - juste change played the correct song</returns>
        private void PlayMusic(int currentLevel)
        {
            // The music played on the previous configuration will not be stopped.
            // This must be managed at the time of the scene change (actual status : not managed)
            switch (currentLevel)
            {
                case 1:
                    firstLevel.Play();
                    break;
                case 2 :
                    secondLevel.Play();
                    break;
                case 3 :
                    thirdLevel.Play();
                    break;
                default:
                    Debug.Log("No music played in this configuration!");
                    break;
            }
        }

        
        /// <summary>
        /// Starts the music when the client master changes scene (previous configuration)
        /// CALLED WHEN ROOM CHANGED OR MASTER CLIENT DISCONNECT
        /// </summary>
        /// <param name="previousLevel">The value of the previous level attribute</param>
        /// <returns>void - juste change stopped the correct song</returns>
        private void StopMusic(int previousLevel)
        {
            switch (previousLevel)
            {
                case 0:
                    waitRoom.Stop();
                    break;
                case 1:
                    firstLevel.Stop();
                    break;
                case 2:
                    secondLevel.Stop();
                    break;
                default:
                    Debug.Log("No music stop in this configuration!");
                    break;
            }
        }
        
        
        #endregion
        
        #region Private Methods

        void ConnectoToPhoton()
        {
            //If we are not connected to server. Server != Rooms.
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.AutomaticallySyncScene = true;
            }
        }

        #endregion



    }


}