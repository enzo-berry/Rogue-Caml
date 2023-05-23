using System;
using System.Linq;
using System.Numerics;
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
        
        private GameObject _target;
        private void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                var position = transform.position;
                position += speed * Time.fixedDeltaTime * (Vector3)direction;
                transform.position = position;
                if(_target)
                    direction = (Vector2)( Quaternion.AngleAxis(10f * Signe(Vector3.Angle(direction, _target.transform.position - position)), new Vector3(0,0,1 )) * direction);
            }
            
            
        }

        private void Awake()
        {
            Debug.Log("zzz");
            setTarget();
        }

        protected double Distance(Vector2 v)
        {
            return (float)Math.Sqrt(v.x * v.x + v.y * v.y);
        }
        protected void setTarget()
        {
            //recupere les entites
            Entity[] tmp = FindObjectsOfType<Entity>();
            
            
            //regarde si c'est un 
            Entity[] players = tmp.Where(x => ((x.characteristics & (Characteristics.Enemy | Characteristics.Player)) 
                                               ^ ((~ParentWeapon.Owner.GetComponent<Characteristics>()) & (Characteristics.Enemy | Characteristics.Player))
                                               ) == 0).ToArray();
            Debug.Log($"{players.Length}");

            double d = 10000000000000;

            for (int i = 0; i < players.Length; i++)
            {
                if (Distance(players[i].transform.position - transform.position) < d)
                {
                    _target = players[i].gameObject;
                    d = Distance(_target.transform.position - transform.position);
                }
            }
        }
    }
}