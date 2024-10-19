using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowButton : MonoBehaviour
{
    public GameObject[] dealtCard;
    public int cardGenerate;

    public void DealLowCard()
    {
        cardGenerate = Random.Range(2, 15);
        dealtCard[cardGenerate].SetActive(true);
        CardControll.newCardNumber = cardGenerate;
        CardControll.guessLow = true;
    }
}
