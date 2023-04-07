using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace RogueCaml
{
    public class RangeWeapon : Weapon
    {
        public GameObject ProjectilePrefab;
        
        public override void Attaque(Vector2 direction)
        {
            Vector3 tmp = new Vector3(direction.x, direction.y, 0);
            GameObject b =
                PhotonNetwork.Instantiate(ProjectilePrefab.name, transform.position + tmp, Quaternion.identity);

            b.GetComponent<Projectile>().direction = direction;
        }
    }
}
