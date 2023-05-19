using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slidescript : MonoBehaviour
{
    public float addSpeed;
    public float slideResetTime;

    
    public PlayerController pControllerScript;
    public Rigidbody rb;
    public CapsuleCollider CC;

    private float slideDrag;
    private bool canSlide;
    
    private bool isPressed;
    [HideInInspector] public bool isSliding;
    [HideInInspector] public bool slideGround;
    private int collisionMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        isSliding = false;
        canSlide = true;
        slideDrag = 0f;
        pControllerScript.doGroundCheck = true;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        if(pControllerScript.doGroundCheck == false)
        {
            SlideGCheck();
        } else
        {
            slideGround = false;
        }
    }
    
    void Inputs()
    {
        if(canSlide && Input.GetButtonDown("Slide") && (rb.velocity.magnitude > 8f || (Input.GetAxis("Sprint") > 0f)))
        {
            isSliding = true;
            canSlide = false;
            pControllerScript.doGroundCheck = false;
            pControllerScript.isGrounded = false;
            SlideState();
            Invoke(nameof(SlideDragIncrease), 3.5f);
        }

        if(Input.GetButtonDown("Jump") && slideGround && pControllerScript.readyToJump)
        {
            pControllerScript.Jump();
        }

        if(isSliding && Input.GetButtonUp("Slide") || rb.velocity.magnitude < 5f)
        {
            StopSlide();
        } 
    }

    void SlideGCheck()
    {
        slideGround = Physics.Raycast(rb.transform.position, rb.transform.up *-1f, 0.55f, ~collisionMask);
    }

    void ResetSlide()
    {
        canSlide = true;
    }


    void SlideState()
    {
        //change scale and add to total flatspeed
        CC.height = 1f;

        pControllerScript.currentDrag = 0f;

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); 
        
        Vector3 Slidespeed = flatVel.normalized * addSpeed;
        rb.velocity = new Vector3(Slidespeed.x, rb.velocity.y, Slidespeed.z);
    }

    void SlideDragIncrease()
    {

        if(slideGround)
        {
            pControllerScript.currentDrag = 2f;
        }
       
    }

    void StopSlide()
    {
        rb.drag = pControllerScript.groundDrag;

        pControllerScript.doGroundCheck = true;

        slideDrag = pControllerScript.groundDrag;

        CC.height = 2f;

        isSliding = false;

        Invoke(nameof(ResetSlide), slideResetTime);
    }

}
