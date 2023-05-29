using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace RogueCaml
{
    public class arget : Mechanic, IPunObservable
    {
        public bool Activable = true;

        public override void Activate(int v)
        {
            Activable = v != 0;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (PhotonNetwork.IsMasterClient && Activable)
            {
                ObjectCharacteristics objectCharacteristics = col.gameObject.GetComponent<ObjectCharacteristics>();
                if (objectCharacteristics.IsProjectil)
                {
                    Activable = false;
                    Lm.UpdateMe(Id, objectCharacteristics.photonView.ViewID);
                }
            }
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //We own this player: send the others our data
                stream.SendNext(Activable);
            }
            else
            {
                //Network player, receive data
                Activable = (bool)stream.ReceiveNext();
            }
        }
    }
}

