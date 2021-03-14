using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    public Transform player;

    public float speed = 20f;
    public Rigidbody rb;
    
    public float attackRange = 5f;
    public float attackPower = 75f;

    private bool moving = false;
    
    // public NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // agent = GetComponent<NavMeshAgent>();

        // agent.stoppingDistance = attackRange;
        // agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget();
        Debug.Log(Vector3.Distance(transform.position, player.position));
        
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            moving = true;
            // rb.AddForce(transform.forward * Time.deltaTime * speed, ForceMode.VelocityChange);
        }
        else
        {
            //rb.velocity = Vector3.zero;
            
            moving = false;
        }



        // agent.SetDestination(player.position);
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            // Vector3 dir = new Vector3(transform.forward.x, 0f, transform.forward.y);
            // rb.AddForce(dir * Time.deltaTime * speed, ForceMode.VelocityChange);
            
            rb.AddForce(transform.forward * Time.deltaTime * speed, ForceMode.VelocityChange);

            // rb.velocity = (transform.forward * Time.deltaTime * speed);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        // transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
        transform.rotation = lookRotation;
    }
}
