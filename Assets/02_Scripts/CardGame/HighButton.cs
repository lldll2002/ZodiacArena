using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighButton : MonoBehaviour
{
    public GameObject[] dealtCard;
    public int cardGenerate;

    public void DealHighCard()
    {
        cardGenerate = Random.Range(2, 15);
        dealtCard[cardGenerate].SetActive(true);
        CardControll.newCardNumber = cardGenerate;
        CardControll.guessHigh = true;
    }
}
