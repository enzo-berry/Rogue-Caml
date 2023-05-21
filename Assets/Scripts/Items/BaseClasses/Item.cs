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
    }
}