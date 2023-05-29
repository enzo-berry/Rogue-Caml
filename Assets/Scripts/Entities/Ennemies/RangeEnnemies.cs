using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


namespace RogueCaml
{
    public class RangeEnnemies : EnnemiesManager
    {
        void Start()
        {
            
            if(photonView.IsMine)
            {
                setTarget();
                weaponPhotonId = PhotonNetwork.Instantiate(WeaponPrefab.name, Vector3.zero, quaternion.identity)
                    .GetPhotonView().ViewID;

                weapon.cooldown *= (1 + (100 - GameManager.Difficulty) / 100);
                Pickup(weaponPhotonId);
                range = weapon.Range;
            }
        }

        private void OnDestroy()
        {
            PhotonNetwork.Destroy(weapon.gameObject);
        }

        //    // Update is called once per frame
    }
}