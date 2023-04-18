using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueCaml
{
    /*
     * classe de base pour les armes
     *
     * Attaque a redefinir pour chaque arme
     * 
     */
    
    
    public class Weapon : Item, IPunObservable
    {
        public GameObject Owner; //proprietaire de l'epee
        public bool attacking = false; //defini si l'epee est en train d'attaquer
        public SpriteRenderer spriteRenderer; //
        public float coolDown; //temps entre 2 attaques
        public bool Hidden = false;

        public float range;
        protected float wait;

        public virtual void Attaque(Vector2 direction)
        {
            throw new System.NotImplementedException();
        }

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(attacking);
                stream.SendNext(Hidden);
            }
            else
            {
                // Network player, receive data
                this.attacking = (bool)stream.ReceiveNext();
                this.Hidden = (bool)stream.ReceiveNext();
            }
        }

        #endregion
        
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            //collider.SetActive(false);
            //render.SetActive(false);
        }
        public override void Pickup(GameObject Player)
        {
            if (Player == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            Hidden = true;
            Owner = Player;
            GameObject o = this.gameObject;
            o.tag = "awe";
            DontDestroyOnLoad(o);
        }
        
        public override void Drop()
        {
            transform.position = Owner.transform.position;
            Owner = null;
            // Cache references for efficiency
            spriteRenderer.enabled = true;
            GameObject o = this.gameObject;
            o.tag = "awn";
            SceneManager.MoveGameObjectToScene(o, SceneManager.GetActiveScene());
        }
    }
    
    
}