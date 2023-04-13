using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RogueCaml{
public class Projectile : MonoBehaviourPunCallbacks
{
    public Vector2 direction = new Vector2(0,0);
    public GameObject Owner;
    public float speed;
    public int Dammage;

    [Serialize] private Rigidbody2D rb;
    
    
    // Start is called before the first frame update
    void Start()
    {  
        transform.Rotate(direction, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(photonView.IsMine)
            transform.position += speed * Time.fixedDeltaTime * (Vector3)direction;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"collision pro: {Owner.tag} vs {col.gameObject.tag}");
        if (col.gameObject.tag[0] != 'a') //si pas de type a (donc non transparant aux balles)
        {
            Debug.Log($"collision pro: {Owner.tag} vs {col.gameObject.tag}");
            if (col.gameObject.tag[0] == 'c' && col.gameObject.tag[2] != Owner.tag[2]) //si il sont de clan different
            {
                col.SendMessage("TakeDommage", Dammage, SendMessageOptions.RequireReceiver);
                PhotonNetwork.Destroy(this.gameObject);
            }
            else if(col.gameObject.tag[0] != 'c') PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
}
