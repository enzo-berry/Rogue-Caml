using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (PlayerManager player in FindObjectsOfType<PlayerManager>())
            {
                player.transform.position = transform.position;
            }
        }
    }
}
