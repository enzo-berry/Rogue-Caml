using System;
using RogueCaml;
using UnityEditorInternal;
using UnityEngine;


public class Computer : InterractableItem
{
    private AudioSource audioData;
    public GameObject interactPanel;
    public GameObject infoPanel;

    public bool isInRange = false;
    public bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isInRange && !isActive) 
            interactPanel.SetActive(true);
        else 
            interactPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter the fields");
        GameObject obj = other.gameObject;
        ObjectCharacteristics x = obj. GetComponent<ObjectCharacteristics>();
        if (x.IsPlayer)
        { 
            isInRange= true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit the fields");
        GameObject obj = other.gameObject;
        ObjectCharacteristics x = obj.GetComponent<ObjectCharacteristics>();
        if (x.IsPlayer)
        {
            isInRange = false;
        }
    }

    public override void Interact()
    {
        if (isActive) 
        {
            HideInfoPanel();
            
        }
        else
        {
            ShowInfoPanel();
        }
    }

    private void ShowInfoPanel()
    {
        audioData.Play();
        infoPanel.SetActive(true);
        isActive = true;
    }

    private void HideInfoPanel()
    {
        infoPanel.SetActive(false);
        isActive = false;
    }
    
    
}
