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
        
        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<PlayerManager>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //check if ismine
            if (!photonView.IsMine)
            {
                return;
            }
            

            if (!animator)
            {
                return;
            }

            if (LevelManager.gameisPaused)
            {
                animator.SetBool("down", false);
                animator.SetBool("up", false);
                animator.SetBool("left", false);
                animator.SetBool("right", false);
                animator.SetFloat("speed", 0);
                return;
            }

            if (!player.IsAlive)
            {
                animator.SetBool("down", false);
                animator.SetBool("up", false);
                animator.SetBool("left", false);
                animator.SetBool("right", false);
                animator.SetFloat("speed", 0);
                animator.SetBool("isDead", true);
                return;
            }
            else {
                animator.SetBool("isDead", false);
            }

            //player.movement is synced
            float horizontal = player.movement.x;
            float vertical = player.movement.y;

            animator.SetFloat("speed", horizontal * horizontal + vertical * vertical);

            if (horizontal > 0)
            {
                animator.SetBool("down", false);
                animator.SetBool("up", false);
                animator.SetBool("left", false);
                animator.SetBool("right", true);
            }
            if (horizontal < 0)
            {
                animator.SetBool("down", false);
                animator.SetBool("up", false);
                animator.SetBool("left", true);
                animator.SetBool("right", false);
            }
            if (vertical < 0)
            {
                animator.SetBool("down", true);
                animator.SetBool("up", false);
                animator.SetBool("left", false);
                animator.SetBool("right", false);

            }
            if (vertical > 0)
            {
                animator.SetBool("down", false);
                animator.SetBool("up", true);
                animator.SetBool("left", false);
                animator.SetBool("right", false);
            }
        }

    }
}