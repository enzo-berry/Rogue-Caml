using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RogueCaml{
public class SwordScript : MonoBehaviour
{
    public EdgeCollider2D collider;
    public GameObject render;
    public int coolDown;
    public int angle;
    public int Dommage;
    // Start is called before the first frame update
    void Start()
    {
        //collider.SetActive(false);
        render.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Attaque()
    {

    }

}
}
