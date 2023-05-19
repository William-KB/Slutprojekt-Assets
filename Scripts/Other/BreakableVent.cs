using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableVent : MonoBehaviour
{
    private Rigidbody rb;



    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            rb.isKinematic = false;
        }
    }
}
