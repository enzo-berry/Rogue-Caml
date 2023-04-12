using Photon.Pun;

namespace RogueCaml
{
    public abstract class Entity : MonoBehaviourPunCallbacks, IPunObservable
    {
        public int Health = 5;
        public float moveSpeed = 5f;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }
        
        public void TakeDommage(int amount)
        {
            Health -= amount;
        }
    }
}