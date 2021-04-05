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

    // tracks if crouching, used to stop weird stuff, maybe kill enemies.
    private bool crouching;

    public LayerMask enemyLayer;

    public LayerMask wallLayer;
    
    // use when deciding if enemy is in layer, layer.value is in 2 bit form, 
    // so doesn't work
    private int enemyLayerValue;
    private int wallLayerValue;
    
    // wall sliding stuff
    private bool isSliding;
    private float startVelocity;
    private Vector3 dirToWall;
    
    // Pause game on death
    public Timer timer;
    
    // Start is called before the first frame update
    void Start()
    {
        crouchOffset = new Vector3(0, 2f, 0);
        rb = GetComponent<Rigidbody>();
        
        // change enemyLayer to normal (hopefully)
        enemyLayerValue = (int) Mathf.Log(enemyLayer.value, 2);
        // change wallLayer to normal base ten math
        wallLayerValue = (int) Mathf.Log(wallLayer.value, 2);
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

        if (Input.GetButtonDown("Fire2") && isGrounded)
        {
            crouching = true;
            startCrouch();
        }

        if (Input.GetButtonUp("Fire2"))
        {
            if (crouching)
            {
                stopCrouch();
                crouching = false;
            }
            // stopCrouch();
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
        
        timer.stopPlaying();
        
        // Stops game when replaying, so no
        //Time.timeScale = 0f;
        // Debug.Log(Time.time);

        mouselook.enabled = false;
        gunscript.enabled = false;
        this.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.layer == enemyLayerValue && crouching)
        {
            collision.transform.GetComponent<EnemyNavMes>().GetHit();
        }
        
        Debug.Log("Touching Somethin");

        if (collision.transform.gameObject.layer == wallLayerValue && Input.GetButton("Jump") && !isSliding && !isGrounded)
        {
            Debug.Log("Hello There");
            startWallSlide();
        }
        
        // return;
        // If crouching and collision is enemy, kill enemy.
        // Allows sliding into enemies
    }

    private void startWallSlide()
    {
        isSliding = true;

        // no falling
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        
        rb.isKinematic = false;
        rb.velocity = controller.velocity;
        startVelocity = controller.velocity.magnitude;
        
        controller.enabled = false;
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