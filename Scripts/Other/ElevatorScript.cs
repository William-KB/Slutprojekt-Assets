using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    bool isOpen;
    bool canOpen;
    float Distance;
    public Collider boxCollider;
    GameObject player;
    
    public Animator ElevatorAnim;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        canOpen = true;
        boxCollider.enabled = false;
    }

    void Update()
    {
        Distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if(Distance < 20f && canOpen)
        {
            ElevatorAnim.SetBool("IsOpen", true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("e;e;");
            canOpen = false;

            boxCollider.enabled = true;

            ElevatorAnim.SetBool("IsOpen", false);  
        }   
    }
}
