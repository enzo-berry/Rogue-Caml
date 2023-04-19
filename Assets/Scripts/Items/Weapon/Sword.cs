using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;


/*ToRecode:
 * use RPC for attack rather than OnPhotonSerializeView.
 * RPC : Better for ponctual events and syncing actions: ex: sword attacking
 * OnPhotonSerializeView : Better for fast sync ex: coords/health.
 * 
 * Don't hardcode coordonates, maybe prefer using animations.
*/

namespace RogueCaml
{
    public class Sword : Weapon
    {

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            currentGameObject = this.gameObject;
            currentGameObject.tag = "sword_unequiped";
        }

        void Update()
        {
            if (owner)
                currentGameObject.transform.position = owner.transform.position;

            if (Hidden)
                spriteRenderer.enabled = false;
            else
                spriteRenderer.enabled = true;
        }


        [PunRPC]
        public override void Attack()
        {
            Debug.Log("Attack RPC called");
            Hidden = !Hidden;
            //Implement attack animation here.
        }


        [PunRPC]
        public override void Pickup(int OwnerId)
        {
            currentGameObject.tag = "sword_equiped";
            Hidden = true;
            owner = PhotonView.Find(OwnerId).gameObject;

        }


        [PunRPC]
        public override void Drop()
        {
            Hidden = false;
            currentGameObject.tag = "sword_unequiped";
            owner = null;
        }

    }
}
