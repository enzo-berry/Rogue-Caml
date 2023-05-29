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

        public GameObject lvl3manager;
        public float coolDown = 100.0f;
        private float last = 0;
        
        public GameObject[] ProjectilType;
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
                coolDown *= 1 - (GameManager.Difficulty - 50) / 100;
                
                last = Time.time;
                Patern.Add(Sulfateuse);
                Patern.Add(Follow);
                
                _pistol = PhotonNetwork.Instantiate(Pistol.name, Vector3.zero, Quaternion.identity).GetComponent<Pistol>();
                Pickup(_pistol.gameObject.GetPhotonView().ViewID);

                nbAction = Patern.Count;
            }
        }
    
        // Update is called once per frame

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

            _pistol.ProjectilePrefab = ProjectilType[0];

            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    _pistol.Attack(v);
                    v = q * v;
                    
                    //last = Time.time;
                    //int a = 0;
                    //while (Time.time - last < 0.5f) a = 345 * 34567;
                    float counter = 0;
                    float waitTime = 4f;
                    while (counter < waitTime)
                    {
                        //Increment Timer until counter >= waitTime
                        counter += Time.deltaTime;
                        //Debug.Log("We have waited for: " + counter + " seconds");
                        //Wait for a frame so that Unity doesn't freeze
                        //Check if we want to quit this function
                    }

                }
                v = p * v;
            }
        }

        void Follow()
        {
            Vector2 v = new Vector2(0, 1);
            
            Quaternion q = Quaternion.AngleAxis(45f, new Vector3(0,0,1 ));

            _pistol.ProjectilePrefab = ProjectilType[1];

            for (int i = 0; i < 8; i++)
            {
                _pistol.Attack(v);
                v = q * v;
                    
                
            }
        }

        public override void Kill()
        {
            if (IsMine)
            {
                Lvl3manager lvlman = lvl3manager.GetComponent<Lvl3manager>();
                lvlman.endgame();
                GameManager.Instance.DestroyObject(this._pistol.gameObject);
                GameManager.Instance.DestroyObject(this.gameObject);
            }
        }
    }
}

