using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
    public float MoveSpeed;
    public Vector3 PlayerLocation;
    public bool alerted = false;

    public AudioSource AlertSound;
    //Delegates To Alert Nearby enemies when spotted
    //Broadcast the enemies position as well, if in range, then get alerted



    
    void Start()
    {
        AlertAction += Alert;
    }

   
    void Update()
    {
        if (IsPlayerInRange() && !alerted)
        {
            AlertAction(PlayerLocation);
            //Alert();
        }
        if (WallCheck())
        {

        }


    }

    public bool IsPlayerInRange()
    {
        //RaycastHit2D Hit = Physics2D.Raycast(transform.position, transform.forward * LineOfSightDistance);
        if (ToggleLineVisibility)
        {
            Debug.DrawRay(transform.position, transform.right * LineOfSightDistance, Color.red);
        }
        
        return (Physics2D.Raycast(transform.position, transform.right * LineOfSightDistance, 20, PlayerMask));
    }

    IEnumerator Patrol()
    {
        while (!alerted)
        {
            yield return new WaitForSeconds(2);

        }
    }

    IEnumerator Chase() 
    { 
        while (alerted)
        {
            yield return new WaitForSeconds(2);
        }
    }

    public bool WallCheck() //Check if we're in front of a wall. If true, stop walking
    {

        return(Physics2D.Raycast(transform.position, transform.right * LineOfSightDistance, 20, WallMask));
    }

    public void Alert(Vector3 Loc)
    {
        alerted = true;
        AlertSound.Play();
        print(Loc);
    }

    public void Shoot() //Shoot at the player if spotted and in range
    {

    }

    public void TurnAround() //If you hit a wall, turn around
    {

    }
}
