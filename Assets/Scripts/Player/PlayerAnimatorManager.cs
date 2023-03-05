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
            
            //Set the initial position of the Player at startup
            animator.SetBool("up", false);
            animator.SetBool("down", true);
            animator.SetBool("right", false);
            animator.SetBool("left", false);
            if(!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator component", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if (!animator)
            {
                return;
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (v > 0)
            {
                animator.SetBool("up", true);
                animator.SetBool("down", false);
                animator.SetBool("right", false);
                animator.SetBool("left", false);
            }
            else if (v < 0)
            {
                animator.SetBool("up", false);
                animator.SetBool("down", true);
                animator.SetBool("right", false);
                animator.SetBool("left", false);
            }
            else if (h > 0)
            {
                animator.SetBool("up", false);
                animator.SetBool("down", false);
                animator.SetBool("right", true);
                animator.SetBool("left", false);
            }
            else if (h < 0)
            {
                animator.SetBool("up", false);
                animator.SetBool("down", false);
                animator.SetBool("right", false);
                animator.SetBool("left", true);
            }
            
            animator.SetFloat("speed", h * h + v * v);
        }



        #endregion
    }
}