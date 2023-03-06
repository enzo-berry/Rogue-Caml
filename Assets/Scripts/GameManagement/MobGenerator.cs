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

namespace RogueCaml{
    public class MobGenerator : MonoBehaviour
    {
        //public float difficulty = 25f;
        public BoxCollider2D bc;
        //private bool notTriggered = true;

        public GameObject ennemyPrefab;

        float timer = 0;

        //Spawner
        public int ennemies_per_round;
        public int delta_between_rounds;

        // Update is called once per frame
        void Update()
        {
            if (timer < delta_between_rounds)
            {
                timer += Time.deltaTime;
            }
            else
            {
                SpawnVague();
                timer = 0;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    //Debug.Log("trigger enter", this);
        //    if(PhotonNetwork.IsMasterClient)
        //    {
        //        if(collision.gameObject.CompareTag("ally"))
        //        {
        //            notTriggered = false;
        //            Vector2 size = bc.size;

        //            for(int i = 0; i < 10; i++)
        //            {
        //                PhotonNetwork.Instantiate(this.ennemyPrefab.name, transform.position + new Vector3( Random.Range(-size.x/2, size.x/2 ), Random.Range(-size.y/2, size.y/2), 0), Quaternion.identity, 0);
        //            }
        //        }
        //    }
        //}

        private void SpawnVague()
        {
            Vector2 size = bc.size;

            for (int j = 0; j < ennemies_per_round; j++)
            {
                PhotonNetwork.Instantiate(this.ennemyPrefab.name, transform.position + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), 0), Quaternion.identity, 0);
            }

        }
        
    }
}