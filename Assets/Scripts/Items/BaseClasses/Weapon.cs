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
        [SerializeField]
        public int dammage; //editor based value

        public abstract void Attack(Vector2 direction);
    }
}