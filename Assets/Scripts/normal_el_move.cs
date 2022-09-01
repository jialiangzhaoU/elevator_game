using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normal_el_move : MonoBehaviour
{
    public float top;
    public float bottom;
    public float speed;
    private bool go_up;


    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
     
        if (transform.position.y <= top && go_up == true)
        {

        
            transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        }
        else
        {


            go_up = false;
        }

        if (transform.position.y >= bottom && go_up == false)
        {

            transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        }
        else
        { 
            go_up = true;
        }



    }
}
