using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Diagnostics;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using Debug = UnityEngine.Debug;


/*ToRecode:
 * use RPC for attack rather than OnPhotonSerializeView.
 * RPC : Better for ponctual events and syncing actions: ex: sword attacking
 * OnPhotonSerializeView : Better for fast sync ex: coords/health.
 * 
 * Don't hardcode coordonates, maybe prefer using animations.
*/

namespace RogueCaml
{
    public class Sword : Weapon, IPunObservable
    {
        //unsynced because only owner handles attacking
        private int sens;
        private Vector2 direction = new Vector2(0f, 0f);


        [SerializeField]
        private float cooldown;

        [SerializeField]
        private float angle;

        private float wait = 0;


        //old code
        private float alpha = 0;
        
        private readonly Quaternion correctionOrientation = Quaternion.AngleAxis(-45f, new Vector3(0, 0, 1));


        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            isAttacking = false;
        }

        void Update()
        {
            //swords follows owner
            if (PhotonOwnerId != 0 && photonView.IsMine && !isAttacking)
            {
                transform.position = Owner.transform.position;
            }

            //pour tous les clients
            if (IsEquiped && !isAttacking)
                spriteRenderer.enabled = false;
            else
                spriteRenderer.enabled = true;
        }

        private void FixedUpdate()
        {
            //make sword rotate to attack
            if (isAttacking && photonView.IsMine)
            {
                if (Time.time - wait < cooldown)
                {
                    direction = Quaternion.AngleAxis(sens * Time.fixedDeltaTime * angle / cooldown, //on applique la rotation
                        new Vector3(0, 0, 1)) * direction;
                    
                    transform.right =  correctionOrientation * this.direction;
                    transform.transform.position = Owner.transform.position + (Vector3)this.direction;
                }
                else
                {
                    Stop();
                }
                
            }
        }

        public override void Stop()
        {
            isAttacking = false;
            BlockProjectils = false;
            //reset rotation
            transform.right = Vector3.right;
        }

        public override void Attack(Vector2 direction)
        {
            Debug.Log("Sword attacking !");
            //checking if it mine
            if (photonView.IsMine && !isAttacking)
            {
                BlockProjectils = true;
                
                direction.Normalize();
                sens = direction.x > 0 ? -1 : 1;
                
                // on tourne l'epee de angle/2 dans le sens opposé de rotation
                this.direction = Quaternion.AngleAxis(-sens * angle / 2, new Vector3(0, 0, 1)) * direction;
                
                
                transform.right =  correctionOrientation * this.direction; //on oriente l'epee de dans le bon sens en prenant en compte qu'elle est tordu
                transform.transform.position = Owner.transform.position + (Vector3)this.direction; 

                //spriteRenderer.enabled = true;
                wait = Time.time;
                isAttacking = true;
            }
        }


        public override int GetDammage()
        {
            return Dammage + Owner.GetComponent<Entity>().BonusDammage;
        }
    }
}
