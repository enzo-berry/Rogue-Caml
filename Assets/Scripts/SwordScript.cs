using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Photon.Pun;
using Photon.Realtime;




namespace RogueCaml{
public class SwordScript : Item
{
    //public EdgeCollider2D collider;
    public PlayerManager target;
    public SpriteRenderer render;
    //public Transform transform;
    public int coolDown;
    public int angle;
    public int Dommage;


    private Vector2 xaxe = new Vector2(1,0);
    private Vector3 mousePosition;
    private float alpha = 0;

    //private PlayerManager owner
    // Start is called before the first frame update

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
    
    void Start()
    {
        

        //collider.SetActive(false);
        //render.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 tmp = target.rb.position;

            Vector2 v = mp - tmp;

            alpha = (float)(v.x==0? (float)(90 * Signe(v.y)) : Math.Atan((float)(v.y/v.x)) + (Signe(v.x)==-1?Math.PI:0));
            Vector2 direction = new Vector2((float)(Math.Cos(alpha)), (float)Math.Sin(alpha));
            
            this.transform.position = target.rb.position + direction;

            alpha *= (float)(180f/Math.PI);
            alpha -= 45f;

            transform.eulerAngles = new Vector3(0f,0f,alpha);
        }
        
        //Vector3 =
        //transform.Rotate(0,0,180);
        
    }

    void FixedUpdate()
    {
        
    }

    void Attaque()
    {
        
    }

    public void Pickup(PlayerManager _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
            // Cache references for efficiency
        target = _target;


        this.gameObject.tag = "Weapon";
    }

    public void Drop()
    {
        target = null;
        this.gameObject.tag = "ItemW";
    }

    int Signe(float f) 
    {
        return f>0? 1 : f==0? 0 : -1;
    }

}
}
