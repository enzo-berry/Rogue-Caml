using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
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
            bool isPlayer()
            {
                return (characteristics & Player) == characteristics;
            }

            ...

            if (isPlayer())
            {
                //Do something
            }

    */


    public abstract class ObjectCharacteristics : MonoBehaviourPunCallbacks
    {
        public Characteristics characteristics = 0;

        [Flags]
        public enum Characteristics
        {
            Entity   =    0b00000001,
            Ennemy   =    0b00000010,
            Player   =    0b00000100,

            Item     =    0b00001000,
            Equiped  =    0b00010000,
            Weapon   =    0b00100000,
        }
    }
}
