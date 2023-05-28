using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using ExitGames.Client.Photon.StructWrapping;
using System.Linq;

namespace RogueCaml
{
    public class PlayerManager : Entity
    {
        public static List<Item> Items;
        
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        [HideInInspector]
        public static GameObject OwnedPlayerInstance;
        public GameObject CameraObj;
        public GameObject computer;

        private Camera cam => CameraObj.GetComponent<Camera>(); 
        public List<Collider2D> objectsInContactWithPlayer = new List<Collider2D>();
        private SpriteRenderer sr;
        private int counter = 0;

        public GameObject UI;
        #region Unity Callbacks

        //making player object permanent
        void Awake()
        {
            if (photonView.IsMine)
            {
                //We set instance of LocalPlayer to var.
                PlayerManager.OwnedPlayerInstance = this.gameObject;
                Health = MaxHealth;
            }
            else
            {
                UI.SetActive(false);
            }
            //PlayerGame object won't be destroyed on changing scene.
            DontDestroyOnLoad(this.gameObject);
        }

        //Setting player infos
        public void Start()
        {
            sr = GetComponent<SpriteRenderer>();
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
            objectsInContactWithPlayer = objectsInContactWithPlayer.FindAll(a => a != null);

            if (counter % 25 == 0)
            {
                if (IsAlive)
                    setSpriteToNormalTransparence();
                else
                    setSpriteToTransparent();
            }


            if (!IsMine)
                return;

            if (IsAlive && !LevelManager.gameisPaused)
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
                GameObject ItemToPickup = GetWeaponInContactWith();
                objectsInContactWithPlayer.RemoveAll(x => x.gameObject == ItemToPickup);

                if (ItemToPickup == null) return;

                //get Photon id of item
                int ItemToPickupPhotonId = ItemToPickup.GetComponent<PhotonView>().ViewID;
                //send PlayerPickup RPC to all players
                Pickup(ItemToPickupPhotonId);
            }

            //Drop item
            if (Input.GetKeyDown(GameManager.keybinds["drop"]) && weapon)
            {
                Drop();
            }

            //Attack
            if (Input.GetKeyDown(GameManager.keybinds["attack"]) && weapon)
            {
                PlayerAttack();
            }

            if (Input.GetKeyDown(GameManager.keybinds["interact"]) && objectsInContactWithPlayer.Count > 0)
            {
                HandleInteract();
            }
        }

        void setSpriteToTransparent()
        {
            sr.material = new Material(sr.material);

            // Set the transparency value to 50% (0.5)
            Color spriteColor = sr.material.color;
            spriteColor.a = 0.5f;
            sr.material.color = spriteColor;
        }

        void setSpriteToNormalTransparence()
        {
            sr.material = new Material(sr.material);

            // Set the transparency value to 50% (0.5)
            Color spriteColor = sr.material.color;
            spriteColor.a = 1f;
            sr.material.color = spriteColor;
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

        void HandleInteract()
        {

            Collider2D[] ColliderThatHaveChar = objectsInContactWithPlayer.FindAll(col => col.gameObject.GetComponent<ObjectCharacteristics>()!=null).ToArray();

            if (ColliderThatHaveChar.Length == 0)
                return;
            
            ObjectCharacteristics objChar  = ColliderThatHaveChar[0].gameObject.GetComponent<ObjectCharacteristics>();

            if (objChar.IsInteractble)
            {
                ((InterractableItem)objChar).Interact();
            }

            
        }

        void Die()
        {
            if (weaponPhotonId != 0)
            {
                Drop();
            }
        }

        #region RPC

        [PunRPC]
        void ClearObjectsInContact()
        {
            if (IsMine)
            {
                objectsInContactWithPlayer.Clear();
            }
        }

        #endregion

        GameObject GetWeaponInContactWith()
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
    }
}
