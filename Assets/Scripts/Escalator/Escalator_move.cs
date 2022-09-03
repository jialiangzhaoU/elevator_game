using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escalator_move : MonoBehaviour
{
    public GameObject top;
    public GameObject bottm;
    public GameObject player;
    public float speed;
 
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
        if (bottm.GetComponent<Escalator_area>().isMove==true) {
           
            player.GetComponent<playerMove>().enabled = false;
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            player.transform.position = Vector2.MoveTowards(player.transform.position, top.transform.position, speed * Time.deltaTime);
        }
        if (player.transform.position == top.transform.position)
        {
            player.GetComponent<playerMove>().enabled = true;
            player.GetComponent<Rigidbody2D>().isKinematic = false;
            bottm.GetComponent<Escalator_area>().isMove = false;
        }

        if (top.GetComponent<Escalator_area>().isMove == true)
        {
           
            player.GetComponent<playerMove>().enabled = false;
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            player.transform.position = Vector2.MoveTowards(player.transform.position, bottm.transform.position, speed * Time.deltaTime);
           
        }
        if (player.transform.position == bottm.transform.position)
        {
            player.GetComponent<playerMove>().enabled = true;
            player.GetComponent<Rigidbody2D>().isKinematic = false;
            top.GetComponent<Escalator_area>().isMove = false;
        }

    }

}
