using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//AudioManager for easy management of ambient audio (only) in levels and menus
//Each level has a trigger to start the appropriate music.
//The different triggers are documented below

public class AudioManager : MonoBehaviour
{
    
    private float _volume; //Private attribute for volume management
    
    //Public attributes for managing audio sources and parameter sliders.
    public Slider slider;
    public AudioSource mainMenu;
    public AudioSource waitingLevel;
    public AudioSource firstLevel;
    public AudioSource secondLevel;
    public AudioSource thirdLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void SetMusicVolume() // Public Mathode called at each update of the slider
    {
        _volume = slider.value;
        mainMenu.volume = _volume;
        waitingLevel.volume = _volume;
    }

    public void FirstPlay() // Public method called to trigger the first level
    {
        firstLevel.volume = _volume;
        firstLevel.Play();
    }

    public void SecondPlay() // Public method called to trigger the second level
    {
        throw new NotImplementedException();
    }

    public void ThirdPlay() // Public method called to trigger the third level
    {
        throw new NotImplementedException();
    }

    public void WaitingPlay() // Public method called to trigger the waiting room
    {
        throw new NotImplementedException();
    }

    public void MenuPlay() // Public method called to trigger the menu panel
    {
        throw new NotImplementedException();
    }
    
}
