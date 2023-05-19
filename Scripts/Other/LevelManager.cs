using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraRotator;

    public UnityEvent LoadNextLevel;
    
    public int disableSmoothTransition;

    
    void OnEnable()
    {
        disableSmoothTransition = PlayerPrefs.GetInt("Disable Smooth Transition");

        
        if(SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "Level 1")
        {
            int CurrentScene = SceneManager.GetActiveScene().buildIndex;
            //load x y z pos and rot
            float Xpos = PlayerPrefs.GetFloat(CurrentScene + "X position");
            float Ypos = PlayerPrefs.GetFloat(CurrentScene + "Y position");
            float Zpos = PlayerPrefs.GetFloat(CurrentScene + "Z position");

            if(disableSmoothTransition == 0)
            {
                player.transform.position = new Vector3(Xpos, Ypos, Zpos);
            }

            
        }
        
    }

    public void RestartLevel()
    {
        //Restart the scene
        Scene CurrentScene =  SceneManager.GetActiveScene();

        SceneManager.LoadScene(CurrentScene.name, LoadSceneMode.Single);
    }

    public void StartLevel(int LevelNumber)
    {
        //Load scene from LevelNumber
        PlayerPrefs.SetInt("Disable Smooth Transition", 1);
        SceneManager.LoadScene("Level " + LevelNumber);
        
    }


    public void NextLevel(int NextLevelNumber)
    {
        PlayerPrefs.SetInt("Disable Smooth Transition", 0);
        SceneManager.LoadScene("Level " + NextLevelNumber);
    }

    void OnTriggerEnter(Collider other)
    {
        SavePlayerState();
        LoadNextLevel.Invoke();
    }


    void SavePlayerState()
    {
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        //save x y z pos and rot
        PlayerPrefs.SetFloat(CurrentScene + 1 + "X position", player.transform.position.x);
        PlayerPrefs.SetFloat(CurrentScene + 1 + "Y position", player.transform.position.y);
        PlayerPrefs.SetFloat(CurrentScene + 1 + "Z position", player.transform.position.z);

        PlayerPrefs.SetFloat(CurrentScene + 1 + "X rotation", cameraRotator.transform.eulerAngles.y);
        PlayerPrefs.SetFloat(CurrentScene + 1 + "Y rotation", player.transform.eulerAngles.x);
    }
}
