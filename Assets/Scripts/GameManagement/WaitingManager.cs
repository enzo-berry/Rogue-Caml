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
        public GameManager gameManager;
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
        public void InstantiateGame()
        {
            Debug.Log("Ca marche pas mais on test qd mm ");
            gameManager = GameManager.Instance;
        } 
    }
}
