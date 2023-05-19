using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Deathfloor : MonoBehaviour
{
    public UnityEvent deathFloor;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            deathFloor.Invoke();
        }
    }
}
