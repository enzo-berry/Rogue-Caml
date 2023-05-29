using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace RogueCaml
{
    public class Door : Mechanic, IPunObservable
    {
        [SerializeField] private bool IsActive;

        [SerializeField] private Animator _animator;

        [SerializeField] private Collider2D _collider;

        public override void Activate(int v)
        {
            IsActive = v != 0;
            _animator.SetBool("isActive", !IsActive);
            _collider.isTrigger = !IsActive;
        }

        private void FixedUpdate()
        {
            _animator.SetBool("isActive", !IsActive);
            _collider.isTrigger = !IsActive;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //We own this player: send the others our data
                stream.SendNext(IsActive);
            }
            else
            {
                //Network player, receive data
                this.IsActive = (bool)stream.ReceiveNext();
            }
        }
    }
}


