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
        
        
        private int timeSinceLastShot = 0;
        private AudioSource audioData;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            characteristics = Characteristics.Weapon;

            Range = ProjectilePrefab.GetComponent<Projectil>().speed * 0.5f;

            audioData = GetComponent<AudioSource>();
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

        public override void Stop()
        {
            isAttacking = false;
            BlockProjectils = false;
            //reset rotation
            transform.right = Vector3.right;
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
                audioData.Play();
                timeSinceLastShot = 0;
                if(Owner != null)
                    transform.position = Owner.transform.position + (Vector3)direction;
                
                GameObject ProjectilObjectCreated = PhotonNetwork.Instantiate(ProjectilePrefab.name, transform.position + (Vector3)direction, Quaternion.identity);
                ProjectilObjectCreated.GetPhotonView().RequestOwnership();
                
                Projectil ProjectilScriptCreated = ProjectilObjectCreated.GetComponent<Projectil>();

                ProjectilScriptCreated.direction = direction;
                ProjectilObjectCreated.transform.right = direction;
                ProjectilScriptCreated.ParentWeaponPhotonId = photonView.ViewID;

                ProjectilScriptCreated.characteristics =  Owner.GetComponent<Entity>().characteristics | Characteristics.Projectil;
                //delete entity characteristics
                ProjectilScriptCreated.characteristics &= ~Characteristics.Entity;

                bool IsOnPlayerTeam = ProjectilScriptCreated.ParentWeapon.GetComponent<Weapon>().IsOnPlayerTeam;
                ProjectilScriptCreated.IsOnPlayerTeam = IsOnPlayerTeam;
            }
        }

        public override int GetDammage()
        {
            return Owner.GetComponent<Entity>().BonusDammage + Dammage;
        }
    }
}
