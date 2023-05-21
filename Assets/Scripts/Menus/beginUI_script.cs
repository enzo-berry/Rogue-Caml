using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace RogueCaml
{
    public class beginUI_script : MonoBehaviour
    {
        public TMP_Text text;
        public Scrollbar scrollbar;

        private void Start()
        {
            if(!PhotonNetwork.IsMasterClient)
                this.gameObject.SetActive(false);

        }
        public void Update()
        {

        }

        public void UpdateDifficulty()
        {
            //gameManager.difficulty = (int)(slider.value * 100);
            text.text = ((int)(scrollbar.value * 100)).ToString();
        }
    }
}


