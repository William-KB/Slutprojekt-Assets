using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject levelsMenu;
    public GameObject pauseMenu;
    public GameObject deathMenu;

    public bool isMainMenu;

    public bool isPaused;

    public bool playerDead;
    
    // Start is called before the first frame update
    void Start()
    {
        isMainMenu = SceneManager.GetActiveScene().name == "MainMenu";
        Time.timeScale = 1;
    }



    ////test to fix display
     void Awake()
    {
#if !UNITY_EDITOR
        // fixes new Unity2021 bug where the secondary monitor was being chosen when the game launches in a release build
        StartCoroutine("MoveToPrimaryDisplay");
#endif
    }
 
    IEnumerable MoveToPrimaryDisplay()
    {
        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);
        if (displays?.Count > 0)
        {
            var moveOperation = Screen.MoveMainWindowTo(displays[0], new Vector2Int(displays[0].width / 2, displays[0].height / 2));
            yield return moveOperation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        if(Input.GetButtonDown("Escape") && !isMainMenu)
        {
            PauseMenu();
        }
    }

    public void DeathMenu()
    {
        Cursor.lockState = CursorLockMode.None;

        playerDead = true;
        
        Invoke(nameof(DeathMenuActual), 1.5f);
        
    }

    public void DeathMenuActual()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0f;
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);

    }

    public void PauseMenu()
    {
        if(!playerDead)
        {
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;

            isPaused = true;

            Time.timeScale = 0f;
            settingsMenu.SetActive(false); 
        }
        
        if(playerDead)
        {
            DeathMenuActual();  
        }
        
    }

    public void MainMenu()
    {
        //set the other menus to false and do the same for settings and levelmenus;
        Cursor.lockState = CursorLockMode.None;
        settingsMenu.SetActive(false);
        levelsMenu.SetActive(false);
        
        mainMenu.SetActive(true);
    }

    public void SettingsMenu()
    {
        
        settingsMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        if(!isMainMenu)
        {
            //if we are not in main menu
            pauseMenu.SetActive(false);
            deathMenu.SetActive(false);
        }
        

        if(isMainMenu)
        {
            //if we are in main menu
            mainMenu.SetActive(false);
            levelsMenu.SetActive(false);  
        }
    }

    public void LevelsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        
        levelsMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        isPaused = false;

        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
