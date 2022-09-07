using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normal_el_move : MonoBehaviour
{
    [Tooltip("Let line of sight be visible, for Dev Purposes")]
    public bool ToggleLineVisibility = false;
    public LayerMask WallMask;
    public float top;
    public float bottom;
    public float speed;
    [Tooltip("How long does the elevator wait when it's stopped?")]
    public float ElevatorWaitTime;
    private bool go_up;
    bool isPlayerRiding; //We need to allow for the player to control the elevators movement while riding it
    bool Waiting; //Elevator should pause when we hit a new floor so people can get on and off
    bool shouldPause;

    void Start()
    {
        shouldPause = true;
        //StartCoroutine(Movement());
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {


        if (go_up)
        {
            if (ToggleLineVisibility)
            {
                Debug.DrawRay(transform.position - transform.up * 3.3f, transform.right * 4, Color.green);
            }
                
            //Draw a different ray depending on if we're going up or down, because that matters
            if (Physics2D.Raycast(transform.position - transform.up * 3.3f, transform.right, 4, WallMask) && shouldPause) //Check if we've hit a new floor, if so, stop
            {
                //print("Hit a New Floor!");
                StartCoroutine(HitFloorPause());
                StartCoroutine(Pause());

            }
            if (transform.position.y <= top && !Waiting)
            {


                transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
            }
            else if (transform.position.y > top && !Waiting)
            {
                StartCoroutine(Pause());


                go_up = false;
            }
        }
        else
        {
            if (ToggleLineVisibility)
            {
                Debug.DrawRay(transform.position - transform.up * 2.2f, transform.right * 4, Color.green); 
            }
            //Draw a different ray depending on if we're going up or down, because that matters
            if (Physics2D.Raycast(transform.position - transform.up * 2.2f, transform.right, 4, WallMask) && shouldPause) //Check if we've hit a new floor, if so, stop
            {
                //print("Hit a New Floor!");
                StartCoroutine(HitFloorPause());
                StartCoroutine(Pause());

            }
            if (transform.position.y >= bottom && !Waiting)
            {

                transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
            }
            else if (transform.position.y < bottom && !Waiting)
            {

                StartCoroutine(Pause());
                go_up = true;
            }
        }


    }

    IEnumerator Pause()
    {
        //print("Waiting");
        Waiting = true;
        yield return new WaitForSeconds(ElevatorWaitTime);
        Waiting = false;
    }

    IEnumerator HitFloorPause() //Stop Drawing a ray for a few moments after hitting a new floor so we don't get stuck waiting on every new floor
    {
        shouldPause = false;
        yield return new WaitForSeconds(ElevatorWaitTime + 0.5f);
        shouldPause = true;
    }
}
