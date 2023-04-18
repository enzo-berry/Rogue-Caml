using Photon.Pun;

namespace RogueCaml
{
    public abstract class Entity : MonoBehaviourPunCallbacks, IPunObservable
    {
        public int Health = 5;
        public float moveSpeed = 5f;
        public float h = 0;
        public float v = 0;
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(Health);
                stream.SendNext(moveSpeed);
                stream.SendNext(h);
                stream.SendNext(v);

            }
            else
            {
                // Network player, receive data
                this.Health = (int)stream.ReceiveNext();
                this.moveSpeed = (float)stream.ReceiveNext();
                this.h = (float)stream.ReceiveNext();
                this.v = (float)stream.ReceiveNext();
            }
        }
        
        public void TakeDommage(int amount)
        {
            Health -= amount;
        }
    }
}