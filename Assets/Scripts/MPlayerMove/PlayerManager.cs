using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace RogueCaml
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public float moveSpeed = 5f;
        public Rigidbody2D rb;
        Vector2 movement;

        public int Health = 3;


        #region IPunObservable implementation

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

        #endregion

        // Update is called once per frame
        void Update()
        {
            if(photonView.IsMine)
            {
                ProcessInputs();
            }
        }

        void FixedUpdate()
        {
            if(photonView.IsMine)
            {
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            }
        }

        void ProcessInputs()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            
        }
    }
}
