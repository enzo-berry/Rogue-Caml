using System;
using System.Collections;
using System.Collections.Generic;
using RogueCaml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueCaml
{
    public class HealthBar : MonoBehaviour
    {
        public PlayerManager PManager;
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private TMP_Text text;

        private void FixedUpdate()
        {
            slider.value = 100 * PManager.Health / PManager.MaxHealth;
            text.text = $"{PManager.Health}/{PManager.MaxHealth}";
        }
    }
}

