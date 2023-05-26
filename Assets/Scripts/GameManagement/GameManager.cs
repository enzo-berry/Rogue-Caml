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
        /*
        //musics and sound settings
        public AudioSource mainMenu;
        public AudioSource waitRoom;
        public AudioSource firstLevel;
        public AudioSource secondLevel;
        public AudioSource thirdLevel;
        */

        //stats
        public static int Level = 0;
        public static int Difficulty = 50;
        public static bool FriendlyFire = false;

        //keybinds
        public static Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();

        //objects
        public static GameManager Instance { get; set; }

        //making GameManeger permanenent between scenes
        private void Awake()
        {
            //if first time we load
            if (Instance == null)
            {
                Instance = this;
                //getting mainmenu

                MainMenu.Instance.DisablePlayButton();
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

        }

        #region GameFunctions

        /// <summary>
        /// Teleports all players to new room.
        /// CAN ONLY BE CALLED BY MASTER CLIENT !
        /// </summary>
        /// <param name="roomname">The room to teleport.</param>
        /// <returns>true if called by MasterClient, false if not</returns>
        private bool SwitchRoom(string roomname)
        {
            if (PhotonNetwork.IsMasterClient)
            {
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

        /// <summary>
        /// Can only be called by Master, in a Transition script normally.
        /// </summary>
        /// <returns></returns>
        public bool NextLevel()
        {
            Level++;
            SyncStats();
            return PhotonNetwork.IsMasterClient && SwitchRoom($"level_{Level}");
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

        public void QuitGame()
        {
            NetworkManager.Instance.LeaveRoom();
            Application.Quit();
        }

        #endregion

/*
        #region AudioManager

        /// <summary>
        /// Starts the music when the client master changes scene
        /// Call at each scene change
        /// </summary>
        /// <returns>void - juste change played the correct song</returns>
        public void PlayMusic()
        {
            // The music played on the previous configuration will not be stopped.
            // This must be managed at the time of the scene change (actual status : not managed)
            switch (Level)
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
        /// <returns>void - juste change stopped the correct song</returns>
        public void StopMusic()
        {
            switch (Level)
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
        
        */

        //To Delete, after reflection, is useless, waiting all merges to do so.
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


            NetworkManager.Instance.photonView.RPC("DestroyObjectRPC", pv.Owner, pv.ViewID);
            return true;
        }

        /// <summary>
        /// Wrapper of the RPC, is only called by the master client, for the other players to sync their stats.
        /// </summary>
        public void SyncStats()
        {
            //Send struct to all players
            NetworkManager.Instance.photonView.RPC("SyncStatsRPC", RpcTarget.OthersBuffered, Level,Difficulty,FriendlyFire);
        }


    }


}