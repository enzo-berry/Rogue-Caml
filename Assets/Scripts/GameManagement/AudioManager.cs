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
    
    private static float _volume; //Private attribute for volume management
    
    //Public statitc attributes for managing audio sources and parameter sliders.
    public Slider slider;
    public AudioSource mainMenu;
    public AudioSource waitingRoom;
    public AudioSource firstLevel;
    public AudioSource secondLevel;
    public AudioSource thirdLevel;

    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = (float)0.5;
        mainMenu.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume()
    {
        AudioListener.volume = slider.value;
    }

    public void MainPlay()
    {
        firstLevel.Stop();
        secondLevel.Stop();
        thirdLevel.Stop();
        mainMenu.Play();
    }

    public void WaitPlay()
    {
        mainMenu.Stop();
        waitingRoom.Play();
    }

    public void FirstPlay()
    {
        firstLevel.Play();
        waitingRoom.Stop();
    }

    public void SecondPlay()
    {
        secondLevel.Play();
        firstLevel.Stop();
    }

    public void ThirdPlay()
    {
        secondLevel.Stop();
        thirdLevel.Play();
    }

}
