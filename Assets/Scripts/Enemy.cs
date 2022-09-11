using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    //Delegates To Alert Nearby enemies when spotted
    //Broadcast the enemies position as well, if in range, then get alerted
    public LayerMask ElevatorMask;
    public static event AlertOthers AlertAction;
    bool inElevator;
    bool hitWall;
    public Bullet BulletPrefab;
    

    //If players y value is a certain distance higher or lower than our distance, then find an escalator or elevator to take
    //Enemy needs to resume patrolling when player is lost


    void Start()
    {
        
        StartCoroutine(Patrol());
    }

    private void OnEnable()
    {
        playerMove.BroadcastLocation += GetPlayerLoc;
        AlertAction += Alert;
        Guest.AlertAction += Alert;
    }

    private void OnDisable()
    {
        playerMove.BroadcastLocation -= GetPlayerLoc;
        AlertAction -= Alert;
        Guest.AlertAction -= Alert;
    }

    void Update()
    {

        ElevatorCheck();
        if (alerted)
        {
            inElevator = InElevator();
            if (WallCheck())
            {
                TurnAround();
            }
        }

        if (IsPlayerInRange() && !alerted)
        {
            AlertAction();
        }

        if (InEscalator)
        {
            RidingEscalator();
        }
        

    }

    public void GetPlayerLoc(Vector3 Loc)
    {
        PlayerLocation = Loc;
        print("Getting Player Location!");
    }


    IEnumerator Chase()
    {
        while (alerted)
        {
            yield return new WaitForSeconds(2);
            if (Vector3.Dot((PlayerLocation - transform.position).normalized, transform.right) < 0) 
            {
                TurnAround();
            }

            Shoot();
            //print("Players Location: " + PlayerLocation);
            //print("Enemies Location: " + transform.position);
            //print("Height Difference: " + (PlayerLocation.y - transform.position.y));
            if ((PlayerLocation.y - transform.position.y) < -0.5) //Player is Below Enemy
            {       
                print("Player Below Enemy");
                while ((PlayerLocation.y - transform.position.y) < -0.5) //Search for and ride elevator until on roughly same level as player
                {
                    if (!WallCheck() && FloorCheck() && !InEscalator) //Move if we DON'T hit a wall and if we DO hit a floor
                    {
                        rb.velocity = transform.right * PatrolSpeed;

                        if (WallCheck())
                        {
                            print("Hit a Wall!");
                            TurnAround();
                        }

                        if (InEscalatorArea && EscalatorDestination.y < transform.position.y) //Take the escalator if it leads DOWN
                        {
                            TakeEscalator();
                        }

                    }
                    //yield return new WaitForSeconds(0.1f);
                    while (!FloorCheck() && !ElevatorCheck()) //If we've found an elevator shaft with no elevator, wait
                    {
                        yield return new WaitForSeconds(1f);

                    }
                    rb.velocity = transform.right * PatrolSpeed; //Once we break out of the previous loop, Enter the elevator
                    if (inElevator)
                    {
                        rb.velocity = new Vector2(0, 0); //Stop in Elevator
                        
                        yield return new WaitForSeconds(1f);

                        while ((PlayerLocation.y - transform.position.y) < -0.5)
                        {
                            print("Waiting In ELevator");
                            yield return new WaitForSeconds(0.7f);
                        }
                        if (Vector3.Dot((PlayerLocation - transform.position).normalized, transform.right) < 0) //Using Gamebject.Find just cause for now, might fix later lol
                        {
                            TurnAround();
                        }
                        print("Breaking out of Elevator!");
                        rb.velocity = transform.right * PatrolSpeed; //Once we break out of the previous loop, leave the elevator
                        //yield return new WaitForSeconds(0.5f);
                    }


                    yield return new WaitForSeconds(0.1f);
                }

            }
            else if ((PlayerLocation.y - transform.position.y) > 0.5) //Player is Above Enemy
            {
                while ((PlayerLocation.y - transform.position.y) > 0.5)  //Search for and ride elevator until on roughly same level as player
                {
                    if (!WallCheck() && FloorCheck() && !InEscalator) //Move if we DON'T hit a wall and if we DO hit a floor
                    {
                        rb.velocity = transform.right * PatrolSpeed;

                        if (WallCheck())
                        {
                            TurnAround();
                        }

                        if (InEscalatorArea && EscalatorDestination.y > transform.position.y) //Take the escalator if it leads UP
                        {
                            TakeEscalator();
                        }

                    }

                    while (!FloorCheck() && !ElevatorCheck()) //If we've found an elevator shaft with no elevator, wait
                    {
                        print("Waiting for Elevator...");
                        yield return new WaitForSeconds(1f);

                    }
                    print("I'm Done Waiting!");
                    rb.velocity = transform.right * PatrolSpeed; //Once we break out of the previous loop, Enter the elevator
                    if (inElevator)
                    {
                        rb.velocity = new Vector2(0, 0); //Stop in Elevator
                        yield return new WaitForSeconds(1f);
                        while ((PlayerLocation.y - transform.position.y) > 0.3)
                        {
                            print("Waiting In Elevator.....");
                            yield return new WaitForSeconds(0.7f);
                        }
                        if (Vector3.Dot((PlayerLocation - transform.position).normalized, transform.right) < 0) //Using Gamebject.Find just cause for now, might fix later lol
                        {
                            TurnAround();
                        }
                        print("Breaking out of Elevator!");
                        rb.velocity = transform.right * PatrolSpeed; //Once we break out of the previous loop, leave the elevator
                        //yield return new WaitForSeconds(0.5f);
                    }

                    yield return new WaitForSeconds(0.1f);
                }
                print("Player Above Enemy");
            }




        }
    }

    public bool InElevator()
    {
        if (ToggleLineVisibility)
        {
            Debug.DrawRay(transform.position, -transform.up, Color.blue);
        }
        
        return (Physics2D.Raycast(transform.position, -transform.up, 0.3f, ElevatorMask));
    }

    //Should be same as floor check, but instead check super high/low for an elevator
    //Alternatively, try making an elevator shaft object
    public bool ElevatorShaftCheck() 
    {

        return false;
    }

    public bool ElevatorCheck() //Check if an elevator has arrived
    {
        if (ToggleLineVisibility)
        {
            Debug.DrawRay(transform.position - transform.up * 0.3f, transform.right * (DistanceCheck + 1), Color.blue);
        }

        return (Physics2D.Raycast(transform.position - transform.up * 0.3f, transform.right, DistanceCheck + 1, ElevatorMask));
    }

    //Draw a line forward
    public override void Alert()
    {
        if (Vector3.Dot((PlayerLocation - transform.position).normalized, transform.right) < 0) //If Enemy is facing away from Player, turn around
        {
            TurnAround();
        }
        
        
        //Shoot();
        alerted = true;
        StartCoroutine(Chase());
        //print(Loc);
    }

    public void Shoot() //Shoot at the player if spotted and in range
    {
        Instantiate(BulletPrefab, transform.position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            Escalator_area startPoint = collision.gameObject.GetComponent<Escalator_area>();
            InEscalatorArea = true;
            EscalatorDestination = collision.gameObject.GetComponent<Escalator_area>().endSpot.transform.position;

            //TakeEscalator();
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


    private bool isdead=false;
    public void dead() {

        if (!isdead) {
            isdead = true;
            Destroy(this.gameObject);
        }
        
    }

}
