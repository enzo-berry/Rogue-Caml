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
        protected GameObject currentGameObject; //needs to be initialized in Start() of child class

        public int PhotonOwnerId;
        public PlayerManager owner
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


        public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
        {
            SyncCharacteristics(stream, info);

            //sync PhotonOwnerId
            if(stream.IsWriting)
            {
                stream.SendNext(PhotonOwnerId);

            }
            else
            {
                PhotonOwnerId = (int)stream.ReceiveNext();
            }

        }
    }
}