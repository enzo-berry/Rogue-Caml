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
            Pistol Pistol1 = PhotonNetwork.Instantiate("Pistol", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Pistol>();
            //same for sword
            Sword sword = PhotonNetwork.Instantiate("Sword", new Vector3(5, 0, 0), Quaternion.identity).GetComponent<Sword>();
        }
        
        //GameObject.Find("InteractPanel").SetActive(false);
        //GameObject.Find("InfoPanel").SetActive(false);

    }

    // Start is called before the first frame update

}
