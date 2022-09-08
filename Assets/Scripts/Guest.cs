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
    }

    private void OnDisable()
    {
        Enemy.AlertAction -= Alert;
        AlertAction -= Alert;
    }

    private void Update()
    {
        if (IsPlayerInRange() && !alerted)
        {
            AlertAction(PlayerLocation);
        }
    }

}