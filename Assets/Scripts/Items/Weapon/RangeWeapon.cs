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
                GameObject b =
                    PhotonNetwork.Instantiate(ProjectilePrefab.name, transform.position + (Vector3)direction, Quaternion.identity);

                b.GetComponent<Projectile>().direction = direction;
            }
        }
    }
}
