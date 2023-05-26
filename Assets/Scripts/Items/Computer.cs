using System;
using Photon.Pun;
using RogueCaml;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Computer : ObjectCharacteristics
{
    private AudioSource audioData;
    public GameObject interactPanel;
    public GameObject infoPanel;

    private bool isInRange = false;

    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        //audioData = GetComponent<AudioSource>();
        //interactPanel.SetActive(false);
        //infoPanel.SetActive(false);
    }

    private void Update()
    {
        //if (isInRange && !isActive)
        //{
        //    interactPanel.SetActive(true);
        //}
        //else
        //{
        //    interactPanel.SetActive(false);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter the fields");
        GameObject obj = other.gameObject;
        ObjectCharacteristics x = obj. GetComponent<ObjectCharacteristics>();
        if (x.IsPlayer) 
        { 
            isInRange= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit the fields");
        GameObject obj = other.gameObject;
        ObjectCharacteristics x = obj.GetComponent<ObjectCharacteristics>();
        if (x.IsPlayer)
        {
            isInRange = false;
        }
    }


    private void ShowInfoPanel()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            infoPanel.SetActive(true);
            isActive = true;
        }
    }

    private void HideInfoPanel()
    {
        if (Input.GetKeyDown(KeyCode.E) && isActive)
        {
            infoPanel.SetActive(false);
            isActive = false;
        }
    }
    
    
}
