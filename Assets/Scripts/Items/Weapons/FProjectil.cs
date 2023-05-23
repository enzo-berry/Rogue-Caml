using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace RogueCaml
{
    public class FProjectil : Projectil
    {
        private void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                transform.position += speed * Time.fixedDeltaTime * (Vector3)direction;
                direction = (Vector2)( Quaternion.AngleAxis(0.5f, new Vector3(0,0,1 )) * ((direction)));
            }
        }
    }
}