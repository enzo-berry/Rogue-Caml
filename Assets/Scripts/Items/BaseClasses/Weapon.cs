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
        public int dammage; //needs to be initialized in Start() of child class

        public abstract void Attack(Vector2 direction);
    }
}