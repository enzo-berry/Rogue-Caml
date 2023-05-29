using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using UnityEngine;

namespace RogueCaml
{
    public class PressurPlate : Mechanic
    {
        private bool Activable = true;

        public override void Activate(int v)
        {
            Activable = v != 0;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            
            if (PhotonNetwork.IsMasterClient)
            {
                ObjectCharacteristics objectCharacteristics = col.gameObject.GetComponent<ObjectCharacteristics>();
                if (objectCharacteristics.IsPlayer)
                {
                    Lm.UpdateMe(Id, objectCharacteristics.photonView.ViewID);
                }
            }
        }
    }
}


