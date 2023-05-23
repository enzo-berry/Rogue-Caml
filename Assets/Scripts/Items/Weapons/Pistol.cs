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
        
        public float cooldown;
        private int timeSinceLastShot = 0;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            characteristics = Characteristics.Weapon;
        }

        private void Update()
        {
            if (PhotonOwnerId != 0)
            {
                //teleport to owner
                transform.position = Owner.transform.position;
            }


            if (IsEquiped)
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
                if(Owner != null)
                    transform.position = Owner.transform.position + (Vector3)direction;
                
                GameObject ProjectilObjectCreated = PhotonNetwork.Instantiate(ProjectilePrefab.name, transform.position + (Vector3)direction, Quaternion.identity);
                ProjectilObjectCreated.GetPhotonView().RequestOwnership();
                
                Projectil ProjectilScriptCreated = ProjectilObjectCreated.GetComponent<Projectil>();

                ProjectilScriptCreated.direction = direction;
                ProjectilObjectCreated.transform.right = direction;
                ProjectilScriptCreated.ParentWeaponPhotonId = photonView.ViewID;

                bool IsOnPlayerTeam = ProjectilScriptCreated.ParentWeapon.GetComponent<Weapon>().IsOnPlayerTeam;
                ProjectilScriptCreated.IsOnPlayerTeam = IsOnPlayerTeam;
            }
        }
    }
}
