using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using UnityEngine;

namespace RogueCaml
{
    public class Lvl3manager : LvlMechanique
    {
        public GameObject boss;

        private int coolDown = 0;
        public override void UpdateMe(int Id, int value)
        {
            Debug.Log($"Activate by id:{Id}");
            if (PhotonNetwork.IsMasterClient)
            {
                switch (Id)
                {
                    case 0:
                        _mechanics[2].Activate(0);
                        break;

                    case 1:
                        break;
                }
            }
        }

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < _mechanics.Length; i++)
                {
                    _mechanics[i].Id = i;
                    _mechanics[i].Lm = this;
                }
                ReviveAllPlayers();
            }
        }

        private void FixedUpdate()
        {

        }

        public void endgame()
        {
            PhotonNetwork.LoadLevel("winScene");
        }
    }
}