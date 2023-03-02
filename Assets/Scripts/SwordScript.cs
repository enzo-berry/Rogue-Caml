using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;




namespace RogueCaml{
public class SwordScript : MonoBehaviourPunCallbacks
{
    public EdgeCollider2D collider;
    private PlayerManager target;
    public SpriteRenderer render;
    public Transform transform;
    public int coolDown;
    public int angle;
    public int Dommage;


    private Vector3 mousePosition;

    //private PlayerManager owner
    // Start is called before the first frame update

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
    
    void Start()
    {
        

        //collider.SetActive(false);
        //render.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }


        if(target.photonView.IsMine)
        {
            Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 tmp = this.transform.position;
            Vector2 direction = new Vector2(Signe(mp.x - tmp.x), Signe(mp.y- tmp.y));
            this.transform.position = target.rb.position + direction;
        }
        
        //Vector3 =
        //transform.Rotate(0,0,180);
        
    }
    void Attaque()
    {
        
    }

    public void SetTarget(PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = _target;
    
        }

    int Signe(float f) 
    {
        return f>0? 1 : f==0? 0 : -1;
    }

}
}
