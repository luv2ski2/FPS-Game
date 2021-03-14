using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Transform target;

    public Vector3 targetDir;

    public float force = 1000f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // not used?????
        if (target != null)
        {
            Vector3 directionVector = (target.position - transform.position).normalized;
            
            GetComponent<Rigidbody>().AddForce(directionVector * Time.deltaTime * force);
        }

        if (targetDir != null)
        {
            // GetComponent<Rigidbody>().AddForce(targetDir * Time.deltaTime * force);
            
            // Debug.Log("Hello There!");
        }
    }

    private void OnCollisionEnter(Collision hit)
    {
        Debug.Log(hit.collider.name + "minecraft!!!!");
        Destroy(gameObject, 0.5f);
    }
}
