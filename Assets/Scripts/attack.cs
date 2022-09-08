using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public Animator mc_animator;
    public int damage;
    public float startTime;
    public float time;
    public Collider2D punch;
    public Collider2D air;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAttackJinZhan();
    }

    void PlayerAttackJinZhan()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        mc_animator.SetBool("FistAttack", true);
        yield return new WaitForSeconds(startTime);
        air.enabled = true;
        punch.enabled = true;
        mc_animator.SetBool("FistAttack", true);
        StartCoroutine(disableHitBox());
    }

    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(time);
        air.enabled = false;
        punch.enabled = false;
        mc_animator.SetBool("FistAttack", false);
    }



}
