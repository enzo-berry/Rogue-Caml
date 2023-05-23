using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace RogueCaml
{
    public class BossManager : Entity
    {
        // Start is called before the first frame update

        public float coolDown = 100.0f;
        private float last = 0;
        
        public GameObject[] BalleType;
        public GameObject Pistol;

        private Pistol _pistol;
        

        private int currendAction = 0;

        public delegate void Action();

        private List<Action> Patern = new List<Action>();
        private int nbAction = 0;
        
        void Start()
        {
            if (IsMine)
            {
                last = Time.time;
                Patern.Add(Sulfateuse);
                
                _pistol = PhotonNetwork.Instantiate(Pistol.name, Vector3.zero, Quaternion.identity).GetComponent<Pistol>();
                Pickup(_pistol.gameObject.GetPhotonView().ViewID);

                nbAction = Patern.Count;
            }
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            if (IsMine && Time.time - last > coolDown)
            {
                last = Time.time;
                Patern[currendAction]();

                currendAction = (++currendAction) % nbAction;
            }
        }

        void Sulfateuse()
        {
            Vector2 v = new Vector2(0, 1);
            
            Quaternion q = Quaternion.AngleAxis(45f, new Vector3(0,0,1 ));
            Quaternion p = Quaternion.AngleAxis(10f, new Vector3(0,0,1 ));

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    _pistol.Attack(v);
                    v = q * v;
                }
                v = p * v;
            }
        }
    }
}

