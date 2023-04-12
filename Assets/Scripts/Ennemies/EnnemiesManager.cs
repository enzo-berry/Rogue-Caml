using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace RogueCaml 
{
    public class EnnemiesManager : Entity
    {
        protected float range;
        [FormerlySerializedAs("target")] 
        public GameObject Target;
        protected char waiting = '\0';
        
        protected Rigidbody2D rb;

        public Vector2 direction;
        protected Vector2 TargetDirection;
        protected float RandomRotation = 180f;
        
        
        public GameObject WeaponPrefab;
        protected Weapon Weapon;
        
        

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(Health);
            }
            else
            {
                    // Network player, receive data
                this.Health = (int)stream.ReceiveNext();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            setTarget();
            setDirection();
        }

        // Update is called once per frame
        public void Update()
        {
            //Master client will handle killing enemies.
            if(Health <= 0 && PhotonNetwork.IsMasterClient)
            {
                Kill();
            }
        }

        
        void FixedUpdate()
        {
            waiting++;
            if(waiting == 20)
            {
                waiting = '\0';
                setTarget();
            }

            if (waiting % 5 == 0)
            {
                setDirection();
                if(Weapon)
                    Weapon.Attaque(TargetDirection);
            }

            transform.position += (Vector3)(moveSpeed * Time.fixedDeltaTime * direction);
            //rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
        }

        protected void setTarget()
        {
            PlayerManager[] players = FindObjectsOfType<PlayerManager>();
            double d = 10000000000000;

            for (int i = 0; i < players.Length; i++)
            {
                if (Distance(players[i].transform.position - transform.position) < d)
                {
                    Target = players[i].gameObject;
                    d = Distance(Target.transform.position - transform.position);
                }
            }
        }

        public void Kill()
        {
            if(Weapon) PhotonNetwork.Destroy(this.Weapon.gameObject);
            PhotonNetwork.Destroy(this.gameObject);
        }

        public void TakeDommage(int amount)
        {
            Health -= amount;
        }

        protected double Distance(Vector2 v)
        {
            return (float)(Math.Sqrt(v.x * v.x + v.y * v.y));
        }

        protected int Signe(float f) 
        {
            return f>0? 1 : f==0? 0 : -1;
        }
        
        protected void setDirection()
        {
            Vector2 v = Target.transform.position - transform.position;
                
            float alpha = (float)(v.x==0? (float)(90 * Signe(v.y)) : Math.Atan((v.y/v.x)) + (Signe(v.x)==-1?Math.PI:0));
            direction = new Vector2((float)(Math.Cos(alpha)), (float)Math.Sin(alpha));

            TargetDirection = new Vector2(direction.x, direction.y);

            if (Distance(v) < Weapon.range) direction = -direction;
            
            
            
            //direction = Quaternion.AngleAxis(Random.Range(-RandomRotation, RandomRotation), Vector3.left) * direction;
        }

    }
}