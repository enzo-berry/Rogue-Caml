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


        
        

        //private PlayerManager owner
        //Start is called before the first frame update


        
        //Called when GameStarts.
        void Awake()
        {
            
        }

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            //collider.SetActive(false);
            //render.SetActive(false);
        }

        void Update()
        {

        }

        void FixedUpdate()
        {
            if (attacking)
            {
                Hidden = false;
                if (Time.time - wait < coolDown)
                {
                    Alpha = (float)((Alpha + sens * (angle * Time.fixedDeltaTime / coolDown)));
                    transform.eulerAngles = new Vector3(0f, 0f, Alpha - 45f);

                    Alpha *= (float)(Math.PI / 180f);

                    Direction = new Vector2((float)(Math.Cos(Alpha)), (float)Math.Sin(Alpha));
                    this.transform.position = Owner.transform.position + (Vector3)Direction;

                    Alpha *= (float)(180f / Math.PI);
                }
                else if (photonView.IsMine) //Attack finished
                {
                    Hidden = false;
                    attacking = false;
                }
            }

            if (Hidden)
                spriteRenderer.enabled = false;
            else
                spriteRenderer.enabled = true;

        }

        public override void  Attaque(Vector2 direction)
        {
            int Signe(float f)
            {
                return f > 0 ? 1 : f == 0 ? 0 : -1;
            }
            
            if (!attacking && photonView.IsMine)
            {
                
                Vector2 mp = direction;

                sens = mp.x > 0 ? -1 : 1;

                
                Alpha = (float)(direction.x == 0 ? (float)(90 * Signe(direction.y)) : Math.Atan((float)(direction.y / direction.x)) + (Signe(direction.x) == -1 ? Math.PI : 0));
                Alpha = (float)(Alpha + (-sens * Math.PI / 2));

                this.transform.position = Owner.transform.position + (Vector3)direction;
                Alpha *= (float)(180f / Math.PI);

                transform.right = direction;
                transform.Rotate(new Vector3(0,0, angle/2));

                wait = Time.time;
                attacking = true;

            }
        }

        //Moved SwordCollisionController functions here, will be easier since we wont use after updates.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("trigger enter", this);

            if (collision.gameObject.CompareTag("cde") && attacking)
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
