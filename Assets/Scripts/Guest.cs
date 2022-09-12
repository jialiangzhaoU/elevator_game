using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : NPC
{
    public static event AlertOthers AlertAction;
    public bool isTarget;

    void Start()
    {
        StartCoroutine(Patrol());
    }

    private void OnEnable()
    {
        Enemy.AlertAction += Alert;
        AlertAction += Alert;
        playerMove.BroadcastLocation += GetPlayerLoc;
    }

    private void OnDisable()
    {
        Enemy.AlertAction -= Alert;
        AlertAction -= Alert;
        playerMove.BroadcastLocation -= GetPlayerLoc;
    }

    private void Update()
    {
        if (IsPlayerInRange() && !alerted)
        {
             if (!GameObject.FindObjectOfType<playerMove>().Disguised) //Do NOT alert if player is disguised. Kinda lame using FindObject but whatever.
            {
                AlertAction();
            }
            
        }
    }

    public void GetPlayerLoc(Vector3 Loc)
    {
        PlayerLocation = Loc;
        print("Getting Player Location!");
    }

}
