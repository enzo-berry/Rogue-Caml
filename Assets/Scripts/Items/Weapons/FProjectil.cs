using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace RogueCaml
{
    
    public class FProjectil : Projectil
    {
        int Signe(float a)
        {
            return a > 0 ? 1 : a == 0 ? 0 : -1;
        }

        private int _decompte = 0;
        
        private GameObject _target;
        private void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                if (_decompte == 0)
                {
                    setTarget();
                    _decompte = 30;
                }
                var position = transform.position;
                if(_target)
                {
                    float angle = Vector3.SignedAngle(direction, _target.transform.position - position, new Vector3(0,0,1));
                    //Debug.Log(angle);
                    direction = (Vector2)(Quaternion.AngleAxis(
                        3f * Signe(angle),
                        new Vector3(0, 0, 1)) * direction);
                    transform.right = direction;
                }                
                
                position += speed * Time.fixedDeltaTime * (Vector3)direction;
                transform.position = position;
                
            }
            
            
        }

        
        //calcule la norme du vecteur
        protected double Distance(Vector2 v)
        {
            return (float)Math.Sqrt(v.x * v.x + v.y * v.y);
        }
        protected void setTarget()
        {
            //recupere les entites
            Entity[] tmp = FindObjectsOfType<Entity>();
            
            
            //recherche les ennemis de l'owner (les ennemies pour le player et inversement)
            double d = 10000000000000;
            Entity p = ParentWeapon.Owner.GetComponent<Entity>(); 
            foreach (Entity entity in tmp)
            {
                
                if (entity.IsEnemy && p.IsPlayer || entity.IsPlayer && p.IsEnemy)
                {
                    if (Distance(entity.transform.position - transform.position) < d)
                    {
                        _target = entity.gameObject;
                        d = Distance(_target.transform.position - transform.position);
                    }
                }
            }
        }
    }
}