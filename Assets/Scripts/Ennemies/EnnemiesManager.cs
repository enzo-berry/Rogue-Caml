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
    public float moveSpeed = 5f;
    public Rigidbody2D rb;


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

    void FixedUpdate()
    {
        PlayerManager[] players = FindObjectsOfType<PlayerManager>();
        Vector2 v = new Vector2(0f,0f);
        double d = 10000000000000;

        
        

        for(int i  = 0; i < players.Length; i++)
        {
            if(Distance(players[i].transform.position - transform.position) < d);
            {
                v = players[i].transform.position - transform.position;
                d = Distance(v);
            }
        }

        float alpha = alpha = (float)(v.x==0? (float)(90 * Signe(v.y)) : Math.Atan((float)(v.y/v.x)) + (Signe(v.x)==-1?Math.PI:0));
        Vector2 direction = new Vector2((float)(Math.Cos(alpha)), (float)Math.Sin(alpha));
        
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }


    public void TakeDommage(int amount)
    {
        Health -= amount;
    }

    double Distance(Vector2 v)
    {
        return (float)(Math.Sqrt(v.x * v.x + v.y * v.y));
    }

    int Signe(float f) 
    {
        return f>0? 1 : f==0? 0 : -1;
    }

}
}