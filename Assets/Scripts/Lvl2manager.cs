using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using UnityEngine;

public class Lvl2manager : LvlMechanique
{
    // Start is called before the first frame update
    public GameObject ball;

    public override void Update(int Id, int value)
    {
        if (photonView.IsMine)
        {
            switch (Id)
            {
                case 0:
                    _mechanics[1].Activate(0);
                    break;
            
                case 1:
                    PhotonNetwork.Instantiate(ball.name, transform.position, Quaternion.identity);
                    break;
            }
        }
    }
}
