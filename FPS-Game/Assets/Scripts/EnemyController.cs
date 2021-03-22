using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// not used, use EnemyNavMes.cs instead


public class EnemyController : MonoBehaviour
{
    public Transform bulletPoint;
    public GameObject bullet;

    private float nextTimeToFire = 0f;
    
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
        
        // shooting
        if ((Vector3.Distance(transform.position, player.position) < attackRange + 3) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / 1f;
            Shoot();
        }



        // agent.SetDestination(player.position);
    }

    void Shoot()
    {
        Vector3 bulletStart = bulletPoint.position;
        Quaternion bulletRotation = transform.rotation;

        GameObject newBullet = Instantiate(bullet, bulletStart, bulletRotation);
        
        newBullet.GetComponent<Rigidbody>().AddForce(transform.transform.forward.normalized * 1000);

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
