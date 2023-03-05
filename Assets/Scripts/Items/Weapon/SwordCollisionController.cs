using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueCaml{
public class SwordCollisionController : MonoBehaviour
{
    public SwordScript me;

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision enter", this);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("collision stay", this);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("collision exit", this);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger enter", this);

        if(collision.gameObject.CompareTag("Ennemy") && me.attacking)
        {
            Debug.Log("test", this);
            collision.gameObject.SendMessage("TakeDommage", me.Dammage, SendMessageOptions.RequireReceiver);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("trigger stay", this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("trigger exit", this);
    }
}
}
