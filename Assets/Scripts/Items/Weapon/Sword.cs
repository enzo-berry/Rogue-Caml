using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;


/*ToRecode:
 * use RPC for attack rather than OnPhotonSerializeView.
 * RPC : Better for ponctual events and syncing actions: ex: sword attacking
 * OnPhotonSerializeView : Better for fast sync ex: coords/health.
 * 
 * Don't hardcode coordonates, maybe prefer using animations.
*/

namespace RogueCaml
{
    public class Sword : Weapon
    {
        //public EdgeCollider2D collider;
        
        
        //public Transform transform;
        
        public int angle;
        public int Dammage;

        private int sens;
        
        private Vector2 Direction = new Vector2(0f, 0f);
        //private Vector3 MousePosition;
        private float Alpha = 0;


        
        private float wait;

        //private PlayerManager owner
        //Start is called before the first frame update


        
        //Called when GameStarts.
        void Awake()
        {
            //I dont understand this part, why would sword actualize LocaPlayerInstance ?
                // #Important
                // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
                if (photonView.IsMine)
                {
                    PlayerManager.LocalPlayerInstance = this.gameObject;
                }

            //We dont destroy Sword object on scene changement.
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            //collider.SetActive(false);
            //render.SetActive(false);
        }

        void Update()
        {
            if (Owner == null)
            {
                /*if(!attacking)
                {   Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 tmp = target.rb.position;

                    Vector2 v = mp - tmp;

                    Alpha = (float)(v.x==0? (float)(90 * Signe(v.y)) : Math.Atan((float)(v.y/v.x)) + (Signe(v.x)==-1?Math.PI:0));
                    direction = new Vector2((float)(Math.Cos(Alpha)), (float)Math.Sin(Alpha));
            
                    this.transform.position = target.rb.position + direction;

                    Alpha *= (float)(180f/Math.PI);
                    Alpha -= 45f;

                    transform.eulerAngles = new Vector3(0f,0f, Alpha);
                }*/
            }
            //else if (Owner.photonView.IsMine && Input.GetMouseButtonDown(0))
            //{
                //Attaque(); //gerer par le player maintenant
            //}
        }

        void FixedUpdate()
        {
            if (attacking)
            {
                spriteRenderer.enabled = true;
                if (Time.time - wait < coolDown)
                {
                    Alpha = (float)((Alpha + sens * (angle * Time.fixedDeltaTime / coolDown)));
                    transform.eulerAngles = new Vector3(0f, 0f, Alpha - 45f);

                    Alpha *= (float)(Math.PI / 180f);

                    Direction = new Vector2((float)(Math.Cos(Alpha)), (float)Math.Sin(Alpha));
                    this.transform.position = Owner.GetRigidBody().position + Direction;

                    Alpha *= (float)(180f / Math.PI);
                    /*
                    direction = new Vector2((float)(Math.Cos(Alpha)), (float)Math.Sin(Alpha));

                    this.transform.position = target.rb.position + direction;

                    Alpha *= (float)(180f/Math.PI);
                    //Alpha -= 45f;

                    transform.eulerAngles = new Vector3(0f,0f, Alpha - 45f);*/
                }
                else if (Owner.photonView.IsMine)
                {
                    spriteRenderer.enabled = false;
                    attacking = false;
                }
            }
            else if (Owner != null)
            {
                spriteRenderer.enabled = false;
            }
        }

        public override void  Attaque(Vector2 direction)
        {
            if (!attacking && Owner.photonView.IsMine)
            {
                
                Vector2 mp = direction;

                sens = mp.x > 0 ? -1 : 1;

                Vector2 tmp = Owner.GetRigidBody().position;
                Vector2 v = mp - tmp;
                Alpha = (float)(v.x == 0 ? (float)(90 * Signe(v.y)) : Math.Atan((float)(v.y / v.x)) + (Signe(v.x) == -1 ? Math.PI : 0));
                Alpha = (float)(Alpha + (-sens * Math.PI / 2));

                direction = new Vector2((float)(Math.Cos(Alpha)), (float)Math.Sin(Alpha));

                this.transform.position = Owner.GetRigidBody().position + direction;
                Alpha *= (float)(180f / Math.PI);

                transform.eulerAngles = new Vector3(0f, 0f, Alpha - 45f);

                //spriteRenderer.enabled = true;
                wait = Time.time;
                attacking = true;

            }
        }

        

        public void Drop()
        {
            Owner = null;
            this.gameObject.tag = "ItemW";
        }

        int Signe(float f)
        {
            return f > 0 ? 1 : f == 0 ? 0 : -1;
        }


        //Moved SwordCollisionController functions here, will be easier since we wont use after updates.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("trigger enter", this);

            if (collision.gameObject.CompareTag("Enemy") && attacking)
            {
                Debug.Log("Sending dammage to ennemy.", this);
                collision.gameObject.SendMessage("TakeDommage", Dammage, SendMessageOptions.RequireReceiver);
            }
        }

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
