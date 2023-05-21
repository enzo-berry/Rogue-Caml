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
        private Animator _animator;

        void Start()
        {
            _animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            currentGameObject = this.gameObject;
            characteristics = Characteristics.Weapon;
        }

        void Update()
        {
            //swords follows owner
            if (owner != null)
            {
                transform.position = owner.transform.position;
            }

            if (isequiped)
                ; //spriteRenderer.enabled = false;
            else
                spriteRenderer.enabled = true;
        }

        public override void Attack(Vector2 direction)
        {
            Debug.Log("Sword attacked !");
            //annimation
            _animator.SetTrigger("Is attacking");
        }

    }
}
