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
    public float wallSlideSpeed;
    private Vector3 slideVelocity;
    [SerializeField] private float timeJump = 0.4f;
    private float nextJump;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDist;

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

        if ((isGrounded && velocity.y < 0) /* || isSliding */ )
        {
            velocity.y = -2f;
        }
        
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if ((Input.GetButtonDown("Jump") && isGrounded) || ((nextJump > Time.time) && !isSliding))
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

        // Should add a way to check if you're on a wall // Did That
        if (!Input.GetButton("Jump") && isSliding)
            stopWallSlide();

        Collider[] hitWalls = Physics.OverlapSphere(wallCheck.position, wallCheckDist, wallLayer);
        if (isSliding && hitWalls.Length == 0) {
            stopWallSlide();
            Debug.Log("kfdjslajfklads");
        }
        
        Debug.Log(isSliding);

    }

    private void FixedUpdate()
    {
        if (isSliding)
        {
            // Fix below, make it so that the player can't go faster than wall slide speed
            
            // rb.AddForce(slideVelocity * wallSlideSpeed * Time.deltaTime);

            rb.velocity = slideVelocity * wallSlideSpeed;
            
            // Debug.Log(slideVelocity + "Je;;pgj");
            // rb.AddForce(transform.forward * wallSlideSpeed * Time.deltaTime);
            
            // rb.AddForce(dirToWall * Time.deltaTime);
        }
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

        /*if (collision.transform.gameObject.layer == wallLayerValue && Input.GetButton("Jump") && !isSliding && !isGrounded)
        {
            Debug.Log("Hello There");
            startWallSlide();
        }
        */
        
        // return;
        // If crouching and collision is enemy, kill enemy.
        // Allows sliding into enemies
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Debug.Log("Hola");
        if (hit.transform.gameObject.layer == wallLayerValue)
            // Debug.Log("Ho");
        if (hit.collider.transform.gameObject.layer == wallLayerValue && Input.GetButton("Jump") && !isSliding && !isGrounded)
        {
            // Debug.Log("Hello there");
            startWallSlide(hit);
        }
    }

    private void startWallSlide(ControllerColliderHit hit)
    {
        isSliding = true;

        // no falling
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        
        rb.isKinematic = false;
        rb.velocity = controller.velocity;
        // wallSlideSpeed = controller.velocity.magnitude;
        // startVelocity = controller.velocity.magnitude;

        // slideVelocity = controller.velocity.normalized;
        // slideVelocity = hit.moveDirection;
        
        // direction to move the playing while wall sliding
        slideVelocity = transform.forward - hit.normal * Vector3.Dot(transform.forward, hit.normal);
        
        controller.enabled = false;

        // direction to the wall
        dirToWall = -hit.normal;
    }

    private void stopWallSlide()
    {
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        controller.enabled = true;
        
        // Reset rotation if your wall slide into a ramp
        Quaternion resetRotation = Quaternion.Euler(Quaternion.identity.eulerAngles.x, transform.rotation.eulerAngles.y, Quaternion.identity.eulerAngles.z);

        transform.rotation = resetRotation;
        isSliding = false;
        // Allow you to jump off a wall
        nextJump = Time.time + timeJump;
    }

    private void startCrouch()
    {
        Vector3 currentPos = camera.position;
        currentPos -= crouchOffset;
        /* float capsuleMove = GetComponent<CapsuleCollider>().height / 2;
        GetComponent<CapsuleCollider>().height = capsuleMove; ///= 2;
        GetComponent<CapsuleCollider>().center -= new Vector3(0, capsuleMove, 0);
        transform.GetChild(0).transform.position =
            new Vector3(transform.position.x, transform.position.y / 2, transform.position.z);
        transform.GetChild(0).localScale = new Vector3(1.2f, 0.9f, 1.2f); */
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

    private void OnDrawGizmosSelected() {
        Gizmos.DrawSphere(wallCheck.position, wallCheckDist);
    }
}