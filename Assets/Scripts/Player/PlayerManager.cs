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

        void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            //If that PlayerObject is my player.
            if (photonView.IsMine)
            {
                ProcessInputs();
            }
        }

        //When game get launched
        void Awake()
        {
            if (photonView.IsMine)
            {
                //We set instance of LocalPlayer to var.
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            //PlayerGame object won't be destroyed on changing scene.
            DontDestroyOnLoad(this.gameObject);
        }

        #region IPunObservable implementation

        //SyncingHealth using OnPhotonSerializeView.
        //Could be done in a graphical way just like the Animations ?
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
                //Send a RPC to all connected clients, basicly calls AttackTESTSync method for every client connected.
                photonView.RPC("AttackTESTSync", RpcTarget.All, PhotonNetwork.NickName);
            }
        }

        //Called when a PlayerAttacks.
        [PunRPC]
        void AttackTESTSync(string PlayerAttacked)
        {
            Debug.Log($"Player {PlayerAttacked} just attacked ! ");
        }

        //Better to use a Getter to keep rigidBody var private.
        public Rigidbody2D GetRigidBody()
        {
            return rigidBody;
        }
    }
}
