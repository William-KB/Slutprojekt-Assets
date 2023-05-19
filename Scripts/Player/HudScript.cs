using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudScript : MonoBehaviour
{
    public WeaponScript wpScript;
    public PlayerController playerScript;

    public TextMeshProUGUI AmmoCounter;
    public Image HealthBar;
    public Transform Camera;
    public Transform Weapon;
    public Transform Counter;
    public Transform crossHair;
    public Transform hpBar;

    public Material UImat;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Positions();

        HealthBar.fillAmount = playerScript.Health/100;

        //print ammo text
        AmmoCounter.text = wpScript.currentAmmo + " / " +  wpScript.ammo;

        ColorUpdate(wpScript.currentAmmo);
    }



    void Positions()
    {
        //fix ammo counter positon
        
        Counter.transform.position = Weapon.transform.position;
        

        //crosshair position

        float Foffset = (playerScript.FOV * -0.23f + 29.9f)/100;
        Vector3 offsetC = new Vector3 (0, 0f, Foffset ); // 0.0936f
        crossHair.transform.localPosition = offsetC;
        hpBar.transform.localPosition = offsetC;
        
    }




    void ColorUpdate(int ammoCount)
    {
        Vector4 lowAmmoColor = new Vector4(0.9f, 0.6f, 0.6f, 0.5f);
        Vector4 ammoColor = new Vector4(1f, 1f, 1f, 0.5f);


        //switch between colors
        if(ammoCount < 4)
        {
            AmmoCounter.color = lowAmmoColor;
        } 
        else
        {
            AmmoCounter.color = ammoColor;
        }
    }
}
