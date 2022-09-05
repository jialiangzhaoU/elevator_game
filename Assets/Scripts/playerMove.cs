using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class playerMove : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public LayerMask headCheck;
    private bool jump_good=false;
    public Collider2D coll;
    public Collider2D head;
    private bool squat;
    private float player_high;
    private float player_weigth;
    private float headCheck_y;
    private float headCheck_x;
    [HideInInspector]
    public float facedirection;
    public float horizontalmove ;
    public string lose_name;



    // Update is called once per frame

    void Start() {
        headCheck_y=head.transform.localPosition.y;
        headCheck_x = head.transform.localPosition.x;
        player_high = this.GetComponent<BoxCollider2D>().size.y;
        player_weigth = this.GetComponent<BoxCollider2D>().size.x;
        facedirection = Input.GetAxisRaw("Horizontal");
        horizontalmove = Input.GetAxis("Horizontal");
    }
    void Update()
    {
        

        Movement();
      
    }
    void Movement()
    {

        horizontalmove = Input.GetAxis("Horizontal");
        facedirection = Input.GetAxisRaw("Horizontal");

        //character move
        if (horizontalmove != 0)
        {
         
            if (squat)
            {
                rb.velocity = new Vector2(horizontalmove * speed * 0.5f, rb.velocity.y);
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

        if (coll.IsTouchingLayers(ground) || coll.IsTouchingLayers(headCheck))
        {
            jump_good = true;
        }
        else
        {
            jump_good = false;
        }

        if (head.IsTouchingLayers(headCheck) || head.IsTouchingLayers(ground))
        {
            if (coll.IsTouchingLayers(ground) || coll.IsTouchingLayers(headCheck))
            {
                print("Player dies due to being squashed");
                player_dead();
            }
            
        }
      


        if (Input.GetButtonDown("Jump") && jump_good == true)
        {

            rb.AddForce(new Vector2(0, jumpforce));
            head.transform.localPosition = new Vector2(headCheck_x, headCheck_y);
            this.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
            this.GetComponent<BoxCollider2D>().size = new Vector2(player_weigth, player_high);
            squat = false;

        }
        if (Input.GetKeyDown(KeyCode.S)) {
            if (squat)
            {
                head.transform.localPosition = new Vector2(headCheck_x, headCheck_y);
                this.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
                this.GetComponent<BoxCollider2D>().size = new Vector2(player_weigth, player_high);
                squat = false;

            }
            else {
                head.transform.localPosition = new Vector2(headCheck_x, headCheck_y-1);
                this.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -player_high / 4);
                this.GetComponent<BoxCollider2D>().size = new Vector2(player_weigth, player_high / 2);
                squat = true;

            }
        }


    }

    public void player_dead() {
        
            StartCoroutine(lose_menu());
        
        
    }

    IEnumerator lose_menu()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(lose_name);


    }


}
