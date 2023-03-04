using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace RogueCaml
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        private Animator animator;
        
        #region MonoBehavior Callbacks
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            if(!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator component", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if(!animator)
            {
                return;
            }
            
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            animator.SetFloat("speed", h * h + v * v);
            animator.SetBool("up", v > 0);
            animator.SetBool("down", v < 0);
            animator.SetBool("right", h > 0);
            animator.SetBool("left", h < 0);
        }

        

        #endregion
    }
}