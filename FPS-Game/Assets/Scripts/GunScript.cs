using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float force = 75f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

    public Transform bulletPoint;
    public GameObject bullet;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !PauseMenu.GameIsPaused)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            // Debug.Log(hit.transform.name);
        }
        
        Vector3 bulletStart = bulletPoint.position;
        Quaternion bulletRot = fpsCam.transform.rotation;
        GameObject newBullet = Instantiate(bullet, bulletStart, bulletRot);

        newBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward.normalized * 1500);

        newBullet.GetComponent<BulletScript>().firedByPlayer = true;

        // I don't think this does anything
        newBullet.GetComponent<BulletScript>().targetDir = fpsCam.transform.forward;
        newBullet.GetComponent<BulletScript>().force = force;
        
        // Destroy(newBullet, 5f);

    }
}
