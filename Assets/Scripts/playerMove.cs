using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class playerMove : MonoBehaviour
{

    //public Animator mc_animator;

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
    public bool InDoor;
    public bool InRangeofDoor;
    public bool Disguised;
    public bool InElevator;
    private bool InEscalator;
    private bool InEscalatorArea;
    public Vector3 EscalatorDestination;

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
        //mc_animator.SetFloat("Horizontal",Input.GetAxis("Horizontal"));
        if (!InDoor && !InEscalator)
        {
            Movement();
        }


        if (Input.GetButtonDown("EnterDoor") && InRangeofDoor)
        {
            if (!InDoor)
            {
                StartCoroutine(EnterDoor()); 
            }
            else if (InDoor)
            {
                StartCoroutine(ExitDoor()); 
            }

        }

        if (InEscalatorArea)
        {
            if (EscalatorDestination.y - transform.position.y > 0) //If the endpoint is Higher, We want to hit W to Ascend
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    InEscalator = true;
                    rb.bodyType = RigidbodyType2D.Static;
                }
            }
            else //Else if the endpoint is lower, we hit S to descend
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    InEscalator = true;
                    rb.bodyType = RigidbodyType2D.Static;
                }
            }
        }

        if (InEscalator)
        {
            transform.position = Vector2.MoveTowards(transform.position, EscalatorDestination, speed * Time.deltaTime);
            if (transform.position == EscalatorDestination)
            {
                InEscalator = false;
                rb.isKinematic = false;
            }
        }




    }
    void Movement()
    {

        horizontalmove = Input.GetAxis("Horizontal");
        facedirection = Input.GetAxisRaw("Horizontal");

        //character move
        if (horizontalmove != 0)
        {
            if (!InDoor)
            {
                if (squat)
                {
                    rb.velocity = new Vector2(horizontalmove * speed * 0.5f, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(horizontalmove * speed, rb.velocity.y);
                }
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
                if (!InEscalatorArea)
                {
                    head.transform.localPosition = new Vector2(headCheck_x, headCheck_y - player_high/2);
                    this.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -player_high / 4);
                    this.GetComponent<BoxCollider2D>().size = new Vector2(player_weigth, player_high / 2);
                    squat = true;
                }


            }
        }


    }

    public void TurnAround() //If you hit a wall or pit, turn around
    {

        if (Input.GetAxis("Horizontal") > 0) //Moving right
        {
            if (Vector3.Dot(transform.right, Vector3.right) < 0)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }
        else if (Input.GetAxis("Horizontal") < 0) //Moving Left
        {
            if (Vector3.Dot(transform.right, Vector3.right) > 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
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

    public void player_jump() {
        rb.AddForce(new Vector2(0, jumpforce));
    }

    IEnumerator EnterDoor()
    {
        print("Entering Door");
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(1);// wait 1 sec for animation mc go out door
        this.gameObject.layer = 11;
        this.transform.Find("jumpCheck").gameObject.layer = 11;
        this.transform.Find("headCheck").gameObject.layer = 11;
        this.GetComponent<Renderer>().enabled = false;
        InDoor = true;
    }

    IEnumerator ExitDoor()
    {
        print("Exiting Door");
        yield return new WaitForSeconds(1);// wait 1 sec for animation mc go out door
        this.gameObject.layer = 8;
        this.transform.Find("jumpCheck").gameObject.layer = 8;
        this.transform.Find("headCheck").gameObject.layer = 8;
        this.GetComponent<Renderer>().enabled = true;
        InDoor = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            Escalator_area startPoint = collision.gameObject.GetComponent<Escalator_area>();
            InEscalatorArea = true;
            EscalatorDestination = collision.gameObject.GetComponent<Escalator_area>().endSpot.transform.position;


            //print("Hit an escalator");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            InEscalatorArea = false;
        }
    }



}
