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

        //objects
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        List<Collider2D> ObjectsInContactWithPlayer = new List<Collider2D>();


        public void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            //If that PlayerObject is my player.
            if (photonView.IsMine && !LevelManager.gameisPaused && alive)
            {
                ProcessInputs();
            }
        }

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

        void FixedUpdate()
        {
            if (photonView.IsMine && alive && !LevelManager.gameisPaused)
            {
                rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
            }
        }

        void ProcessInputs()
        {

            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            //Pickup item
            if (Input.GetKeyDown(KeyCode.LeftControl) && !weapon)
            {
                //Debug.Log("trigger stay", this);
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    GameObject ItemToPickup = GetObjectInContactWith();

                    if (ItemToPickup == null) return;
                    
                    weapon = ItemToPickup;
                    int OwnerPhotonViewId = photonView.ViewID;

                    weapon.GetComponent<PhotonView>().RPC("Pickup", RpcTarget.All, OwnerPhotonViewId);
                }
            }

            //Attack
            if (weapon && Input.GetKeyDown(KeyCode.Mouse0))
            {
                weapon.GetComponent<PhotonView>().RPC("Attack", RpcTarget.All);
            }
        }

        GameObject GetObjectInContactWith()
        {
            foreach(Collider2D collision in ObjectsInContactWithPlayer)
            {
                if (!collision.gameObject.tag.Contains("unequiped")) 
                    continue;
                else
                {
                    return collision.gameObject;
                }
            }
            return null;
        }

        //Check when player is in contact with a sword
        private void OnTriggerEnter2D(Collider2D collision)
        {
            ObjectsInContactWithPlayer.Add(collision);
        }

        //same as above but when player is not in contact with a sword
        private void OnTriggerExit2D(Collider2D collision)
        {
            ObjectsInContactWithPlayer.Remove(collision);
        }

    }
}
