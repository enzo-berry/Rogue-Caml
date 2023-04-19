using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

namespace RogueCaml
{
    public abstract class Item : MonoBehaviourPunCallbacks
    {
        protected SpriteRenderer spriteRenderer;
        protected GameObject currentGameObject;
        public GameObject owner = null;
        public bool Hidden = true; //Synced

        [PunRPC]
        public abstract void Pickup(int OwnerId);

        [PunRPC]
        public abstract void Drop();

        //syncing hidden for now before attack animation.
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(Hidden);
            }
            else
            {
                Hidden = (bool)stream.ReceiveNext();
            }
        }


    }
}