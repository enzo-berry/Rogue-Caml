using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using Assets.Scripts;

namespace RogueCaml
{
    public abstract class Item : ObjectCharacteristics, IPunObservable
    {

        //objects
        protected SpriteRenderer spriteRenderer; //needs to be initialized in Start() of child class

        public int PhotonOwnerId; //synced
        public PlayerManager owner //depends on PhotonOwnerId
        {
            get
            {
                if (PhotonOwnerId == 0)
                {
                    return null;
                }
                else
                {
                    PhotonView photonView = PhotonNetwork.GetPhotonView(PhotonOwnerId);
                    return photonView.GetComponent<PlayerManager>();
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //PhotonOwnerId is only writable by the owner of the Item.
            //Thats why owner is changed in pickup.

            //sync PhotonOwnerId
            if (stream.IsWriting)
            {
                stream.SendNext(PhotonOwnerId);
            }
            else
            {
                PhotonOwnerId = (int)stream.ReceiveNext();
            }

            //Syncing characteristics
            SyncCharacteristics(stream, info);

        }

    }
}