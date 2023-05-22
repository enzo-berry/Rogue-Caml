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
    public class Projectil : ObjectCharacteristics, IPunObservable
    {
        public Vector2 direction = new Vector2(0, 0);

        public int ParentWeaponPhotonId;

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

        public Weapon ParentWeapon
        {
            get
            {
                if (ParentWeaponPhotonId == 0)
                {
                    Debug.LogError("ParentWeaponPhotonId = 0 !");
                }

                PhotonView PhotonViewParentWeapon = PhotonNetwork.GetPhotonView(ParentWeaponPhotonId);
                GameObject gameObject = PhotonViewParentWeapon.gameObject;

                if (gameObject == null)
                {
                    Debug.Log("GameObject of ParentWeapon is null");
                }

                return gameObject.GetComponent<Weapon>();
            }
        }

        //On serialize view for ParentWeaponPhotonId
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(ParentWeaponPhotonId);
            }
            else
            {
                // Network player, receive data
                this.ParentWeaponPhotonId = (int)stream.ReceiveNext();
            }

            SyncCharacteristics(stream, info);

        }

        //Weapon that created this projectile
        public float speed;

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