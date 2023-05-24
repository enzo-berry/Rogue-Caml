using Photon.Pun;
using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace RogueCaml
{
    //Used for Player and Mobs
    public abstract class Entity : ObjectCharacteristics, IPunObservable
    {
    //Synced in PhotonView
        public int Health = 5;
        public int moveSpeed = 5;
        public int attackSpeed = 1;

        [NonSerialized] public bool alive = true; //NonSerialazed means it won't be accesible in the inspector.
        [NonSerialized] public Vector2 movement; //A vector2 to store the movement of the player. is used in Update method.

        public int weaponPhotonId = 0;
        public bool IsMine
        {
            get
            {
                return photonView.IsMine;
            }
        }

    //Unsynced
        public Weapon weapon{ 
            get 
            {
                
                if (weaponPhotonId == 0)
                {
                    return null;
                }
                else
                {
                    PhotonView photonView = PhotonNetwork.GetPhotonView(weaponPhotonId);
                    return photonView.GetComponent<Weapon>();
                }
            }
        }

        protected Rigidbody2D rb;

        public new void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(Health);
                stream.SendNext(moveSpeed);
                stream.SendNext(attackSpeed);
                stream.SendNext(movement);
                stream.SendNext(alive);
                stream.SendNext(weaponPhotonId);
            }
            else
            {
                // Network player, receive data
                Health = (int)stream.ReceiveNext();
                moveSpeed = (int)stream.ReceiveNext();
                attackSpeed = (int)stream.ReceiveNext();
                movement = (Vector2)stream.ReceiveNext();
                alive = (bool)stream.ReceiveNext();
                weaponPhotonId = (int)stream.ReceiveNext();
            }

            //Syncing characteristics
            base.OnPhotonSerializeView(stream, info);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //If player is mine
            if (IsMine)
            {
                CollisionManager(collision.gameObject);
            }
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //If player is mine
            if (IsMine)
            {
                CollisionManager(collision.gameObject);
            }
        }

        protected void Pickup(int ItemPhotonId)
        {
            //Is only called on the possesor of the player on its player.
            //Order of declaration is important since weapon getter uses weaponPhotonId.
            weaponPhotonId = ItemPhotonId;

            weapon.PhotonOwnerId = photonView.ViewID;
            weapon.gameObject.GetPhotonView().RequestOwnership();

            weapon.IsEquiped = true;
            weapon.IsOnPlayerTeam = true;
        }

        protected void Drop()
        {
            //Order is important since weapon getter uses weaponPhotonId.
            weapon.IsEquiped = false;
            weapon.IsOnPlayerTeam = false;//Either one or the other, so just change it.

            weapon.PhotonOwnerId = 0;
            weaponPhotonId = 0;
        }

        public void TakeDammage(int amount)
        {
            Debug.Log("Entity took " + amount.ToString() + " damage");
            Health -= amount;
        }
        
        
        protected void CollisionManager(GameObject collisionGameObject)
        {
            ObjectCharacteristics objectCharacteristics = collisionGameObject.GetComponent<ObjectCharacteristics>();

            //damaging
            if (objectCharacteristics != null)
            {
                // ou c'est un ennemy et je suis un joueur
                  if (objectCharacteristics.IsProjectil) //regarde si c'est un projectile
                  {
                      Debug.Log($"Projectil entered {gameObject.name}");

                      Projectil projectil = collisionGameObject.GetComponent<Projectil>();
                      
                      if ((GameManager.FriendlyFire || //si friendly fire alors oui sinon 
                           (projectil.IsEnemy && IsPlayer) || // je suis un joeur et c'est un ennemy
                           (projectil.IsPlayer && IsEnemy)))
                      {
                          TakeDammage(projectil.dammage);
                          GameManager.Instance.DestroyObject(collisionGameObject);
                      }
                  }

                  if (objectCharacteristics.IsWeapon && objectCharacteristics.IsEquiped)
                  {
                      Debug.Log($"{gameObject.name} got hit by a weapon");

                      Weapon weapon = collisionGameObject.GetComponent<Weapon>();

                      if (weapon != null && weapon.PhotonOwnerId != photonView.ViewID)
                      {
                          TakeDammage(weapon.dammage);
                      }

                  }
                    
                    
                
                    
            }
        }

    }
}