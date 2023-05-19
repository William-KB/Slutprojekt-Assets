using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WeaponScript : MonoBehaviour
{
    public int ammo;

    public float fireRate;
    public float reloadTime;
   
    [HideInInspector] public int currentAmmo;
    [HideInInspector] public bool isAiming;
    [HideInInspector] public bool isSideways;
    [HideInInspector] public bool isReload;
    private bool canReload;
    private bool isFire;
    private bool canFire;
    private bool firstPickup;
    public float bulletVelocity;
    public GameObject bullet;
    public GameObject casing;

    public Transform muzzle;
    public Transform shellEjector;
    
    public TextMeshProUGUI AmmoCounter;
    public Animator anim;

    public WallrunScript wrScript;
    public Slidescript sScript; 
    public PlayerController pController;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = ammo;

        firstPickup = SceneManager.GetActiveScene().name != "MainMenu";


        if(firstPickup)
        {
            anim.SetBool("FirstPickup",firstPickup);
            canFire = false;
            Invoke(nameof(resetFire),6);
        }
        else
        {
            canFire = true;
        }

        canReload = true;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        AnimationHandler();
    }


    void Inputs()
    {
        isAiming = Input.GetButton("Aim");
        
        isFire = Input.GetButton("Fire1");

        if(wrScript.isWallRunning || sScript.isSliding)
        {
            isSideways = true;
        } 

        if(wrScript.isWallRunning == false && sScript.isSliding == false)
        {
            isSideways = false;
        }


        if(Input.GetButtonDown("Fire1") && canFire && currentAmmo > 0)
        {
            Fire();
            FireAnims();
        }


        if(Input.GetButtonDown("Reload") && canReload && currentAmmo < 10)
        {
            canReload = false;
            canFire = false;
            anim.SetBool("IsReload", true);
            Reload();
        }

    }


    void Fire()
    {
        //decrease ammo amount
        currentAmmo--;

        canFire = false;
        canReload = false;
        Invoke(nameof(resetFire), fireRate);

        //get rotation for bullet
        Quaternion rotation = muzzle.transform.rotation * Quaternion.Euler(0f, -90f, 0f);

        //spawn bullet gamobject, find rigidbody, add force
        var bulletProjectile = Instantiate(bullet, muzzle.transform.position, rotation);
        Rigidbody bulletRB = bulletProjectile.GetComponent<Rigidbody>();

        bulletRB.AddRelativeForce(-bulletVelocity, 0f, 0f);

        //spawn casing, find rigidbody, add force and rotation
        var casingProjectile = Instantiate(casing, shellEjector.transform.position, rotation);
        
        Rigidbody casingRB = casingProjectile.GetComponent<Rigidbody>();
        
        casingRB.velocity = pController.rb.velocity;
        casingRB.AddRelativeForce(0f,Random.Range(75f, 110f),Random.Range(45f, 75f));
        casingRB.AddRelativeTorque(Random.Range(-175f,175f), Random.Range(-200f,220f), 0f);

    }

    void resetFire()
    {
        canFire = true;
        canReload = true;
    }

    void Reload()
    {
        Invoke(nameof(finishedReload), reloadTime);
        Invoke(nameof(resetReload), reloadTime);
    }

    void finishedReload()
    {
        anim.SetBool("IsReload", false);
        currentAmmo = ammo;
    }

    void resetReload()
    {
        canReload = true;
        canFire = true;
    }

    void AnimationHandler()
    {   
        anim.SetBool("IsAiming", isAiming);

        anim.SetBool("IsSide", isSideways);
    }

    void FireAnims()
    {
        //creates string to choose which fire anim
        string fireType = "Fire" + Random.Range(1,3);

        if(isAiming)
        {
            fireType = "Aim" + fireType;
        }

        if(isSideways && isAiming == false)
        {
            fireType = "Side" + fireType;
        }

        anim.Play(fireType);
    }
}
