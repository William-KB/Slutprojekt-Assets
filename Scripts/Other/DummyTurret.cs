using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTurret : MonoBehaviour
{
    public GameObject bullet;
    private Transform muzzle;
    private Transform turret;

    private Rigidbody rb;
    private Transform pCapsule;
    
    private bool canFire;

    // Start is called before the first frame update
    void Start()
    {
        //get turret and muzzle transform.
        turret = gameObject.transform.GetChild(1);
        muzzle = turret.transform.GetChild(0);

        rb = gameObject.GetComponent<Rigidbody>();

        canFire = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         if(canFire)
        {
            turretFire();
        }
    }

    void turretFire()
    {
        
        Quaternion rotation = muzzle.transform.rotation * Quaternion.Euler(0f, 0f, 180f);

        var bulletProjectile = Instantiate(bullet, muzzle.transform.position, rotation);
        
        Rigidbody bulletRB = bulletProjectile.GetComponent<Rigidbody>();

        bulletRB.AddRelativeForce(-300f, Random.Range(-3f, 3f), Random.Range(-3f, 3f));

        canFire = false;
        Invoke(nameof(resetFire), 0.4f);
    }

        void resetFire()
    {
        canFire = true;
    }
}
