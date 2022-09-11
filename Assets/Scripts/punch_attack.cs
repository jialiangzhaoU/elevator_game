using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punch_attack : MonoBehaviour
{

    public GameObject blood;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<repulse>())
        {
            StartCoroutine(enemyDead(other));

        }
    }

    IEnumerator enemyDead(Collider2D other) {

       other.gameObject.GetComponent<Enemy>().e_animator.SetBool("dead", true);
        yield return new WaitForSeconds(0.3f);

        
            other.gameObject.GetComponent<Enemy>().dead();
        
        
       
        Instantiate(blood,
                  other.gameObject.transform.position,
                  other.gameObject.transform.rotation);
    }



}
