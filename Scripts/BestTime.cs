using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestTime : MonoBehaviour
{
    public TextMeshProUGUI bestTimeText;
    // Start is called before the first frame update
    void Awake()
    {
        bestTimeText.text = "Best Time: " + PlayerPrefs.GetFloat("BestTime");
    }
}
