using Photon.Pun;
using UnityEngine.UIElements;
using UnityEngine;
using System;
using Assets.Scripts;

namespace RogueCaml
{
    //Used for Player and Mobs
    public abstract class Entity : ObjectCharacteristics
    {
    //Synced in PhotonView
        public int Health = 5;
        public int moveSpeed = 5;
        public int attackSpeed = 1;

        [NonSerialized] public bool alive = true;
        [NonSerialized] public Vector2 movement; //A vector2 to store the movement of the player. is used in Update method.
    
    //Unsynced
        //objects
        [NonSerialized] public GameObject weapon = null;
        public Weapon weaponscript => weapon!=null ? weapon.GetComponent<Weapon>() : null;
        protected Rigidbody2D rb;


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(Health);
                stream.SendNext(moveSpeed);
                stream.SendNext(attackSpeed);
                stream.SendNext(movement);
                stream.SendNext(alive);
                stream.SendNext(characteristics);
            }
            else
            {
                // Network player, receive data
                Health = (int)stream.ReceiveNext();
                moveSpeed = (int)stream.ReceiveNext();
                attackSpeed = (int)stream.ReceiveNext();
                movement = (Vector2)stream.ReceiveNext();
                alive = (bool)stream.ReceiveNext();
                characteristics = (Characteristics)stream.ReceiveNext();

            }
        }

        public void TakeDommage(int amount)
        {
            Health -= amount;
        }

    }
}