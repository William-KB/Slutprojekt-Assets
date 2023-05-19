using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    public Slider FOVSlider;
    public TextMeshProUGUI FOVtext;
    public Slider SensSlider;
    public TextMeshProUGUI SensText;
    public float fieldOfView;
    public float Sensitivity;
    public Camera Cam;
    public PlayerController pController;

    void OnEnable()
    {
        fieldOfView = PlayerPrefs.GetFloat("Fov");
        Sensitivity = PlayerPrefs.GetFloat("Sensitivity");
    }

    // Update is called once per frame

    public void FOVSettings()
    {
        // get fov from slider and round up the horizontal fov
        fieldOfView = FOVSlider.value;
        FOVtext.text = "FOV: " + Mathf.Ceil(Camera.VerticalToHorizontalFieldOfView(fieldOfView, 1.778f));

        //check if main menu to apply fov to menu Cam
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            Cam.fieldOfView = fieldOfView;
        }
        
    }

    public void SensSettings()
    {
        Sensitivity = SensSlider.value;
        SensText.text = "Sensitivity: " + Mathf.Round(Sensitivity * 100)/100;
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat("Fov", fieldOfView);
        PlayerPrefs.SetFloat("Sensitivity", Sensitivity);
    }
}
