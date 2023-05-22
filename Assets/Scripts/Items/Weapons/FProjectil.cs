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
        private void FixUpdate()
        {
            if (photonView.IsMine)
            {
                transform.position += speed * Time.fixedDeltaTime * (Vector3)direction;
                Debug.Log(Vector3.up);
                direction = (Vector2)( Quaternion.Euler(0, 50, 0) * ((direction)));
            }
        }
    }
}