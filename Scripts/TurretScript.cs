using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public float rSpeed;

    public float hp;

    public ParticleSystem particles;
    public GameObject bullet;
    public Transform playerT;
    private Transform muzzle;
    private Transform turret;

    private Rigidbody rb;

    private Rigidbody playerRB;
    private Transform pCapsule;
    private Transform playerRaycastHitPoint;
    


    private bool playerHit;
    private bool rayHit;
    private RaycastHit HitInfo;
    private bool canFire;

    private bool isDead;

    int layerMask = 1 << 8;
    // Start is called before the first frame update
    void Start()
    {
        //get turret and muzzle transform.
        turret = gameObject.transform.GetChild(1);
        muzzle = turret.transform.GetChild(0);

        rb = gameObject.GetComponent<Rigidbody>();

        //get player and playerraycasthitpoint
        GameObject playerGObj = GameObject.FindWithTag("Player");
        playerT = playerGObj.transform;

        pCapsule = playerGObj.transform.GetChild(0);
        playerRaycastHitPoint = pCapsule.transform.GetChild(0);
        
        Debug.Log(playerRaycastHitPoint);

        playerRB = playerGObj.GetComponent<Rigidbody>();


        //set up variables
        playerHit = false;
        canFire = true;
        isDead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TargetFind();

        if(playerHit && isDead == false)
        {
            FaceTarget();
        }

        if(playerHit && canFire && isDead == false)
        {
            turretFire();
        }
    }

    void TargetFind()
    {
        Vector3 direction = (playerRaycastHitPoint.position - turret.transform.position).normalized;

       // Debug.DrawRay(turret.transform.position, direction * 1000 ,Color.green, 3);
        //raycast
        Physics.Raycast(turret.transform.position, direction, out HitInfo , 10000f, ~layerMask);
        if(HitInfo.collider.tag == "Player")
        {
            playerHit = true;
        } 
        else
        {
            playerHit = false;
        }        
    }
    
    void FaceTarget()
    {
        // find rotation, apply rotation.
        Vector3 direction = (playerT.position + new Vector3(0f, 0.65f, 0f) - turret.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, lookRotation * Quaternion.Euler(-90f,-90f,0f), rSpeed);
    }

    void turretFire()
    {

        
        Quaternion rotation = muzzle.transform.rotation * Quaternion.Euler(0f, 0f, 180f);

        var bulletProjectile = Instantiate(bullet, muzzle.transform.position, rotation);
        
        Rigidbody bulletRB = bulletProjectile.GetComponent<Rigidbody>();

        bulletRB.AddRelativeForce(-300f, Random.Range(-3f, 3f), Random.Range(-3f, 3f));

        canFire = false;
        Invoke(nameof(resetFire), 0.1f);
    }

    void resetFire()
    {
        canFire = true;
    }

    void Damage()
    {
        hp -= 34;
        hp = Mathf.Clamp(hp, 0f, 100f);

        //Debug.Log("turret HP: " + hp);

        if(hp == 0 && isDead == false)
        {
            die();
        }
    }

    void die()
    {
        particles.Play(true);

        isDead = true;

        playerRB.AddExplosionForce(8f, rb.transform.position, 6, 4f, ForceMode.Impulse);

        rb.AddForce(Vector3.up * 3 , ForceMode.Impulse);

        Vector3 Tourque = new Vector3(Random.Range(-5f,5f), Random.Range(-5f,5f), Random.Range(-5f,5f));
        rb.AddTorque(Tourque, ForceMode.Impulse);
        
        //gameObject.SetActive(false);
    }
    

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            Damage();
        }
        
    }
}
