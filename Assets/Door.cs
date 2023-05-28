using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueCaml
{
    public class Door : Mechanic
    {
        [SerializeField]
        private bool IsActive;

        [SerializeField]
        private Animator _animator;
        
        [SerializeField]
        private Collider2D _collider;
        
        public override void Activate(int v)
        {
            IsActive = v != 0;
            _animator.SetBool("isActive", IsActive);
            _collider.isTrigger = !IsActive;
        }
    }
}


