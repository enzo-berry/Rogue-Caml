using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RogueCaml 
{
    public class Projectil : ObjectCharacteristics
    {
        public Vector2 direction = new Vector2(0, 0);
        [HideInInspector] 
        public GameObject ParentWeapon; //Weapon that created this projectile
        public float speed;

        public int dammage
        {
            get
            {
                Weapon ParentWeaponScript = ParentWeapon.GetComponent<Weapon>();
                if (ParentWeaponScript != null)
                    return ParentWeaponScript.dammage;
                else
                {
                    Debug.LogError("Projectil ParentWeaponScript does not exists !");
                    return 0;
                }

            }
        }

        [Serialize] private Rigidbody2D rb;

        // Start is called before the first frame update
        void Start()
        {
            characteristics = characteristics | Characteristics.Projectil;
            transform.Rotate(direction, 0f);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (photonView.IsMine)
                transform.position += speed * Time.fixedDeltaTime * (Vector3)direction;
        }


        //detect collision with other objects
        void OnTriggerEnter2D(Collider2D other)
        {
            if (photonView.IsMine)
            {
                Debug.Log("Enter Trigger Projectil");
                //checking if it is not an object
                ObjectCharacteristics oc = other.gameObject.GetComponent<ObjectCharacteristics>();

                if (oc == null)
                    GameManager.Instance.DestroyObject(gameObject);

                //if Collision is an Object we handle it in the Object
            }
        }

    }
}
