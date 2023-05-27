using System.Data.Common;
using Photon.Pun;
using UnityEngine;

namespace RogueCaml
{
    public abstract class Mechanic : MonoBehaviourPunCallbacks
    {
        public LvlMechanique Lm;
        public int Id;

        public abstract void Activate(int v);
    }
}