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

    public float speed;

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
        
        if(!col.gameObject.CompareTag(tag) && !col.gameObject.CompareTag("Equiped") && PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(this.gameObject);
    }
}
}
