using Photon.Pun;
using UnityEngine;

namespace RogueCaml
{
    public abstract class LvlMechanique : MonoBehaviourPunCallbacks
    {
        public Mechanic[] _mechanics;
        public abstract void UpdateMe(int Id, int value);
        
        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < _mechanics.Length; i++)
                {
                    _mechanics[i].Id = i;
                    _mechanics[i].Lm = this;
                }
            }
        
        }
    }
}