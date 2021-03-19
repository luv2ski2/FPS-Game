using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newPlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    // movement stuff
    public float speed = 12;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    // movement / jumpy stuff
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // jumpy stuff
    Vector3 velocity;
    private bool isGrounded;

    public GameObject deadScreen;

    public MouseLook mouselook;
    public GunScript gunscript;

    // crouch stuff
    public Transform camera;
    public Vector3 crouchOffset;
    public Rigidbody rb;
    public float crouchForce = 5f;

    private bool crouching;
    
    // Start is called before the first frame update
    void Start()
    {
        crouchOffset = new Vector3(0, 2f, 0);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            // crouching = true;
            startCrouch();
        }

        if (Input.GetButtonUp("Fire2"))
        {
            // crouching = false;
            stopCrouch();
        }
        
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void GetHit()
    {
        Debug.Log("I've been deded");
        
        deadScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mouselook.enabled = false;
        gunscript.enabled = false;
        this.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        return;
        // If crouching and collision is enemy, kill enemy.
        // Allows sliding into enemies
    }

    private void startCrouch()
    {
        Vector3 currentPos = camera.position;
        currentPos -= crouchOffset;
        camera.position = currentPos;
        rb.isKinematic = false;
        rb.velocity = controller.velocity;
        rb.AddForce(rb.velocity.normalized * crouchForce, ForceMode.Acceleration);
        controller.enabled = false;
    }

    private void stopCrouch()
    {
        camera.position += crouchOffset;
        rb.isKinematic = true;
        controller.enabled = true;
    }
    
}