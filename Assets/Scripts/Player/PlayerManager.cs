using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

namespace RogueCaml
{
    public class PlayerManager : Entity
    {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        
        public int attaqueSpeed = 1;

        private Rigidbody2D rigidBody;
        public static Vector2 movement;

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
            if (photonView.IsMine && !LevelManager.gameisPaused)
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

            if (weapon && Input.GetKey(KeyCode.Mouse0))
            {
                //Send a RPC to all connected clients, basicly calls AttackTESTSync method for every client connected.
                //photonView.RPC("AttackTESTSync", RpcTarget.All, PhotonNetwork.NickName);
                
                Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 tmp = transform.position;

                Vector2 v = mp - tmp;

                float alpha = (float)(v.x==0? (float)(90 * Signe(v.y)) : Math.Atan((float)(v.y/v.x)) + (Signe(v.x)==-1?Math.PI:0));
                Vector2 direction = new Vector2((float)(Math.Cos(alpha)), (float)Math.Sin(alpha));
                
                weapon.SendMessage("Attaque", direction, SendMessageOptions.RequireReceiver);
            }
        }

        //Called when a PlayerAttacks.
        [PunRPC]
        void AttackTESTSync(string PlayerAttacked)
        {
            if (weapon is not null)
            {
                weapon.GetComponent<Weapon>().Attaque(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            Debug.Log($"Player {PlayerAttacked} just attacked ! ");
        }

        //Better to use a Getter to keep rigidBody var private.
        public Rigidbody2D GetRigidBody()
        {
            return rigidBody;
        }
        
        int Signe(float f)
        {
            return f > 0 ? 1 : f == 0 ? 0 : -1;
        }
    }
}
