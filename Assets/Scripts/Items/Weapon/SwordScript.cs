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
    public float coolDown;
    public int angle;
    public int Dammage;

    private int sens;


    private Vector2 xaxe = new Vector2(1,0);
    private Vector2 direction = new Vector2(0f,0f);
    private Vector3 mousePosition;
    private float alpha = 0;


    public bool attacking = false;
    private float wait;

    //private PlayerManager owner
    // Start is called before the first frame update

    #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                //stream.SendNext(attacking);
            }
            else
            {
                // Network player, receive data
                //this.attacking = (bool)stream.ReceiveNext();
            }
        }

    #endregion
    
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
            /*if(!attacking)
            {   Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 tmp = target.rb.position;

                Vector2 v = mp - tmp;

                alpha = (float)(v.x==0? (float)(90 * Signe(v.y)) : Math.Atan((float)(v.y/v.x)) + (Signe(v.x)==-1?Math.PI:0));
                direction = new Vector2((float)(Math.Cos(alpha)), (float)Math.Sin(alpha));
            
                this.transform.position = target.rb.position + direction;

                alpha *= (float)(180f/Math.PI);
                alpha -= 45f;

                transform.eulerAngles = new Vector3(0f,0f, alpha);
            }*/
        }
        if(target.photonView.IsMine && Input.GetMouseButtonDown(0))
        {
            Attaque();
        }
    }

    void FixedUpdate()
    {
        if(attacking)
        {
            if(Time.time - wait < coolDown)
            {
                alpha = (float)((alpha + sens * (angle * Time.fixedDeltaTime / coolDown)));
                transform.eulerAngles = new Vector3(0f,0f, alpha - 45f);

                alpha *= (float)(Math.PI/180f);

                direction = new Vector2((float)(Math.Cos(alpha)), (float)Math.Sin(alpha));
                this.transform.position = target.rb.position + direction;

                alpha *= (float)(180f/Math.PI);
                /*
                direction = new Vector2((float)(Math.Cos(alpha)), (float)Math.Sin(alpha));

                this.transform.position = target.rb.position + direction;

                alpha *= (float)(180f/Math.PI);
                //alpha -= 45f;

                transform.eulerAngles = new Vector3(0f,0f, alpha - 45f);*/
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                attacking = false;
            }
        }
    }

    public void Attaque()
    {
        if(!attacking)
        {
            Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            sens = mp.x>0?-1:1;

            Vector2 tmp = target.rb.position;
            Vector2 v = mp - tmp;
            alpha = (float)(v.x==0? (float)(90 * Signe(v.y)) : Math.Atan((float)(v.y/v.x)) + (Signe(v.x)==-1?Math.PI:0));
            alpha = (float)(alpha + (-sens * Math.PI / 2));

            direction = new Vector2((float)(Math.Cos(alpha)), (float)Math.Sin(alpha));

            this.transform.position = target.rb.position + direction;
            alpha *= (float)(180f/Math.PI);

            transform.eulerAngles = new Vector3(0f,0f, alpha - 45f);

            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            wait = Time.time;
            attacking = true;
            
        }
    }

    void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    

    public void Pickup(PlayerManager _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
            // Cache references for efficiency
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
