using System;
using System.Collections;
using System.Collections.Generic;
using RogueCaml;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text AttText;
    public TMP_Text ArmorText;
    public TMP_Text ASText;
    public TMP_Text SpeedText;

    public PlayerManager p;
    
    private void FixedUpdate()
    {
        AttText.text = $"{p.BonusDammage}";
        ArmorText.text = $"{p.Armor}";
        ASText.text = $"{p.attackSpeed}";
        SpeedText.text = $"{p.moveSpeed}";
    }
}
