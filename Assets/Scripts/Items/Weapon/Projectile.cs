using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RogueCaml{
public class Projectile : MonoBehaviour
{
    public Vector2 direction = new Vector2(0,0);

    public float speed;

    [Serialize] private Rigidbody2D rb;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position += speed * Time.fixedDeltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag != tag  && PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(this.gameObject);
    }
}
}
