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
        //Incomprehensible X)

        protected float range;
        [FormerlySerializedAs("target")] 
        public GameObject Target;
        protected char waiting = '\0';

        public MobGenerator mobGenerator;
        
        public Vector2 direction;
        protected Vector2 TargetDirection;
        protected float RandomRotation = 180f;

        public GameObject WeaponPrefab;


        // Start is called before the first frame update
        void Start()
        {
            collisionDammage = 1;
            setTarget();
            SetDirection();
        }
        
        

        void FixedUpdate()
        {
            waiting++;
            if(waiting == 20)
            {
                waiting = '\0';
                setTarget();
            }

           
            SetDirection();
            if (weapon)
            {
                TargetDirection = (Target.transform.position - transform.position);
                TargetDirection.Normalize();
                weapon.Attack(TargetDirection);
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

        public virtual void Kill()
        {
            if(weapon) GameManager.Instance.DestroyObject(this.weapon.gameObject);
            if(mobGenerator) mobGenerator.EnnemyDied();
            GameManager.Instance.DestroyObject(this.gameObject);
        }

        protected double Distance(Vector2 v)
        {
            return (float)Math.Sqrt(v.x * v.x + v.y * v.y);
        }

        protected int Signe(float f) 
        {
            return f>0? 1 : f==0? 0 : -1;
        }
        
        protected void SetDirection()
        {
            Vector2 d = Target.transform.position - transform.position;
            
            direction = d;
            if (Distance(this.direction) < range) direction = -direction;
            direction.Normalize();
            
            //direction = Quaternion.AngleAxis(Random.Range(-RandomRotation, RandomRotation), new Vector3(0,0,1)) * direction;
        }
    }
}