using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public AudioSource audioAttack;
    public Animator mc_animator;
    public int damage;
    public float startTime;
    public float time;
    public Collider2D punch;
    public Collider2D air;
    public float wait_time;
    private float temp_time =0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temp_time -= Time.deltaTime;
        PlayerAttackJinZhan();
    }

    void PlayerAttackJinZhan()
    {
        if (Input.GetKeyDown(KeyCode.Z) && temp_time<=0)
        {
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        mc_animator.SetBool("FistAttack", true);
        temp_time = wait_time;
        
            audioAttack.Play();

        
        yield return new WaitForSeconds(startTime);
        air.enabled = true;
        punch.enabled = true;
        
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
