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
        private GameManager gameManager;
        // Start is called before the first frame update
        void Start()
        {
            //on cache le boutton start pour les pas master
            MasterChoice.gameObject.SetActive(false);
            if (PhotonNetwork.IsMasterClient)
            {
                MasterChoice.gameObject.SetActive(true);
            }
        }

        // Update is called once per frame
        

        public void InstantiateGame()
        {
            gameManager = GameManager.Instance;
            gameManager.StartGame();
        } 
    }
}
