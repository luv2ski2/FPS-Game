using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Transform target;

    public bool firedByPlayer = false;

    public Transform targetPlayer;

    public Vector3 targetDir;

    public float force = 1000f;

    public GameObject shooter;
    
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

        // also not used ????
        if (targetDir != null)
        {
            // GetComponent<Rigidbody>().AddForce(targetDir * Time.deltaTime * force);
            
            // Debug.Log("Hello There!");
        }
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.collider.gameObject == shooter || (hit.collider.gameObject.layer == 9 && !firedByPlayer))
        {
            return;
        }
        
        if (hit.collider.GetComponent<EnemyNavMes>() != null)
        {
            hit.collider.GetComponent<EnemyNavMes>().GetHit();
        }

        if (targetPlayer != null && hit.collider.transform == targetPlayer)
        {
            // Use newPlayerMovement
            hit.collider.GetComponent<newPlayerMovement>().GetHit();
        }
        
        // Debug.Log(hit.collider.name + "minecraft!!!!");
        Destroy(gameObject, 0.5f);
        
    }
}
