using Photon.Pun;
using UnityEngine;

namespace RogueCaml
{
    /*
     * classe de base pour les armes
     *
     * Attaque a redefinir pour chaque arme
     * 
     */
    
    
    public class Weapon : Item, IPunObservable
    {
        public PlayerManager Owner; //proprietaire de l'epee
        public bool attacking = false; //defini si l'epee est en train d'attaquer
        public SpriteRenderer spriteRenderer; //
        public float coolDown; //temps entre 2 attaques
        

        public virtual void Attaque(Vector2 direction)
        {
            throw new System.NotImplementedException();
        }
        
        public override void Pickup(PlayerManager target)
        {
            throw new System.NotImplementedException();
        }

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(attacking);
            }
            else
            {
                // Network player, receive data
                this.attacking = (bool)stream.ReceiveNext();
            }
        }

        #endregion
    }
}