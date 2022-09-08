using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    //Delegates To Alert Nearby enemies when spotted
    //Broadcast the enemies position as well, if in range, then get alerted
    
    public static event AlertOthers AlertAction;

    public Bullet BulletPrefab;


    //If players y value is a certain distance higher or lower than our distance, then find an escalator or elevator to take
    //


    void Start()
    {
        
        StartCoroutine(Patrol());
    }

    private void OnEnable()
    {
        AlertAction += Alert;
        Guest.AlertAction += Alert;
    }

    private void OnDisable()
    {
        AlertAction -= Alert;
        Guest.AlertAction -= Alert;
    }

    void Update()
    {
        if (IsPlayerInRange() && !alerted)
        {
            AlertAction(PlayerLocation);
        }

        if (InEscalator)
        {
            RidingEscalator();
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

    public override void Alert(Vector3 Loc)
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

}
