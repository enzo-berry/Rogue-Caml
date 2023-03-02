using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RogueCaml{
public class SwordScript : MonoBehaviour
{
    public EdgeCollider2D collider;
    private PlayerManager target;
    public SpriteRenderer render;
    public Rigidbody2D rb;
    public Transform transform;
    public int coolDown;
    public int angle;
    public int Dommage;
    // Start is called before the first frame update

    
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
        //Vector3 =
        //transform.Rotate(0,0,180);
        //transform.position = Vector3(3,3,0);
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

}
}
