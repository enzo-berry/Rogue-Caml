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
        private PlayerManager player;
        
        #region MonoBehavior Callbacks
        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<PlayerManager>();
            animator = GetComponent<Animator>();
            
            if(!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator component", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (LevelManager.gameisPaused)
                return;


            if (!animator)
            {
                return;
            }


            float h = player.Height_mov;
            float v = player.Width_mov;

            animator.SetFloat("speed", h * h + v * v);

            if (h > 0)
            {
                animator.SetBool("down", false);
                animator.SetBool("up", false);
                animator.SetBool("left", false);
                animator.SetBool("right", true);
            }
            if (h < 0)
            {
                animator.SetBool("down", false);
                animator.SetBool("up", false);
                animator.SetBool("left", true);
                animator.SetBool("right", false);
            }
            if (v < 0)
            {
                animator.SetBool("down", true);
                animator.SetBool("up", false);
                animator.SetBool("left", false);
                animator.SetBool("right", false);

            }
            if (v > 0)
            {
                animator.SetBool("down", false);
                animator.SetBool("up", true);
                animator.SetBool("left", false);
                animator.SetBool("right", false);
            }
        }



        #endregion
    }
}