using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using ExitGames.Client.Photon.StructWrapping;
using Assets.Scripts;

namespace RogueCaml
{
    public class PlayerManager : Entity
    {
        //objects
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject OwnedPlayerInstance;
        List<Collider2D> ObjectsInContactWithPlayer = new List<Collider2D>();

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
            characteristics = Characteristics.Player;
            IsOnPlayerTeam = true;
        }

        void Update()
        {
            //If that PlayerObject is my player.
            if (IsMine && !LevelManager.gameisPaused && alive)
            {
                ProcessInputs();
            }

            if (IsMine && Health <= 0)
            {
                //Play death animation
                /////////////////////////


                //destroy entity
                GameManager.Instance.DestroyObject(gameObject);
            }
        }

        void FixedUpdate()
        {
            if (IsMine && alive && !LevelManager.gameisPaused)
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
                PlayerPickup(ItemToPickupPhotonId);
            }

            //Drop item
            if (Input.GetKeyDown(GameManager.keybinds["drop"]) && weapon)
            {
                //send PlayerDrop RPC to all players
                PlayerDrop();
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
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            //Set direction to only 1 and 0
            direction.Normalize();

            weapon.Attack(direction);
        }

        void PlayerPickup(int ItemPhotonId)
        {

            weaponPhotonId = ItemPhotonId;

            weapon.PhotonOwnerId = photonView.ViewID;
            weapon.gameObject.GetPhotonView().RequestOwnership();

            weapon.IsEquiped = true;
            weapon.IsOnPlayerTeam = true;
        }

        void PlayerDrop()
        {
            weapon.IsEquiped = false;
            weapon.IsOnPlayerTeam = false;//Either one or the other, so just change it.

            weapon.PhotonOwnerId = 0;
            weaponPhotonId = 0;
        }


        #region RPCs

        [PunRPC]
        void ClearObjectsInContact()
        {
            if (IsMine)
            {
                ObjectsInContactWithPlayer.Clear();
            }
        }

        #endregion

        GameObject GetItemInContactWith()
        {
            foreach (Collider2D collision in ObjectsInContactWithPlayer)
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
                GameObject gameObject = collision.gameObject;

                ObjectCharacteristics objectCharacteristics = gameObject.GetComponent<ObjectCharacteristics>();

                //damaging
                if (objectCharacteristics != null)
                {
                    if (objectCharacteristics.IsProjectil)
                    {
                        Debug.Log("Projectil entered owned player");

                        Projectil projectil = gameObject.GetComponent<Projectil>();
                        //if (projectil.IsOnPlayerTeam != this.IsOnPlayerTeam)
                        {
                            TakeDommage(projectil.dammage);
                            GameManager.Instance.DestroyObject(gameObject);
                        }
                    }
                }

                //Used for equiping an Object
                ObjectsInContactWithPlayer.Add(collision);
            }
        }

        //same as above but when player is not in contact with an objcet anymore
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (IsMine)
            {
                //Used for equiping an Object
                ObjectsInContactWithPlayer.Remove(collision);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
        }
    }
}
