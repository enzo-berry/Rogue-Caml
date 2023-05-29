using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace RogueCaml
{
    public class Lvl1Manager : LvlMechanique
    {
        // Start is called before the first frame update
        public override void UpdateMe(int Id, int value)
        {
            GameManager.Instance.NextLevel();
        }

        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _mechanics[0].Lm = this;
                _mechanics[0].Activate(0);
                ReviveAllPlayers();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

