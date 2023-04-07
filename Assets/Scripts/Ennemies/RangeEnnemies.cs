using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using UnityEngine;


namespace RogueCaml
{
    public class RangeEnnemies : EnnemiesManager
    {
        private float securityRange;

        public GameObject WeaponPrefab;
        private Weapon Weapon;
        
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
            if(waiting == 10)
            {
                waiting = '\0';
                setTarget();
            }

            Vector2 v = Target.transform.position - transform.position;
            

            float alpha = (float)(v.x==0? (float)(90 * Signe(v.y)) : Math.Atan((float)(v.y/v.x)) + (Signe(v.x)==-1?Math.PI:0));
            Vector2 direction = new Vector2((float)(Math.Cos(alpha)), (float)Math.Sin(alpha));
        
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
        }
        
    }
}

