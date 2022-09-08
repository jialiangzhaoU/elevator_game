using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class to_win : MonoBehaviour
{
    public string win_name;
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
        if (other.gameObject.GetComponent<attack>())
        {
           
            StartCoroutine(win_move(other)); 

           StartCoroutine(win_wait(other));

        }
    }

    IEnumerator win_wait(Collider2D other)
    {

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(win_name);


    }

    IEnumerator win_move(Collider2D other) {

        
        other.gameObject.GetComponent<playerMove>().enabled = false;
        
        other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * 5 , other.gameObject.GetComponent<Rigidbody2D>().velocity.y);
        other.gameObject.transform.GetChild(0).DetachChildren();
        yield return 1;
    }


    
}
