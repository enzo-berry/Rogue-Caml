using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    // Start is called before the first frame update
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
