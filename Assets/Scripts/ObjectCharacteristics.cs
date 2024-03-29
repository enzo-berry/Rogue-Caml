﻿using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RogueCaml
{
    /*
        Here is the new ObjectCharacteristics class, it is used as a base class for every game object in level.
        The variable characteristics is a bitfield, it is used to define the characteristics of the object.
        characteristics needs to by synced using a photonview.
        Low level implementation to check a characteristic must be done in a getter.
        Example:
        

        //Don't do
            if ((characteristics & Player) == characteristics) //This checks if the object is a player.
            {
                //Do something
            }


        //Do this
            bool isPlayer
            {
                get{
                    return (characteristics & Player) == characteristics;
                }
            }

            ...

            if (isPlayer())
            {
                //Do something
            }

    */


    public abstract class ObjectCharacteristics : MonoBehaviourPunCallbacks, IPunObservable
    {
        public Characteristics characteristics = 0;

        

        [Flags]
        public enum Characteristics
        {
            Entity =          0b0000000000000001,
            Enemy =           0b0000000000000010,
            Player =          0b0000000000000100,

            Item =            0b0000000000001000,
            Equiped =         0b0000000000010000,
            Weapon =          0b0000000000100000,
            Projectil =       0b0000000001000000,
            Piece =           0b0000000010000000,
            PC =              0b0000000100000000,
            Interactable =    0b0000100000000000,
            Consumable =      0b0001000000000000,

            PlayerTeam =      0b0000001000000000,
            
            BlockProjectils = 0b0000010000000000
        }

        public bool IsConsumable
        {
            get
            {
                return (characteristics & Characteristics.Consumable) == Characteristics.Consumable;
            }
        }

        public bool IsInteractble
        {
            get
            {
                return (characteristics & Characteristics.Interactable) == Characteristics.Interactable;
            }
            set
            {
                if (value)
                {
                    characteristics |= Characteristics.Interactable;
                }
                else
                {
                    characteristics &= ~Characteristics.Interactable;
                }
            }
        }
        public bool IsPC
        {
            get
            {
                return (characteristics & Characteristics.PC) == Characteristics.PC;
            }
        }
        public bool IsEntity
        {
            get
            {
                return (characteristics & Characteristics.Entity) == Characteristics.Entity;
            }
        }

        public bool IsEnemy
        {
            get
            {
                return (characteristics & Characteristics.Enemy) == Characteristics.Enemy;
            }
            set
            {
                if (value)
                {
                    characteristics |= Characteristics.Enemy; // Set the Equiped flag
                }
                else
                {
                    characteristics &= ~Characteristics.Enemy; // Clear the Equiped flag
                }
            }
        }

        public bool IsPlayer
        {
            get
            {
                return (characteristics & Characteristics.Player) == Characteristics.Player;
            }
            set
            {
                if (value)
                {
                    characteristics |= Characteristics.Player; // Set the Equiped flag
                }
                else
                {
                    characteristics &= ~Characteristics.Player; // Clear the Equiped flag
                }
            }
        }

        public bool IsItem
        {
            get
            {
                return (characteristics & Characteristics.Item) == Characteristics.Item;
            }
        }

        public bool IsEquiped
        {
            get
            {
                return (characteristics & Characteristics.Equiped) == Characteristics.Equiped;

            }

            set
            {
                if (value)
                {
                    characteristics |= Characteristics.Equiped; // Set the Equiped flag
                }
                else
                {
                    characteristics &= ~Characteristics.Equiped; // Clear the Equiped flag
                }
            }
        }

        public bool IsWeapon
        {
            get
            {
                return (characteristics & Characteristics.Weapon) == Characteristics.Weapon;

            }
        }

        public bool IsProjectil
        {
            get
            {
                return (characteristics & Characteristics.Projectil) == Characteristics.Projectil;
            }
        }


        public bool IsOnPlayerTeam
        {
            get
            {
                return  (characteristics & Characteristics.PlayerTeam) == Characteristics.PlayerTeam;
            }
            set
            {
                if (value)
                {
                    characteristics |= Characteristics.PlayerTeam; // Set the Equiped flag
                }
                else
                {
                    characteristics &= ~Characteristics.PlayerTeam; // Clear the Equiped flag
                }
            }
        }
        
        public bool BlockProjectils
        {
            get
            {
                return (characteristics & Characteristics.BlockProjectils) == Characteristics.BlockProjectils;
            }
            set
            {
                if (value)
                {
                    characteristics |= Characteristics.BlockProjectils; // Set the BlockProjectils flag
                }
                else
                {
                    characteristics &= ~Characteristics.BlockProjectils; // Clear the BlockProjectils flag
                }
            }
        }

        public bool IsPiece
        {
            get
            {
                return (characteristics & Characteristics.Piece) == Characteristics.Piece;
            }
        }

        //On serialize view
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //We own this player: send the others our data
                stream.SendNext(characteristics);
            }
            else
            {
                //Network player, receive data
                this.characteristics = (Characteristics)stream.ReceiveNext();
            }
        }
        

    }
}
