using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Photon.Pun;
using Photon.Realtime;

namespace RogueCaml{
public class EnnemiesCollisionController : MonoBehaviour
{
    public EnnemiesManager me;

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision enter", this);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("collision stay", this);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("collision exit", this);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger enter", this);

        if(collision.gameObject.CompareTag("Weapon"))
        {
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("trigger stay", this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("trigger exit", this);
    }
}
}