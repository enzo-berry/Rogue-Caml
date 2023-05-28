using Photon.Pun;
using RogueCaml;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static bool gameisPaused = false;

    private void Start()
    {
        if (GameManager.Level == 1 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Pistol", new Vector3(-8, -4, 0), Quaternion.identity);
            PhotonNetwork.Instantiate("Pistol", new Vector3(8, 4, 0), Quaternion.identity);

            PhotonNetwork.Instantiate("Sword", new Vector3(8, -6, 0), Quaternion.identity);
            PhotonNetwork.Instantiate("Sword", new Vector3(-8, 6, 0), Quaternion.identity);
        }

        //GameObject.Find("InteractPanel").SetActive(false);
        //GameObject.Find("InfoPanel").SetActive(false);

    }

    // Start is called before the first frame update

}
