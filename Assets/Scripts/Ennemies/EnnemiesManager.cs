using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

namespace RogueCaml 
{
    public class EnnemiesManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public int Health = 5;
        public float moveSpeed = 5f;
        public Rigidbody2D rb;

        [FormerlySerializedAs("target")] 
        public GameObject Target;

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

        private char waiting = '\0';
        void FixedUpdate()
        {
            waiting++;
            if(waiting == 20)
            {
                waiting = '\0';
                setTarget();
            }

            Vector2 v = Target.transform.position - transform.position;
            
            float alpha = (float)(v.x==0? (float)(90 * Signe(v.y)) : Math.Atan((float)(v.y/v.x)) + (Signe(v.x)==-1?Math.PI:0));
            Vector2 direction = new Vector2((float)(Math.Cos(alpha)), (float)Math.Sin(alpha));
        
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
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

    }
}