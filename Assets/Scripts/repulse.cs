using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repulse : MonoBehaviour
{
    public bool isRepulse;
    public LayerMask window;
    // Start is called before the first frame update
    void Start()
    {
        isRepulse = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRepulse) {
            StartCoroutine(repulse_fun());
        }
      

       

    }

    IEnumerator repulse_fun()
    {
        yield return new WaitForSeconds(1);
        isRepulse=false;
    }

    void OnTriggerStay2D(Collider2D other)
    {



        if (other.gameObject.GetComponent<window>()&& isRepulse)
        {

            Destroy(other.gameObject);

        }
    }

}
