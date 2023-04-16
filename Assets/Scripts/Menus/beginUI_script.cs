using System;
using System.Collections;
using System.Collections.Generic;
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
        public Slider slider;

        private void UpdateDifficulty()
        {
            //gameManager.difficulty = (int)(slider.value * 100);
            text.text = ((int)(slider.value * 100)).ToString();
        }
    }
}


