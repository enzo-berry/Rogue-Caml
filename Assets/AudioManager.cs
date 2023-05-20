using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//Audio manager va gérer tous les sons dans le jeux (ambiance et sons d'interactions)

public class AudioManager : MonoBehaviour
{
    private Dictionary<string, AudioSource> _audioSources;
    public Slider slider;
    private float _volume;
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

    public void SetMusicVolume()
    {
        _volume = slider.value;
        mainMenu.volume = _volume;
        waitingLevel.volume = _volume;
    }

    public void firstPlay()
    {
        firstLevel.volume = _volume;
        firstLevel.Play();
    }
}
