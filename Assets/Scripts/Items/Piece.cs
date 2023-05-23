using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueCaml
{
    public class Piece : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //To implement in the PlayerManager
            PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
            if (playerManager == null)
                return;
            else
            {
                PhotonNetwork.Destroy(gameObject);
                GameManager.Instance.NextLevel();
            }
        }

    }


}

