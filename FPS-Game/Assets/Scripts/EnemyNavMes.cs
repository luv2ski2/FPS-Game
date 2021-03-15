using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMes : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rb;

    // Enemy's target
    public Transform player;
    
    public float range = 25f;
    public float force = 20f;

    public Transform gunPoint;
    public GameObject bullet;
    
    private float nextTimeToFire = 0f;
    
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

        if (agent.enabled)
        {
            agent.SetDestination(player.position);
            FaceTarget();
        }
        
        if ((Vector3.Distance(transform.position, player.position) < range + 3) && Time.time >= nextTimeToFire && agent.enabled)
        {
            nextTimeToFire = Time.time + 1f / 1f;
            Shoot();
        }
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
    }

    public void GetHit()
    {
        agent.enabled = false;
        rb.isKinematic = false;
    }
}
