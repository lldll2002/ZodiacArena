using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealCard : MonoBehaviour
{
    public GameObject[] dealtCard;
    public int cardGenerate;
    public GameObject highButton;
    public GameObject lowButton;
    public GameObject dealButton;

    public void DealMyNewCard()
    {
        cardGenerate = Random.Range(2, 15);
        dealtCard[cardGenerate].SetActive(true);
        highButton.SetActive(true);
        lowButton.SetActive(true);
        dealButton.SetActive(false);
        CardControll.dealtCardNumber = cardGenerate;
    }
}
