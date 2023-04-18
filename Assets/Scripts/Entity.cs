using Photon.Pun;

namespace RogueCaml
{
    public abstract class Entity : MonoBehaviourPunCallbacks, IPunObservable
    {
        public int Health = 5;
        public float moveSpeed = 5f;
        public float Height_mov = 0;
        public float Width_mov = 0;
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(Health);
                stream.SendNext(moveSpeed);
                stream.SendNext(Height_mov);
                stream.SendNext(Width_mov);

            }
            else
            {
                // Network player, receive data
                this.Health = (int)stream.ReceiveNext();
                this.moveSpeed = (float)stream.ReceiveNext();
                this.Height_mov = (float)stream.ReceiveNext();
                this.Width_mov = (float)stream.ReceiveNext();
            }
        }
        
        public void TakeDommage(int amount)
        {
            Health -= amount;
        }
    }
}