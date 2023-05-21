using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WallrunScript : MonoBehaviour
{
    public float WallrunSpeed;
    public float maxWallrunTime;
    public Transform orientation;
    public float wallCheckDistance;
    public Rigidbody rb;
    public float wallRunGravity;

    private Vector3 Defaultgravity;
    private float currentGravity;
    
    [Space(20)]
    
    [Header("Refrences")]
    public PlayerController pControllerScript;
    public Slidescript slidescript;

    public Animator animator;
    private bool rightWall;
    private bool leftWall;
    private RaycastHit rightWallHit;
    private RaycastHit leftWallHit;

    [HideInInspector] public bool isWallRunning;
    private bool canWallrun;
    private int maxDownspeed;
    private float forceMult;

    Vector3 wallForward;
    Vector3 wallNormal;

    private int collisionMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        Defaultgravity = new Vector3(0f, -9.8f, 0f);
        maxDownspeed = -2;
        canWallrun = true;
        forceMult = 2;
    }

    // Update is called once per frame
    void Update()
    {
        wallRunCheck();

        if(isWallRunning == false)
        {
            Physics.gravity = Defaultgravity;
            forceMult = 20;
        }

        CameraAnimations();

        if (isWallRunning && Input.GetButtonDown("Jump") && canWallrun)
        {
            WallJump();
        }
    }

    void FixedUpdate()
    {
        
        //check if i can wallrun
        if(isWallRunning && canWallrun)
        {
            wallRunMovement();
            
            

            //
            if(isWallRunning == false)
            {
                canWallrun = false;
            }

           
        }
    }

    void WallJump()
    {
        stopWallrun();

        pControllerScript.Jump();

        //apply force upwards forwards and from the wall 

        Vector3 wallJumpForce = rb.transform.forward * 1.2f + rb.transform.up * 0.65f + wallNormal * 3f;
        rb.AddForce(wallJumpForce, ForceMode.Impulse);
        
    }


    void wallRunCheck()
    {
        //info about wall from raycasts   
        rightWall = Physics.Raycast(transform.position, rb.transform.right * 1.0f, out rightWallHit, wallCheckDistance, ~collisionMask);
        leftWall = Physics.Raycast(transform.position, rb.transform.right * -1.0f, out leftWallHit, wallCheckDistance, ~collisionMask);

        //Debug.DrawRay(transform.position, rb.transform.right * 1.0f);
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); 

        //if i can wallrun
        if((rightWall || leftWall) && flatVel.magnitude > 4f && pControllerScript.isGrounded == false)
        {
            isWallRunning = true;
            forceMult = 5;
        } else 
        {
            isWallRunning = false;
        }
    }

    void wallRunMovement()
    {   
        //gets wall normal and wall forward
        wallNormal = rightWall ? rightWallHit.normal : leftWallHit.normal;
        
        wallForward = Vector3.Cross(wallNormal, transform.up);


        //changes gravity
        currentGravity = wallRunGravity;

        Physics.gravity = new Vector3(0f, currentGravity, 0f);

        //checks which way the wall force should be applied
        if((orientation.forward - -wallForward).magnitude > (orientation.forward - wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
        
        rb.AddForce(-wallForward, ForceMode.Force);

        //adds force
        if(rightWall)
        {
            rb.AddForce(-wallNormal * rightWallHit.distance * forceMult, ForceMode.Force);
        } else
        {
            rb.AddForce(-wallNormal * leftWallHit.distance * forceMult, ForceMode.Force);
        }

        UpspeedCheck();
    }

  

    void UpspeedCheck()
    {   
        //limit max up/down velocity when wallrunning
        if (rb.velocity.y < maxDownspeed || rb.velocity.y > 4f)
        {
            rb.AddForce(rb.transform.up * (-rb.velocity.y) * 8);
        }
    }

    void ResetWallRun()
    {
        canWallrun = true;
        maxDownspeed = -50;
    }

    void stopWallrun()
    {
        if(isWallRunning)
        {   
            maxDownspeed = -100;
            rb.AddForce(wallNormal * 0.2f, ForceMode.Impulse);  
            canWallrun = false;

            Invoke(nameof(ResetWallRun), 0.75f);
        }
    }


    void CameraAnimations()
    {
        animator.SetBool("isWallrunning", isWallRunning);
        animator.SetBool("RightWall", rightWall);
    }
}
