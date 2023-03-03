using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Photon.Pun;
using Photon.Realtime;

namespace RogueCaml{
public class EnnemiesManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public int Health = 5;


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(Health);
        }
        else
        {
                // Network player, receive data
            this.Health = (int)stream.ReceiveNext();
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if(Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    public void TakeDommage(int amount)
    {
        Health -= amount;
    }
}
}