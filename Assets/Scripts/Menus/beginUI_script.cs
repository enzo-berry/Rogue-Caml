using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueCaml
{
    public class beginUI_script : MonoBehaviour
    {
        public GameManager gameManager;
        // Start is called before the first frame update
        public TMP_Text text;
        public Scrollbar scrollbar;

        private void Start()
        {
            if(!PhotonNetwork.IsMasterClient)
                this.gameObject.SetActive(false);
            gameManager = GameManager.Instance;
        }

        public void UpdateDifficulty()
        {
            //gameManager.difficulty = (int)(slider.value * 100);
            text.text = ((int)(scrollbar.value * 100)).ToString();
        }
    }
}


