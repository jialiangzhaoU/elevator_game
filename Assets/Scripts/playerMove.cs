using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour


{

    public Animator mc_animator;

    public Rigidbody2D rb;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    private bool jump_good=false;
    public Collider2D coll;
    private bool squat;
    private float player_high;
    private float player_weigth;


    // Update is called once per frame

    void Start() {
        player_high = this.GetComponent<BoxCollider2D>().size.y;
        player_weigth = this.GetComponent<BoxCollider2D>().size.x;
    }
    void Update()
    {
        mc_animator.SetFloat("Horizontal",Input.GetAxis("Horizontal"));

        Movement();
      
    }
    void Movement()
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");

        //character move
        if (horizontalmove != 0)
        {
            if (squat)
            {
                rb.velocity = new Vector2(horizontalmove * speed*0.5f, rb.velocity.y);
            }
            else {
                rb.velocity = new Vector2(horizontalmove * speed, rb.velocity.y);
            }
            
           
        }

        //character turn around
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
           
        }

        if (coll.IsTouchingLayers(ground))
        {
            jump_good = true;
        }
        else
        {
            jump_good = false;
        }


        if (Input.GetButtonDown("Jump") && jump_good == true)
        {

            rb.AddForce(new Vector2(0, jumpforce));
            this.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
            this.GetComponent<BoxCollider2D>().size = new Vector2(player_weigth, player_high);
            squat = false;

        }
        if (Input.GetKeyDown(KeyCode.S)) {
            if (squat)
            {
                this.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
                this.GetComponent<BoxCollider2D>().size = new Vector2(player_weigth, player_high);
                squat = false;

            }
            else {
                this.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -player_high / 4);
                this.GetComponent<BoxCollider2D>().size = new Vector2(player_weigth, player_high / 2);
                squat = true;

            }
        }


    }
 


}
