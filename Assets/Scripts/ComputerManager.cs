using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComputerManager : MonoBehaviour

{
    AudioSource audioData;
    public bool isInRange;
    public KeyCode keyInteraction;
    public UnityEvent interactAction;
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(keyInteraction))
            {
                interactAction.Invoke();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ally"))
        {
            isInRange = true;
            Debug.Log("Player enter computer collider");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ally"))
        {
            isInRange = false;
            Debug.Log("Player exit computer collider");
        }
    }
}
