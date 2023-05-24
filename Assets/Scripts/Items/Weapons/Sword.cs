using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using Unity.VisualScripting;


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
        private int sens => direction.x > 0 ? -1 : 1;
        private Vector2 direction = new Vector2(0f, 0f);

        [SerializeField]
        private bool isAttacking;

        [SerializeField]
        private int cooldown;

        [SerializeField]
        private int angle;

        private float wait = 0;


        //old code
        private float alpha = 0;


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
                    alpha = (float)((alpha + sens * (angle * Time.fixedDeltaTime / cooldown)));
                    transform.eulerAngles = new Vector3(0f, 0f, alpha - 45f);

                    alpha *= (float)(Math.PI / 180f);

                    direction = new Vector2((float)(Math.Cos(alpha)), (float)Math.Sin(alpha));
                    this.transform.position = Owner.transform.position + (Vector3)direction;
                    alpha *= (float)(180f / Math.PI);

                }
                else
                {
                    isAttacking = false;
                }
                
            }
        }

        public override void Attack(Vector2 direction)
        {
            Debug.Log("Sword attacking !");
            //checking if it mine
            if (photonView.IsMine && !isAttacking)
            {
                alpha = (float)(direction.x == 0 ? (float)(90 * Signe(direction.y)) : Math.Atan((float)(direction.y / direction.x)) + (Signe(direction.x) == -1 ? Math.PI : 0));
                alpha = (float)(alpha + (-sens * Math.PI / 2));

                this.transform.position = Owner.transform.position + (Vector3)direction;
                alpha *= (float)(180f / Math.PI);

                transform.right = direction;
                transform.Rotate(new Vector3(0, 0, angle / 2));

                //spriteRenderer.enabled = true;
                wait = Time.time;
                isAttacking = true;
            }
        }

        int Signe(float f)
        {
            return f > 0 ? 1 : f == 0 ? 0 : -1;
        }

        //Syncing attacking
        public new void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //PhotonOwnerId is only writable by the owner of the Item.
            //Thats why owner is changed in pickup.

            //sync PhotonOwnerId
            if (stream.IsWriting)
            {
                stream.SendNext(isAttacking);
            }
            else
            {
                isAttacking = (bool)stream.ReceiveNext();
            }

            base.OnPhotonSerializeView(stream, info);
        }



    }
}
