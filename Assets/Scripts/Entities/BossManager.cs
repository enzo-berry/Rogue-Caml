using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueCaml
{
    public class BossManager : Entity
    {
        // Start is called before the first frame update

        public float coolDown = 5.0f;
        private float last = 0;

        public delegate void Action();

        private List<Action> Patern = new List<Action>();
        
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

