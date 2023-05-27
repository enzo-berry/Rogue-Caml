using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    private BoxCollider2D bc;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Get all players
            PlayerManager[] players = FindObjectsOfType<PlayerManager>();

            foreach (PlayerManager player in players)
            {
                // Teleport the player to the random position
                player.gameObject.transform.position = gameObject.transform.position;
            }

        }
    }
}
