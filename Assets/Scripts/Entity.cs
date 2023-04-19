using Photon.Pun;
using UnityEngine.UIElements;
using UnityEngine;

namespace RogueCaml
{
    public abstract class Entity : MonoBehaviourPunCallbacks, IPunObservable
    {
        //stats
        public int Health = 5;
        public int moveSpeed = 5;
        public int attackSpeed = 1;

        //vars
        public bool alive = true;

        //objects
        public GameObject weapon = null;
        protected Rigidbody2D rb;

        //synced
        public Vector2 movement; //A vector2 to store the movement of the player. is used in Update method.


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(Health);
                stream.SendNext(movement);
            }
            else
            {
                // Network player, receive data
                this.Health = (int)stream.ReceiveNext();
                this.movement = (Vector2)stream.ReceiveNext();
            }
        }

        public void TakeDommage(int amount)
        {
            Health -= amount;
        }

    }
}