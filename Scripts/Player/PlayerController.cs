using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [Header("player Settings")]
    public float minmaxCamPitch;
    public float FOV;
    public float sensX;
    public float sensY;
    public float jumpForce;
    public float jumpDelay;
    public float airDrag;
    public float groundDrag;
    public float airMultiplier;
    public float walkMultiplier;
    public float sprintSpeed;
    public float walkSpeed;
    public float sprintMultiplier;
    public float maxAirSpeed;
    public float Health;
    [Space(20)]
    
    [Header("Refrences")]
    public Rigidbody rb;
    public Camera cameraOBJ;
    public Transform cameraRotator;

    public static Transform playerTransform;

    public SettingsScript settingsScript;
    public GameObject deathScreen;

    public MenuScript menuScript;
    

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool doGroundCheck;
    [HideInInspector] public bool readyToJump;
    [HideInInspector] public float currentDrag;
    private float mouseX;
    private float mouseY;

    private float walkThing;
    private float strafeThing;
    private float sprintMult;

    private bool isDead;

    private bool DisableCamera;

    private int collisionMask = 1 << 8;
    
    [Space(20)]
    
    [Header("Events")]

    public UnityEvent playerDeath;

    public UnityEvent pauseMenu;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;

        mouseX = PlayerPrefs.GetFloat(CurrentScene + "X rotation");
        mouseY = PlayerPrefs.GetFloat(CurrentScene + "Y rotation");

        doGroundCheck = true;
        readyToJump = true;
        doGroundCheck = true;
        Inputs();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        SpeedCheck();

        // set the FOV and sens from settings to playercamera
        FOV = settingsScript.fieldOfView;
        cameraOBJ.fieldOfView = FOV;

        DisableCamera = menuScript.isPaused;

        sensX = settingsScript.Sensitivity;
        sensY = settingsScript.Sensitivity;

        if(isDead == false && !DisableCamera)
        {
            CameraControlls();
        }
       
    }

    

    void CameraControlls()
    {
        //Get mouse inputs
        mouseY += Input.GetAxis("Mouse Y") * sensY;
        mouseX += Input.GetAxis("Mouse X") * sensX;

        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        
        //apply the clamped rotation
        cameraRotator.transform.localRotation = Quaternion.Euler(-mouseY, 0f, 0f); 
        gameObject.transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
    }

    void Inputs()
    {   
        //movement vectors and inputs
        float strafe = Input.GetAxis("Strafe");
        float walk = Input.GetAxis("Walk");

        walkThing = walk;
        strafeThing = strafe;  

        //sprint inputs
        if (Input.GetAxis("Sprint") > 0f)
        {
            sprintMult = sprintMultiplier;
        } else sprintMult = 1f;


        // ground check and jump stuff
        if(doGroundCheck)
        {
            isGrounded = Physics.Raycast(rb.transform.position, rb.transform.up *-1f, 1.1f, ~collisionMask);
        }
        //Debug.DrawRay(rb.transform.position, rb.transform.up *-1.1f, Color.green, 2f);
        //Debug.Log("1: " + isGrounded + " 2: " + readyToJump);
    
        if ((isGrounded && Input.GetAxis("Jump") > 0 && readyToJump))
        {
            Jump();
        }
    }

    void Movement()
    {
        // get forwards and right vectors with all of the multipliers.
        Vector3 rbForward = rb.transform.forward * walkThing * walkMultiplier * sprintMult;
        Vector3 rbRight = rb.transform.right * strafeThing * walkMultiplier * sprintMult;
        
        if(isGrounded == false)
        {
            //
            rb.AddForce(rbForward/walkMultiplier/sprintMult * airMultiplier , ForceMode.Force);
            rb.AddForce(rbRight/walkMultiplier/sprintMult * airMultiplier , ForceMode.Force);
        }
        else 
        {
            rb.AddForce(rbForward, ForceMode.Force);
            rb.AddForce(rbRight, ForceMode.Force);
        }

        if(isGrounded == false && doGroundCheck)
        {
            currentDrag = airDrag;
        } else if (isGrounded && doGroundCheck)
        {
            currentDrag = groundDrag;
        }
        
        rb.drag = currentDrag;
        
    }

    void SpeedCheck()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); 
        float totalSpeed;

        if (Input.GetAxis("Sprint") > 0f)
        {
            totalSpeed = (sprintSpeed - walkSpeed) + walkSpeed;
        } else 
        totalSpeed = walkSpeed;

        if (isGrounded == false)
        {
            totalSpeed = maxAirSpeed;
        } else 
        totalSpeed = walkSpeed;

        if (flatVel.magnitude > totalSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * totalSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public void Jump()
    {
        readyToJump = false; 
        rb.AddForce(rb.transform.up * jumpForce/10, ForceMode.Impulse);
        Invoke(nameof(ResetJump), jumpDelay);
    }

    public void ResetJump()
    {
        readyToJump = true;
    }
    void FixedUpdate()
    {

        if(isDead == false)
        {
            Movement();
        }
        //Debug.Log("W: "+ walkThing + ", S: " + strafeThing + ", V; " + rb.velocity.magnitude);
    }

        void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            DamagePlayer();
        }
    }

    void DamagePlayer()
    {
        Health -= 15;
        Health = Mathf.Clamp(Health, 0f, 100f);

        if(Health == 0 && isDead == false)
        {
            die();
        }
    }

    public void die()
    {
        playerDeath.Invoke();

        isDead = true;

        rb.freezeRotation = false;

        rb.AddRelativeTorque(10f, 0f, 0f);

        Cursor.lockState = CursorLockMode.None;
    }
}
