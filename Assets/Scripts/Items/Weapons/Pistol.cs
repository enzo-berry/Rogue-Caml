using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

namespace RogueCaml
{
    public class Pistol : Weapon
    {
        public GameObject ProjectilePrefab;
        public int cooldown; //value set in editor
        private int timeSinceLastShot = 0;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            characteristics = Characteristics.Weapon;
        }

        private void Update()
        {
            if (isequiped)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }
        }

        private void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                if (timeSinceLastShot < cooldown)
                {
                    timeSinceLastShot++;
                }
            }


        }

        public override void Attack(Vector2 direction)
        {
            if (photonView.IsMine && timeSinceLastShot >= cooldown)
            {
                timeSinceLastShot = 0;
                transform.position = owner.transform.position + (Vector3)direction;
                GameObject ProjectilObjectCreated = PhotonNetwork.Instantiate(ProjectilePrefab.name, transform.position + (Vector3)direction, Quaternion.identity);
                Projectile ProjectilScriptCreated = ProjectilObjectCreated.GetComponent<Projectile>();

                ProjectilScriptCreated.direction = direction;
                ProjectilObjectCreated.transform.right = (direction);
            }
        }
    }
}
