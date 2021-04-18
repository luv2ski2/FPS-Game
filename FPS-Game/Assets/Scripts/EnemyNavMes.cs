using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMes : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rb;

    public Animator animator;

    // Enemy's target
    public Transform player;
    
    public float range = 25f;
    public float force = 20f;

    public Transform gunPoint;
    public GameObject bullet;
    
    private float nextTimeToFire = 0f;

    private bool seenPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.stoppingDistance = range;
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!seenPlayer) 
            seenPlayer = inLineOfSight(player);

        if (seenPlayer)
        {
            if (agent.enabled)
            {
                agent.SetDestination(player.position);
                FaceTarget();
            
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
        
            if ((Vector3.Distance(transform.position, player.position) < range + 0.5f) && Time.time >= nextTimeToFire && agent.enabled)
            {
                nextTimeToFire = Time.time + 1f / 1f;
                Shoot();
            }
        }
        /*if (agent.enabled)
        {
            agent.SetDestination(player.position);
            FaceTarget();
            
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        
        if ((Vector3.Distance(transform.position, player.position) < range + 0.5f) && Time.time >= nextTimeToFire && agent.enabled)
        {
            nextTimeToFire = Time.time + 1f / 1f;
            Shoot();
        }*/
    }

    private bool inLineOfSight(Transform target)
    {
        RaycastHit hit;
        Vector3 rayDirection = target.position - gunPoint.position;
        
        Debug.DrawRay(gunPoint.position, rayDirection);

        if (Physics.Raycast(gunPoint.position, rayDirection, out hit, 10000f))
        {
            // Debug.Log("HELlo there!");
            // Debug.Log(hit.transform.name);

            if (hit.transform == target)
            {
                // Debug.Log("HIT player");
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    bool InRange()
    {
        return Vector3.Distance(transform.position, player.position) <= range;
    }

    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, gunPoint.position, transform.rotation);
        
        newBullet.GetComponent<Rigidbody>().AddForce(transform.transform.forward.normalized * force);

        newBullet.GetComponent<BulletScript>().shooter = gameObject;

        newBullet.GetComponent<BulletScript>().targetPlayer = player;
    }

    public void GetHit()
    {
        agent.enabled = false;
        rb.isKinematic = false;

        animator.enabled = false;
    }
}
