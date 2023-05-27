using Photon.Pun;
using UnityEngine;

namespace RogueCaml
{
    
    public class Ball : MonoBehaviourPunCallbacks
    {
        public enum Color
        {
            Red,
            Blue,
            Green
        }

        public Color color;
    }
}