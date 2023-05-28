using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Photon.Pun;
using RogueCaml;
using UnityEngine;

public class PylonScript : Mechanic, IPunObservable
{
    public bool IsActive = false;
    
    public Animator Animator;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (photonView.IsMine && !IsActive)
        {
            Ball b = col.gameObject.GetComponent<Ball>();
            if (b != null)
            {
                switch (b.color)
                {
                    case Ball.Color.Blue:
                        Animator.SetBool("blue", true);
                        break;
                
                    case Ball.Color.Green:
                        Animator.SetBool("green", true);
                        break;
                
                    case Ball.Color.Red:
                        Animator.SetBool("red", true);
                        break;
                }

                GameManager.Instance.DestroyObject(col.gameObject);
                Lm.UpdateMe(Id, 0);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(IsActive);
        }
        else
        {
            //Network player, receive data
            this.IsActive = (bool)stream.ReceiveNext();
        }
    }

    public override void Activate(int v)
    {
        Debug.Log($"Pylon activated");
    }
}
