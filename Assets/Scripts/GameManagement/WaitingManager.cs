using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RogueCaml
{
    public class WaitingManager : MonoBehaviour
    {
        public Canvas MasterChoice;
        // Start is called before the first frame update
        void Start()
        {
            MasterChoice.gameObject.SetActive(false);
            if (PhotonNetwork.IsMasterClient)
            {
                MasterChoice.gameObject.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
  

        }
    }
}
