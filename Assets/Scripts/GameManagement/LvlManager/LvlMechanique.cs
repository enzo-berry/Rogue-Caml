using Photon.Pun;
using System;
using UnityEngine;

namespace RogueCaml
{
    public abstract class LvlMechanique : MonoBehaviourPunCallbacks
    {
        private int cooldownChecking = 0;
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

        public void ReviveAllPlayers()
        {
            //send RPC to all players
            NetworkManager.Instance.photonView.RPC("Revive", RpcTarget.All);
        }

        private void FixedUpdate()
        {
            if (cooldownChecking%25==0 && PhotonNetwork.IsMasterClient)
            {
                PlayerManager[] players = FindObjectsOfType<PlayerManager>();
                players = Array.FindAll(players, p => p.IsAlive);

                if (players.Length == 0)
                {
                    //means all players died
                    //send lost RPC
                    NetworkManager.Instance.photonView.RPC("LostGame", RpcTarget.All);
                }
            }
            cooldownChecking++;
        }

    }
}