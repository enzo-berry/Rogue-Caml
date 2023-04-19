using Photon.Pun;
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
        public int dammage;
        public float cooldown;
        public float range;
        protected float wait;

        [PunRPC]
        public abstract void Attack();


    }
}