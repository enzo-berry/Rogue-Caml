using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

namespace RogueCaml
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        public float moveSpeed = 5f;
        public int attaqueSpeed = 1;
        public int Health = 3;

        private Rigidbody2D rigidBody;
        private Vector2 movement;

        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        public GameObject weapon;


        #region IPunObservable implementation

        //SyncingHealth
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(Health);
            }
            else
            {
                // Network player, receive data
                this.Health = (int)stream.ReceiveNext();
            }
        }

        #endregion

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
            }
        }

        /*#if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
        #endif*/

        //void CalledOnLevelWasLoaded(int level)
        //{
        //    // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        //    GameObject _uiGo = Instantiate(this.PlayerStatsUI);

        //    _uiGo.SendMessage("SetTarget",this, SendMessageOptions.RequireReceiver);

        //}

        /*#if UNITY54ORNEWER
        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable ();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #endif

        #if UNITY54ORNEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
        #endif*/

        void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
            }
        }

        void ProcessInputs()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(KeyCode.Mouse0))
            {
                photonView.RPC("AttackTESTSync", RpcTarget.All, PhotonNetwork.NickName);
            }
        }

        //Called when a PlayerAttacks.
        [PunRPC]
        void AttackTESTSync(string PlayerAttacked)
        {
            Debug.Log($"Player {PlayerAttacked} just attacked ! ");
        }

        public Rigidbody2D GetRigidBody()
        {
            return rigidBody;
        }
    }
}
