using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator doorOpen;
    private bool inDoor=false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && collision.gameObject.GetComponent<playerMove>())
        {
           
            collision.gameObject.GetComponent<playerMove>().InRangeofDoor = true;
            if (collision.gameObject.GetComponent<playerMove>().InDoor)
            {
                doorOpen.SetBool("open", true);
                inDoor = true;
                StartCoroutine(keep_close());
                
            }
            

        }
        if (collision.gameObject.GetComponent<playerMove>())
        {
            if (!collision.gameObject.GetComponent<playerMove>().InDoor && inDoor)
            {
                doorOpen.SetBool("open", true);
                inDoor = false;
                StartCoroutine(keep_close());

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && collision.gameObject.GetComponent<playerMove>())
        {
            collision.gameObject.GetComponent<playerMove>().InRangeofDoor = false;
            
        }
    }
  

    IEnumerator keep_close()
    {
 

        yield return new WaitForSeconds(0.5f);
        doorOpen.SetBool("open", false);
    }
}
