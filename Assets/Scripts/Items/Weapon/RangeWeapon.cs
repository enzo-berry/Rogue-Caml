using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

namespace RogueCaml
{
    public class RangeWeapon : Weapon
    {
        public GameObject ProjectilePrefab;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            Hidden = true;
            range = 0.5f * ProjectilePrefab.GetComponent<Projectile>().speed;
        }
        
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
                b.tag = $"ap{Owner.tag[2]}";
                p.Team = Owner.tag[2];
                p.Owner = Owner;
                p.direction = direction;
                b.transform.right = (direction);
            }
        }
    }
}
