using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

using Random = UnityEngine.Random;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using UnityEngine.Serialization;

namespace RogueCaml 
{
    public class MobGenerator : Mechanic
    {
        public GameObject[] ennemyPrefab;

        //Spawner
        public int ennemies_per_round;
        //public int delta_between_rounds;
        public int nb_rounds;
        
        [SerializeField]
        private int countRound = 0;
        [SerializeField]
        private int countEnnemy = 0;

        private bool IsActive = false;

        private BoxCollider2D boxCollider;

        public GameObject[] doors;

        // Start is called before the first frame update
        void Start()
        {
            if(!PhotonNetwork.IsMasterClient) this.gameObject.SetActive(false);
            boxCollider = GetComponent<BoxCollider2D>();
        }


        // Update is called once per frame
        void Update()
        {
            
            
        }

        private void FixedUpdate()
        {
            if (IsActive)
            {
                if (countRound <= nb_rounds && countEnnemy <= 0)
                {
                    SpawnVague();
                }
                else if (countRound > nb_rounds)
                {
                    IsActive = false;
                    Lm.Update(Id, 0);
                }
            }
        }

        public void EnnemyDied()
        {
            countEnnemy--;
        }

        private void DisableDoors()
        {
            foreach (GameObject door in doors)
            {
                door.SetActive(false);
            }
        }
        
        

        private void SpawnVague()
        {
            countRound++;
            //Vector2 size = boxCollider.size;
            countEnnemy = ennemies_per_round;
            for (int j = 0; j < ennemies_per_round; j++)
            {
                //For now we spawn the mobs on the mobspawner.
                PhotonNetwork.Instantiate(this.ennemyPrefab[(int)Random.Range(0f, (float)ennemyPrefab.Length)].name, transform.position, Quaternion.identity, 0).GetComponent<EnnemiesManager>().mobGenerator = this;
            }
        }

        public override void Activate(int v)
        {
            IsActive = true;
            Debug.Log($"Pylon activated");
        }
    }
}
