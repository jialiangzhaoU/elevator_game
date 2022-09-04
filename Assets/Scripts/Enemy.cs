using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Delegates To Alert Nearby enemies when spotted
    //Broadcast the enemies position as well, if in range, then get alerted
    public delegate void AlertOthers(Vector3 Loc);
    public static event AlertOthers AlertAction;


    [Tooltip("So the Enemy distinguishes between player and other entities")]
    public LayerMask PlayerMask;
    [Tooltip("So the Enemy detects a wall in front of them to turn around")]
    public LayerMask WallMask;
    [Tooltip("How far does the enemy perceive players?")]
    public float LineOfSightDistance;
    [Tooltip("Let line of sight be visible, for Dev Purposes")]
    public bool ToggleLineVisibility = false;
    [Tooltip("How Close do we have to be to a wall/floor to turn around?")]
    public float DistanceCheck;
    public float PatrolSpeed;
    public float ChaseSpeed;
    public Vector3 PlayerLocation;
    public bool alerted = false;

    public AudioSource AlertSound;
    public Rigidbody2D rb;
    public Bullet BulletPrefab;

    bool Waiting;

    //Shoot
    //Wait for elevator




    void Start()
    {
        AlertAction += Alert;
        StartCoroutine(Patrol());
    }


    void Update()
    {
        if (IsPlayerInRange() && !alerted)
        {
            AlertAction(PlayerLocation);
        }



    }

    public bool IsPlayerInRange()
    {
        //RaycastHit2D Hit = Physics2D.Raycast(transform.position, transform.forward * LineOfSightDistance);
        if (ToggleLineVisibility)
        {
            Debug.DrawRay(transform.position + transform.up * 0.1f, transform.right * LineOfSightDistance, Color.red);
        }

        return (Physics2D.Raycast(transform.position, transform.right, LineOfSightDistance, PlayerMask));
    }

    IEnumerator Patrol()
    {
        while (!alerted)
        {
            if (!WallCheck() && FloorCheck()) //Move if we DON'T hit a wall and if we DO hit a floor
            {
                rb.velocity = transform.right * PatrolSpeed;
            }
            else
            {
                rb.velocity = new Vector3(0, 0, 0);
                print("Waiting");
                yield return new WaitForSeconds(2);
                TurnAround();
            }
            yield return new WaitForSeconds(0.1f);
        }




    }



    IEnumerator Chase()
    {
        while (alerted)
        {
            yield return new WaitForSeconds(2);
        }
    }


    //Draw a line forward
    public bool WallCheck() //Check if we're in front of a wall. If true, stop walking
    {
        if (ToggleLineVisibility)
        {
            Debug.DrawRay(transform.position, transform.right * DistanceCheck, Color.green);
        }

        return (Physics2D.Raycast(transform.position, transform.right, DistanceCheck, WallMask));
    }
    //Draw a line Down
    public bool FloorCheck() //Check if there's floor in front of us, i.e., if we hit a pit without an elevator to stop us
    {
        if (ToggleLineVisibility)
        {
            Debug.DrawRay(transform.position + transform.right * DistanceCheck, -transform.up * 4, Color.black);
        }

        return (Physics2D.Raycast(transform.position + transform.right * DistanceCheck, -transform.up, DistanceCheck, WallMask));
    }

    public void Alert(Vector3 Loc)
    {
        Shoot();
        alerted = true;
        AlertSound.Play();
        print(Loc);
    }

    public void Shoot() //Shoot at the player if spotted and in range
    {
        Instantiate(BulletPrefab, transform.position, transform.rotation);
    }

    public void TurnAround() //If you hit a wall or pit, turn around
    {
        //print("Turning Around");
        if (transform.right == Vector3.right) //Which way are we facing? Which way should we turn towards?
        {
            //print("Right");
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            //print("Left");
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

    }

}
