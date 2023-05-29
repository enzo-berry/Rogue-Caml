using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using UnityEngine;

public class Lvl2manager : LvlMechanique
{
    // Start is called before the first frame update
    public GameObject ball;

    public override void UpdateMe(int Id, int value)
    {
        Debug.Log($"Activate by id:{Id}");
        if (photonView.IsMine)
        {
            switch (Id)
            {
                case 0:
                    _mechanics[2].Activate(0);
                    break;
            
                case 1:
                    PhotonNetwork.Instantiate(ball.name, transform.position, Quaternion.identity);
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
}
