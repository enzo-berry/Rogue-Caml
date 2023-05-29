using Photon.Pun;
using UnityEngine.UIElements;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace RogueCaml
{
    //Used for Player and Mobs
    public abstract class Entity : ObjectCharacteristics, IPunObservable
    {
        //Synced in PhotonView
        public int MaxHealth = 5;
        public int Health;
        public int moveSpeed = 5;
        public int attackSpeed = 1;
        public int BonusDammage = 0;
        public int Armor = 0;
        public int collisionDammage = 1;

        public bool IsAlive => Health > 0; //NonSerialazed means it won't be accesible in the inspector.
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

        public int CollisionDammage
        {
            get
            {
                return collisionDammage + BonusDammage;
            }
        }

        private void Awake()
        {
            Health = MaxHealth;
        }

        protected Rigidbody2D rb;

        public new void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(MaxHealth);
                stream.SendNext(Health);
                stream.SendNext(moveSpeed);
                stream.SendNext(attackSpeed);
                stream.SendNext(movement);
                stream.SendNext(weaponPhotonId);
                stream.SendNext(BonusDammage);
                stream.SendNext(Armor);
                stream.SendNext(collisionDammage);
            }
            else
            {
                // Network player, receive data
                MaxHealth = (int)stream.ReceiveNext();
                Health = (int)stream.ReceiveNext();
                moveSpeed = (int)stream.ReceiveNext();
                attackSpeed = (int)stream.ReceiveNext();
                movement = (Vector2)stream.ReceiveNext();
                weaponPhotonId = (int)stream.ReceiveNext();
                BonusDammage = (int)stream.ReceiveNext();
                Armor  = (int)stream.ReceiveNext();
                collisionDammage = (int)stream.ReceiveNext();
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
            Debug.Log("try pickup");
            Item item = PhotonNetwork.GetPhotonView(ItemPhotonId).gameObject.GetComponent<Item>();

            if (item.IsWeapon)
            {
                //Is only called on the possesor of the player on its player.
                //Order of declaration is important since weapon getter uses weaponPhotonId.
                if (weapon)
                {
                    Drop();
                }
            
            
                weaponPhotonId = ItemPhotonId;

                weapon.PhotonOwnerId = photonView.ViewID;
                weapon.gameObject.GetPhotonView().RequestOwnership();
            
                DontDestroyOnLoad(weapon.gameObject);

                weapon.IsEquiped = true;
                weapon.IsOnPlayerTeam = IsOnPlayerTeam;
                weapon.IsEnemy = IsEnemy;
                weapon.IsPlayer = IsPlayer;
            }
        }
        
        public void Update()
        {
            //Master client will handle killing enemies.
            if(Health <= 0 && photonView.IsMine)
            {
                Kill();
            }

            if (weapon == null)
                weaponPhotonId = 0;
        }

        public virtual void Kill()
        {
            if (weapon)
                GameManager.Instance.DestroyObject(weapon.gameObject);
            GameManager.Instance.DestroyObject(gameObject);
        }

        protected void Drop()
        {
            SceneManager.MoveGameObjectToScene(weapon.gameObject, SceneManager.GetActiveScene());
            
            //Order is important since weapon getter uses weaponPhotonId.
            weapon.IsEquiped = false;
            weapon.IsOnPlayerTeam = false;//Either one or the other, so just change it.

            weapon.Stop();

            weapon.PhotonOwnerId = 0;
            weaponPhotonId = 0;
        }

        public virtual void TakeDammage(int amount)
        {
            Debug.Log("Entity took " + amount.ToString() + " damage");
            if(Health > 0 && amount > Armor)
                Health -= amount - Armor;
        }


        protected void CollisionManager(GameObject collisionGameObject)
        {
            ObjectCharacteristics objectCharacteristics = collisionGameObject.GetComponent<ObjectCharacteristics>();
            if (objectCharacteristics == null)
                return;

            //damaging
            if ((GameManager.FriendlyFire || //si friendly fire alors oui sinon 
                 (objectCharacteristics.IsEnemy && IsPlayer) || // je suis un joeur et c'est un ennemy
                 (objectCharacteristics.IsPlayer && IsEnemy)))
            {
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
                        TakeDammage(weapon.Dammage);
                    }

                }

                if (objectCharacteristics.IsEntity)
                {
                    Debug.Log($"{gameObject.name} got hit by a {collisionGameObject.name}");
                    Entity entity = (Entity)objectCharacteristics;

                    TakeDammage(entity.CollisionDammage);
                }
            }

            if (objectCharacteristics.IsConsumable)
            {
                if (!IsPlayer)
                    return;

                Consumable consumable = collisionGameObject.GetComponent<Consumable>();
                consumable.Apply(gameObject.GetComponent<PlayerManager>());

            }
        }

        protected void CollisionExitManager(GameObject collisionGameObject)
        {

        }
    }
}