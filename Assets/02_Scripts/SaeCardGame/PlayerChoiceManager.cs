using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChoiceManager : MonoBehaviour
{
    public Button headsButton; // 앞면 버튼
    public Button tailsButton; // 뒷면 버튼
    public Button coinButton; // 코인 버튼
    private SaeGameManager gameManager; // GameManager에 선택 결과를 전달하기 위한 참조

    void Start()
    {
        // GameManager 스크립트 찾기
        gameManager = FindObjectOfType<SaeGameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager를 찾을 수 없습니다.");
            return;
        }

        coinButton.interactable = false;

        // 버튼 클릭 이벤트 설정
        headsButton.onClick.AddListener(() => SetPlayerChoice("앞면"));
        tailsButton.onClick.AddListener(() => SetPlayerChoice("뒷면"));
    }

    public void SetPlayerChoice(string choice)
    {
        // GameManager에 선택 결과 전달
        gameManager.SetPlayerChoice(choice);
        // Debug.Log($"플레이어가 {choice}를 선택했습니다");

        // 버튼을 비활성화하여 중복 선택을 방지
        headsButton.interactable = false;
        tailsButton.interactable = false;
        coinButton.interactable = true;
    }
}
