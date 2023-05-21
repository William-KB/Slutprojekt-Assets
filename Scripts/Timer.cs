using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float currentTime;
    private float BestTime;

    private bool HasBestTime;
    private bool TimerEnabled;
    // Start is called before the first frame update
    void Start()
    {
 
        if(PlayerPrefs.GetInt("TimerEnabled") == 1)
        {
            TimerEnabled = true;
        }

        if(SceneManager.GetActiveScene().name == "Level 1")
        {
            currentTime = 0f;
        }
    }

    void OnEnable()
    {
        currentTime = PlayerPrefs.GetFloat("Time");
        BestTime = PlayerPrefs.GetFloat("BestTime");
        if(BestTime > 30)
        {
            HasBestTime = true;
        }
        else
        {
            HasBestTime = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if(TimerEnabled)
       {
            Count();
       }
    }

    void Count()
    {
        currentTime = currentTime + Time.deltaTime;

        timerText.text = currentTime.ToString("0.00");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StopTimer();
        }
    }

    void OnDisable()
    {
        SaveTime();
    }

    void SaveTime()
    {   
        PlayerPrefs.SetFloat("Time", currentTime);
    }

    void EndTimer()
    {
        if(currentTime < BestTime || !HasBestTime)
        {
            PlayerPrefs.SetFloat("BestTime", currentTime);
        }
        
    }

    public void StartTimer()
    {
        TimerEnabled = true;
    }

    void StopTimer()
    {
        TimerEnabled = false;
        EndTimer();
    }


    public void Validation(int IsTimerEnabled)
    {
        PlayerPrefs.SetInt("TimerEnabled", IsTimerEnabled);
    }
}
