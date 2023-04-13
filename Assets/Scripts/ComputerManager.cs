using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComputerManager : MonoBehaviour

{
    AudioSource audioData;
    public bool isInRange;
    public KeyCode keyInteraction;
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(keyInteraction))
            {
                if (panel.activeInHierarchy == false)
                {
                    audioData.Play();
                    panel.SetActive(true);
                }
                else
                {
                    panel.SetActive(false);
                }
                
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ally"))
        {
            isInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ally"))
        {
            isInRange = false;
            panel.SetActive(false);
        }
    }
}
