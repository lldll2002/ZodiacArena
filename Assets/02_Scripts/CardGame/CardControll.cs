using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardControll : MonoBehaviour
{
    public static int dealtCardNumber;
    public static int newCardNumber;
    public GameObject correctText;
    public GameObject incorrectText;
    public static bool guessHigh = false;
    public static bool guessLow = false;
    public GameObject highButton;
    public GameObject lowButton;
    public GameObject dealButton;
    public GameObject[] dealingLeftCards;
    public GameObject[] turningRightCards;



    // 동일한 수 = 승자 ? 비김 ?

    void Update()
    {
        if (guessHigh == true)
        {
            guessHigh = false;
            highButton.SetActive(false);
            lowButton.SetActive(false);
            StartCoroutine(GuessingHigher());
        }
        if (guessLow == true)
        {
            guessLow = false;
            highButton.SetActive(false);
            lowButton.SetActive(false);
            StartCoroutine(GuessingLower());
        }
    }

    IEnumerator GuessingHigher()
    {
        yield return new WaitForSeconds(1);
        if (newCardNumber >= dealtCardNumber)
        {
            correctText.SetActive(true);
            GlobalScore.currentScore += 1;
            yield return new WaitForSeconds(3);
            dealingLeftCards[2].SetActive(false);
            dealingLeftCards[3].SetActive(false);
            dealingLeftCards[4].SetActive(false);
            dealingLeftCards[5].SetActive(false);
            dealingLeftCards[6].SetActive(false);
            dealingLeftCards[7].SetActive(false);
            dealingLeftCards[8].SetActive(false);
            dealingLeftCards[9].SetActive(false);
            dealingLeftCards[10].SetActive(false);
            dealingLeftCards[11].SetActive(false);
            dealingLeftCards[12].SetActive(false);
            dealingLeftCards[13].SetActive(false);
            dealingLeftCards[14].SetActive(false);
            turningRightCards[2].SetActive(false);
            turningRightCards[3].SetActive(false);
            turningRightCards[4].SetActive(false);
            turningRightCards[5].SetActive(false);
            turningRightCards[6].SetActive(false);
            turningRightCards[7].SetActive(false);
            turningRightCards[8].SetActive(false);
            turningRightCards[9].SetActive(false);
            turningRightCards[10].SetActive(false);
            turningRightCards[11].SetActive(false);
            turningRightCards[12].SetActive(false);
            turningRightCards[13].SetActive(false);
            turningRightCards[14].SetActive(false);
            turningRightCards[newCardNumber].SetActive(true);
            yield return new WaitForSeconds(1);
            highButton.SetActive(true);
            lowButton.SetActive(true);
            dealtCardNumber = newCardNumber;
            correctText.SetActive(false);
        }
        else
        {
            incorrectText.SetActive(true);
            GlobalScore.currentScore = 0;
            yield return new WaitForSeconds(3);
            dealingLeftCards[2].SetActive(false);
            dealingLeftCards[3].SetActive(false);
            dealingLeftCards[4].SetActive(false);
            dealingLeftCards[5].SetActive(false);
            dealingLeftCards[6].SetActive(false);
            dealingLeftCards[7].SetActive(false);
            dealingLeftCards[8].SetActive(false);
            dealingLeftCards[9].SetActive(false);
            dealingLeftCards[10].SetActive(false);
            dealingLeftCards[11].SetActive(false);
            dealingLeftCards[12].SetActive(false);
            dealingLeftCards[13].SetActive(false);
            dealingLeftCards[14].SetActive(false);
            turningRightCards[2].SetActive(false);
            turningRightCards[3].SetActive(false);
            turningRightCards[4].SetActive(false);
            turningRightCards[5].SetActive(false);
            turningRightCards[6].SetActive(false);
            turningRightCards[7].SetActive(false);
            turningRightCards[8].SetActive(false);
            turningRightCards[9].SetActive(false);
            turningRightCards[10].SetActive(false);
            turningRightCards[11].SetActive(false);
            turningRightCards[12].SetActive(false);
            turningRightCards[13].SetActive(false);
            turningRightCards[14].SetActive(false);
            yield return new WaitForSeconds(1);
            dealButton.SetActive(true);
            incorrectText.SetActive(false);
        }
    }
    IEnumerator GuessingLower()
    {
        yield return new WaitForSeconds(1);
        if (newCardNumber <= dealtCardNumber)
        {
            correctText.SetActive(true);
            GlobalScore.currentScore += 1;
            yield return new WaitForSeconds(3);
            dealingLeftCards[2].SetActive(false);
            dealingLeftCards[3].SetActive(false);
            dealingLeftCards[4].SetActive(false);
            dealingLeftCards[5].SetActive(false);
            dealingLeftCards[6].SetActive(false);
            dealingLeftCards[7].SetActive(false);
            dealingLeftCards[8].SetActive(false);
            dealingLeftCards[9].SetActive(false);
            dealingLeftCards[10].SetActive(false);
            dealingLeftCards[11].SetActive(false);
            dealingLeftCards[12].SetActive(false);
            dealingLeftCards[13].SetActive(false);
            dealingLeftCards[14].SetActive(false);
            turningRightCards[2].SetActive(false);
            turningRightCards[3].SetActive(false);
            turningRightCards[4].SetActive(false);
            turningRightCards[5].SetActive(false);
            turningRightCards[6].SetActive(false);
            turningRightCards[7].SetActive(false);
            turningRightCards[8].SetActive(false);
            turningRightCards[9].SetActive(false);
            turningRightCards[10].SetActive(false);
            turningRightCards[11].SetActive(false);
            turningRightCards[12].SetActive(false);
            turningRightCards[13].SetActive(false);
            turningRightCards[14].SetActive(false);
            turningRightCards[newCardNumber].SetActive(true);
            yield return new WaitForSeconds(1);
            highButton.SetActive(true);
            lowButton.SetActive(true);
            dealtCardNumber = newCardNumber;
            correctText.SetActive(false);
        }
        else
        {
            incorrectText.SetActive(true);
            GlobalScore.currentScore = 0;
            yield return new WaitForSeconds(3);
            dealingLeftCards[2].SetActive(false);
            dealingLeftCards[3].SetActive(false);
            dealingLeftCards[4].SetActive(false);
            dealingLeftCards[5].SetActive(false);
            dealingLeftCards[6].SetActive(false);
            dealingLeftCards[7].SetActive(false);
            dealingLeftCards[8].SetActive(false);
            dealingLeftCards[9].SetActive(false);
            dealingLeftCards[10].SetActive(false);
            dealingLeftCards[11].SetActive(false);
            dealingLeftCards[12].SetActive(false);
            dealingLeftCards[13].SetActive(false);
            dealingLeftCards[14].SetActive(false);
            turningRightCards[2].SetActive(false);
            turningRightCards[3].SetActive(false);
            turningRightCards[4].SetActive(false);
            turningRightCards[5].SetActive(false);
            turningRightCards[6].SetActive(false);
            turningRightCards[7].SetActive(false);
            turningRightCards[8].SetActive(false);
            turningRightCards[9].SetActive(false);
            turningRightCards[10].SetActive(false);
            turningRightCards[11].SetActive(false);
            turningRightCards[12].SetActive(false);
            turningRightCards[13].SetActive(false);
            turningRightCards[14].SetActive(false);
            yield return new WaitForSeconds(1);
            highButton.SetActive(true);
            incorrectText.SetActive(false);

        }
    }
}
