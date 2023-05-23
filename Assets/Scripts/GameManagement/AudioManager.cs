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
    
    //Public attributes for managing audio sources and parameter sliders.
    public Slider slider;
    public static AudioSource MainMenu;
    public static AudioSource WaitingLevel;
    public static AudioSource FirstLevel;
    public static AudioSource SecondLevel;
    public static AudioSource ThirdLevel;
    
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

    
    public void SetMusicVolume() // Public Methode called at each update of the slider
    {
        AudioListener.volume = slider.value;
        MainMenu = mainMenu;
        WaitingLevel = waitingLevel;
        FirstLevel = firstLevel;
        SecondLevel = secondLevel;
        ThirdLevel = thirdLevel;
    }

    public static void FirstPlay() // Public method called to trigger the first level
    {
        WaitingLevel.Stop();
        FirstLevel.Play();
    }

    public static void SecondPlay() // Public method called to trigger the second level
    {
        FirstLevel.Stop();
        SecondLevel.Play();
    }

    public static void ThirdPlay() // Public method called to trigger the third level
    {
        SecondLevel.Stop();
        ThirdLevel.Play();
    }

    public static void WaitingPlay() // Public method called to trigger the waiting room
    {
        MainMenu.Stop();
        WaitingLevel.Play();
    }

    public static void MenuPlay() // Public method called to trigger the menu panel
    {
        FirstLevel.Stop();
        SecondLevel.Stop();
        ThirdLevel.Stop();
        MainMenu.Play();
    }
    
}
