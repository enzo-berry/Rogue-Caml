using Photon.Pun;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueCaml
{
    /*
     * classe de base pour les armes
     *
     * Attaque a redefinir pour chaque arme
     * 
     */
    public abstract class Weapon : Item
    {
        [SerializeField]
        public int Dammage; //editor based value
        public bool isAttacking;
        public float Range;
        
        [SerializeField]
        public float cooldown;

        public abstract void Attack(Vector2 direction);
        
        public abstract int GetDammage();

        public abstract void Stop(); //stop the attack

        public new void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //PhotonOwnerId is only writable by the owner of the Item.
            //Thats why owner is changed in pickup.

            //sync PhotonOwnerId
            if (stream.IsWriting)
            {
                stream.SendNext(isAttacking);
            }
            else
            {
                isAttacking = (bool)stream.ReceiveNext();
            }

            //Syncing characteristics
            base.OnPhotonSerializeView(stream, info);
        }
    }
}