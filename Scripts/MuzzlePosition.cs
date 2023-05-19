using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzlePosition : MonoBehaviour
{
    public WeaponScript wpScript;

    public Transform muzzlePos;

    private Vector3 DefaultPos = new Vector3(0f, -0.01f, 0.5f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    
        if(wpScript.isAiming)
        {
            gameObject.transform.localPosition = DefaultPos;
            gameObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            gameObject.transform.position = muzzlePos.transform.position;
            gameObject.transform.rotation = muzzlePos.transform.rotation;
        }
        
    
    }
}
