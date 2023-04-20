using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using Assets.Scripts;

namespace RogueCaml
{
    public abstract class Item : ObjectCharacteristics
    {

        //objects
        protected SpriteRenderer spriteRenderer; //needs to be initialized in Start() of child class
        protected GameObject currentGameObject; //needs to be initialized in Start() of child class
        public PlayerManager owner = null;

        public bool isweapon
        {
            get
            {
                return (characteristics & Characteristics.Weapon)==Characteristics.Weapon;
            }
        }

        public bool isequiped
        {
            get
            {
                return ((characteristics & Characteristics.Equiped) == Characteristics.Equiped);
            }
        }

        //syncing characteristics
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(characteristics);
            }
            else
            {
                characteristics = (Characteristics)stream.ReceiveNext();
            }
        }
    }
}