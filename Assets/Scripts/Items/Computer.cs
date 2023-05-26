using System;
using Photon.Pun;
using RogueCaml;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Computer : ObjectCharacteristics
{
    AudioSource audioData;
    public GameObject interactPanel;
    public bool isRange { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        interactPanel.SetActive(false);
    }


}
