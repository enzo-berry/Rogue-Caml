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
        if (PhotonNetwork.IsMasterClient)
        {
            Sword Sword = PhotonNetwork.Instantiate("Sword", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Sword>();
        }
    }

    // Start is called before the first frame update

}
