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
        public BoxCollider2D bc;
        public GameObject ennemyPrefab;

        float timer = 0;

        //Spawner
        public int ennemies_per_round;
        public int delta_between_rounds;

        // Start is called before the first frame update
        void Start()
        {

        }


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

        private void SpawnVague()
        {
            Vector2 size = bc.size;

            for (int j = 0; j < ennemies_per_round; j++)
            {
                //For now we spawn the mobs on the mobspawner.
                PhotonNetwork.Instantiate(this.ennemyPrefab.name, transform.position, Quaternion.identity, 0);
            }

        }
        
    }
}