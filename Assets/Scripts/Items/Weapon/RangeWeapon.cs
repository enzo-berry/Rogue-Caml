using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace RogueCaml
{
    public class RangeWeapon : Weapon
    {
        public GameObject ProjectilePrefab;

        
        private void Update()
        {
            if (attacking) attacking = Time.time - wait < coolDown;
        }

        public override void Attaque(Vector2 direction)
        {
            if(!attacking)
            {
                wait = Time.time;
                attacking = true;
                transform.position = Owner.transform.position + (Vector3)direction;
                GameObject b =
                    PhotonNetwork.Instantiate(ProjectilePrefab.name, transform.position + (Vector3)direction, Quaternion.identity);
                
                Projectile p = b.GetComponent<Projectile>();
                p.direction = direction;
                b.transform.right = (direction);
            }
        }
    }
}
