using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RogueCaml;
using UnityEngine;

public class DoorWall : Mechanic
{
    [SerializeField] private Door[] _doors;


    public override void Activate(int v)
    {
        foreach (Door d in _doors)
        {
            d.Activate(v);
        }
    }
    
    
}
