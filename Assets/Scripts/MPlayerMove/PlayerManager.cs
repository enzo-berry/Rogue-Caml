using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace RogueCaml
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        public float moveSpeed = 5f;
        public int attaqueSpeed = 1;
        public int Health = 3;

        public Rigidbody2D rb;
        Vector2 movement;

        

        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        public GameObject PlayerUiPrefab;
        public GameObject WeaponPrefab;


        #region IPunObservable implementation

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
            if(photonView.IsMine)
            {
                ProcessInputs();
            }
        }

        #if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
        #endif

        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
            GameObject _weapon = Instantiate(this.WeaponPrefab);
            _uiGo.SendMessage("SetTarget",this, SendMessageOptions.RequireReceiver);
            _weapon.SendMessage("SetTarget",this, SendMessageOptions.RequireReceiver);
        }

        #if UNITY54ORNEWER
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
        #endif

        void Start()
        {
            if (PlayerUiPrefab != null)
            {
                GameObject _uiGo =  Instantiate(PlayerUiPrefab);
                _uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
                //GameObject _weapon = Instantiate(this.WeaponPrefab);
                //_weapon.SendMessage("SetTarget",this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }

            //a la fin
            #if UNITY54ORNEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            #endif
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
            if(photonView.IsMine)
            {
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            }
        }

        void ProcessInputs()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            
        }
    }
}
