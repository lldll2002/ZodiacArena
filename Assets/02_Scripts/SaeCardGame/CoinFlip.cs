using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinFlip : MonoBehaviour
{
    public Button CoinButton;

    public static bool FlipCoin()
    {
        // 0 또는 1을 무작위로 생성하여 50% 확률로 결정
        bool isHeads = Random.Range(0, 2) == 0;
        string result = isHeads ? "앞면" : "뒷면";
        Debug.Log($"{result}이 나왔습니다!");

        return isHeads;
    }
}
