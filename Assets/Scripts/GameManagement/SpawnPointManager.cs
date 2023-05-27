using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    void Start()
    {

            // Get all players
        PlayerManager[] players = FindObjectsOfType<PlayerManager>();

        foreach (PlayerManager player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
                player.gameObject.transform.position = gameObject.transform.position;
        }
    }
}
