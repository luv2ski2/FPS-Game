using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMovement : MonoBehaviour
{

    public Rigidbody rb;

    public float acceleration;
    public float maxSpeed;

    public float jumpHeight = 4f;
    public int ground;

    private bool isGrounded;
    private bool isJumping;

    private Vector3 move;

    private float x;
    private float z;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        
        /*move = (transform.right * x + transform.forward * z) * acceleration;    */

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            // Debug.Log("JUMPING");
        }
        else
        {
            isJumping = false;
        }

        /*move = (transform.right * x + transform.forward * z) * acceleration;*/
        // Debug.Log(move);
        
        // rb.AddForce(move * Time.deltaTime * acceleration, ForceMode.VelocityChange);
        // rb.AddForce(new Vector3(5, 0, -5));
    }

    private void FixedUpdate()
    {
        move = (transform.right * x + transform.forward * z) * acceleration;
        
        // float mag = (move * Time.deltaTime).magnitude;

        float mag = move.magnitude;
        
        // Debug.Log(mag + " gaming - " + Time.deltaTime + " ___ " + mag * Time.deltaTime);
        
        // Debug.Log(rb.velocity.magnitude);

        // Debug.Log(isJumping);
        
        /*jump();

        isJumping = false;*/
        
        // Debug.Log(mag);
        
        /*if (mag > maxSpeed)
        {
            move = (move.normalized * maxSpeed) / Time.deltaTime;

            // move = move.normalized * maxSpeed * Time.deltaTime;
        }*/
        
        // Debug.Log(move.magnitude);

        move = move * Time.deltaTime;
        
        // Debug.Log(isGrounded);

        if (x == 0 && z == 0 && isGrounded)
        {
            rb.velocity = Vector3.zero;
        }

        if (isJumping)
        {
            jump();
        }
        

        rb.AddForce(move);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        // Debug.Log("hiiii " + rb.velocity.magnitude);
        // Debug.Log(rb.velocity + "BVbcv");
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        // might work. not sure
        Debug.Log("hello there");
        Debug.Log(collision.transform.gameObject.layer);
        Debug.Log(ground);
        if (collision.transform.gameObject.layer == ground)
        {
            Debug.Log("minecraft");
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.gameObject.layer == ground)
        {
            isGrounded = false;
        }
    }

    private void jump()
    {
        //Debug.Log("Jumping");

        move += (Vector3.up * jumpHeight);
    }
}
