using Assets.Scripts;
using Photon.Pun;
using RogueCaml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Computer : ObjectCharacteristics
{
    AudioSource audioData;
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        panel.SetActive(false);
    }

    //////////////////////////To migrate to player input
    //void Update()
    //{
    //    if (isInRange)
    //    {
    //        if (Input.GetKeyDown(GameManager.key_controls["interact"]))
    //        {
    //            if (panel.activeInHierarchy == false)
    //            {
    //                audioData.Play();
    //                panel.SetActive(true); //modifier ici pour adapter l'affichage en fonction du niveau
    //            }
    //            else
    //            {
    //                panel.SetActive(false); //modifier ici pour adapter l'affichage en fonction du niveau
    //            }
                
    //        }
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("cda"))
    //    {
    //        isInRange = true;
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("cda"))
    //    {
    //        isInRange = false;
    //        if (!panel.gameObject.CompareTag("Piece"))
    //        {
    //            panel.SetActive(false);
    //        }
    //    }
    //}


    //sync characteristics


}
