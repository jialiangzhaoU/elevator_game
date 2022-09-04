using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fall_out : MonoBehaviour
{
    public LayerMask ground;
    public LayerMask elevator;
    private Collider2D foot;
    public float time;
    private float temp_time;
    // Start is called before the first frame update
    void Start()
    {
        temp_time=time;
        foot = this.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!(foot.IsTouchingLayers(ground) || foot.IsTouchingLayers(elevator)))
        {
            temp_time -= Time.deltaTime;
        }
        else {
            temp_time = time;
        }
        if (temp_time <= 0) {
            Destroy(this.transform.parent.gameObject);
        }
        
    }

  
}
