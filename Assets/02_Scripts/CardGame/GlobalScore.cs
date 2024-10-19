using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalScore : MonoBehaviour
{
    public static int currentScore;
    public GameObject scoreDisplay;
    public int highScore;
    public GameObject bestDisplay;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("BestScoreHilo");
        bestDisplay.GetComponent<TMP_Text>().text = "BEST :" + highScore;
    }

    void Update()
    {
        scoreDisplay.GetComponent<TMP_Text>().text = "STREAK: " + currentScore;
        if (currentScore > highScore)
        {
            bestDisplay.GetComponent<TMP_Text>().text = "BEST :" + currentScore;
            PlayerPrefs.SetInt("BestScoreHilo", currentScore);
        }
    }
}
