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
                    case 0: // doorWall0
                        break;

                    case 1: // doorWall1
                        break;
                    
                    case 2 or 3: //PP => D0
                        _mechanics[0].Activate(0);
                        break;
                    
                    case 4: // doorWall2
                        break;
                    
                    case 5: // doorWall3
                        break;
                    
                    case 6 or 7: //PP => D1
                        _mechanics[1].Activate(0);
                        break;
                    
                    case 8 or 9: //PP => D2
                        _mechanics[4].Activate(0);
                        break;
                    
                    case 10: //PP => D3
                        _mechanics[5].Activate(0);
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

        public void endgame()
        {
            PhotonNetwork.LoadLevel("winScene");
        }
    }
}