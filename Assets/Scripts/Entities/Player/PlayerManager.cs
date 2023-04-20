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
        }

        void Update()
        {
            //If that PlayerObject is my player.
            if (photonView.IsMine && !LevelManager.gameisPaused && alive)
            {
                ProcessInputs();
            }
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
                photonView.RPC("PlayerPickup", RpcTarget.All, ItemToPickupPhotonId);
            }

            //Drop item
            if (Input.GetKeyDown(GameManager.keybinds["drop"]) && weapon)
            {
                //send PlayerDrop RPC to all players
                photonView.RPC("PlayerDrop", RpcTarget.All);

            }

            //Attack
            if (Input.GetKeyDown(GameManager.keybinds["attack"]) && weapon)
            {
                //send PlayerAttack RPC to all players
                photonView.RPC("PlayerAttack", RpcTarget.All);
            }
        }

        #endregion

        #region RPCs

        [PunRPC]
        void PlayerAttack()
        {
            //Get as a Vector2 the direction of the mouse from the player
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            //Set direction to only 1 and 0
            direction.Normalize();
            
            

            weaponscript.Attack(direction);
        }

        [PunRPC]
        void PlayerPickup(int ItemPhotonId)
        {
            //Get item by it's ViewId
            weapon = PhotonView.Find(ItemPhotonId).gameObject;
            weaponscript.owner = this;
            weaponscript.characteristics = Characteristics.Weapon | Characteristics.Equiped;
        }

        [PunRPC]
        void PlayerDrop()
        {
            weaponscript.owner = null;
            weaponscript.characteristics = Characteristics.Weapon;
            weapon = null;
        }

        [PunRPC]
        void ClearWeapon() //used when changing scene for now, in order to prevent sync errors.
        {
            PhotonView.Destroy(weapon);
            weapon = null;
        }

        #endregion

        GameObject GetItemInContactWith()
        {
            foreach(Collider2D collision in ObjectsInContactWithPlayer)
            {
                Item item = collision.gameObject.GetComponent<Item>();
                if (item == null)//If couldn't get Item component,
                    continue;
                if (item.isweapon && !item.isequiped)
                    return collision.gameObject;
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