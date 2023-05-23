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
        public int ParentWeaponPhotonId; //synced
        public float LifeTime = 15;
        private float _begin;
        public Weapon ParentWeapon //depends on ParentWeaponPhotonId
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
        public int dammage //depends on ParentWeapon
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
        public float speed; //not synced, only owner uses it.

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

        // Start is called before the first frame update
        void Start()
        {
            characteristics = characteristics | Characteristics.Projectil;
            transform.Rotate(direction, 0f);
            _begin = Time.time;
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

                if (oc == null) //if its a static object in scene.
                    GameManager.Instance.DestroyObject(gameObject);

                //if Collision is an Object we handle it in the Object
            }
        }

        private void Update()
        {
            if (photonView.IsMine && Time.time - _begin > LifeTime)
            {
                GameManager.Instance.DestroyObject(this);
            }
        }
    }
}
