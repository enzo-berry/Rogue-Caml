using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using ExitGames.Client.Photon.StructWrapping;

namespace RogueCaml
{
    public class PlayerManager : Entity
    {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        [HideInInspector]
        public static GameObject OwnedPlayerInstance;
        public GameObject CameraObj;

        private Camera cam => CameraObj.GetComponent<Camera>(); 
        private List<Collider2D> objectsInContactWithPlayer = new List<Collider2D>();

        #region Unity Callbacks

        //making player object permanent
        void Awake()
        {
            if (photonView.IsMine)
            {
                //We set instance of LocalPlayer to var.
                PlayerManager.OwnedPlayerInstance = this.gameObject;
            }
            //PlayerGame object won't be destroyed on changing scene.
            DontDestroyOnLoad(this.gameObject);
        }

        //Setting player infos
        public void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if (!IsMine)
            {
                CameraObj.SetActive(false);
            }
            else
            {
                cam.tag = "MainCamera";
            }
        }

        void Update()
        {
            //If that PlayerObject is my player.
            if (IsMine && !LevelManager.gameisPaused && IsAlive)
            {
                ProcessInputs();
            }

            if (IsMine && Health <= 0)
            {
                Die();
            }
        }

        void FixedUpdate()
        {
            if (IsMine && IsAlive && !LevelManager.gameisPaused)
            {
                rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
            }
        }

        void ProcessInputs()
        {
            //Using Axis, because it's easier to change controls in InputManager.
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            //Pickup item
            if (Input.GetKeyDown(GameManager.keybinds["pickup"]) && !weapon)
            {
                GameObject ItemToPickup = GetItemInContactWith();

                if (ItemToPickup == null) return;

                //get Photon id of item
                int ItemToPickupPhotonId = ItemToPickup.GetComponent<PhotonView>().ViewID;
                //send PlayerPickup RPC to all players
                Pickup(ItemToPickupPhotonId);
            }

            //Drop item
            if (Input.GetKeyDown(GameManager.keybinds["drop"]) && weapon)
            {
                //send PlayerDrop RPC to all players
                Drop();
            }

            //Attack
            if (Input.GetKeyDown(GameManager.keybinds["attack"]) && weapon)
            {
                //send PlayerAttack RPC to all players
                PlayerAttack();
            }
        }

        #endregion

        void PlayerAttack()
        {
            //replace to attack animation
            //Get as a Vector2 the direction of the mouse from the player
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 Pos = transform.position;

            Vector2 direction = mousePosition - Pos;

            //Set direction to only 1 and 0
            direction.Normalize();

            weapon.Attack(direction);
        }

        void Die()
        {
            if (weaponPhotonId != 0)
            {
                Drop();
            }
            IsAlive = false;
        }

        #region RPCs

        [PunRPC]
        void ClearObjectsInContact()
        {
            if (IsMine)
            {
                objectsInContactWithPlayer.Clear();
            }
        }

        #endregion

        GameObject GetItemInContactWith()
        {
            foreach (Collider2D collision in objectsInContactWithPlayer)
            {
                Item item = collision.gameObject.GetComponent<Item>();
                if (item == null)//If couldn't get Item component,
                    continue;
                if (item.IsWeapon && !item.IsEquiped)
                    return collision.gameObject;
            }
            return null;
        }

        //Check when player is in contact with an object
        private void OnTriggerEnter2D(Collider2D collision)
        {
            
            //If player is mine
            if (IsMine)
            {
                
                CollisionManager(collision.gameObject);

                //Used for equiping an Object
                objectsInContactWithPlayer.Add(collision);
            }
        }

        //same as above but when player is not in contact with an objcet anymore
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (IsMine)
            {
                //Used for equiping an Object
                objectsInContactWithPlayer.Remove(collision);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
        }
    }
}
