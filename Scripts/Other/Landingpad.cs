using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Landingpad : MonoBehaviour
{
    public GameObject deathfloor;

    void OnTriggerEnter(Collider other)
    {
        deathfloor.SetActive(false);
    }
}
