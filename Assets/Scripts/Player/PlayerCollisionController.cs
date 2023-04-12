using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

namespace RogueCaml
{
    public class PlayerCollisionController : MonoBehaviourPunCallbacks
    {
        public PlayerManager playerController;

        void Start()
        {
            playerController = GetComponent<PlayerManager>();
        }

        // Start is called before the first frame update
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //Debug.Log("collision enter", this);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            //Debug.Log("collision stay", this);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            //Debug.Log("collision exit", this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("trigger enter", this);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Debug.Log("trigger stay", this);
            if (Input.GetButton("Fire1"))
            {
                //Equiping weapon
                if (collision.gameObject.CompareTag("Unequipped") && playerController.weapon==null)
                {
                    collision.gameObject.SendMessage("Pickup", playerController, SendMessageOptions.RequireReceiver);
                    playerController.weapon = collision.gameObject;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //Debug.Log("trigger exit", this);
        }
    }
}

    /*
    void OnCollisionStay(Collision other)
            {
                Debug.LogWarning("çaa", this);
                //static int WeaponCD;
                switch(other.collider.tag)
                {
                    case "Weapon":
                        if(weapon == null)
                        {
                            weapon = other.collider.gameObject;
                            weapon.SendMessage("SetTarget",this, SendMessageOptions.RequireReceiver);
                        }
                        else if(Input.GetButtonDown("e"))
                        {
                            weapon.SendMessage("UnsetTarget",this, SendMessageOptions.RequireReceiver);
                            weapon = other.collider.gameObject;
                            weapon.SendMessage("SetTarget",this, SendMessageOptions.RequireReceiver);
                        }
                        break;
                }
            }
    */