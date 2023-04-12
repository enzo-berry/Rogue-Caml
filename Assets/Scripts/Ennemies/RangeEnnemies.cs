using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using Unity.Mathematics;
using UnityEngine;


namespace RogueCaml
{
    public class RangeEnnemies : EnnemiesManager
    {
        private float securityRange;

        
        //pour ajouter un peu de random pour le deplacement
        
        // Start is called before the first frame update
        void Start()
        {
            setTarget();
            Weapon = PhotonNetwork.Instantiate(WeaponPrefab.name, Vector3.zero, quaternion.identity)
                .GetComponent<Weapon>();
            Weapon.Pickup(this.gameObject);
            range = Weapon.range;
        }

        // Update is called once per frame
    }
}