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
        // Start is called before the first frame update
        public abstract void Pickup(PlayerManager target);
    }
}