using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
                 

    public Transform target;
 
   
    
    public float smoothSpeed = 5f; 

    void Start()
    {
            target = GameObject.FindGameObjectWithTag("Player").transform;
    }

      
    void Update()
    {
        if(target == null) 
        {
             target = GameObject.FindGameObjectWithTag("Player").transform;
             
        }
    }
        
    private void FixedUpdate()
    {
        
        Vector3 startPosition = new Vector3(target.position.x, target.position.y, -1f);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, startPosition, smoothSpeed);
        transform.position = smoothPosition;

        
    
    
         
    }


   


}
