using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetallDoor : MonoBehaviour
{
    bool isOpen;
    bool canOpen;
    float Distance;
    public Collider boxCollider;
    GameObject player;
    
    public Animator doorAnim;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        canOpen = true;
    }

    void Update()
    {
        Distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if(Distance < 10f && canOpen)
        {
            doorAnim.SetBool("IsOpen", true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            canOpen = false;

            boxCollider.enabled = false;

            doorAnim.SetBool("IsOpen", false);  
        }   
    }
}
