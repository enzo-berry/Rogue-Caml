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
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    Sword Sword = PhotonNetwork.Instantiate("Pistol", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Sword>();
        //}

        if (GameManager.level == 1)
        {
            Pistol Pistol1 = PhotonNetwork.Instantiate("Pistol", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Pistol>();
            Pistol Pistol2 = PhotonNetwork.Instantiate("Pistol", new Vector3(10, 0, 0), Quaternion.identity).GetComponent<Pistol>();

        }

    }

    // Start is called before the first frame update

}
