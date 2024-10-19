using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SaeGameManager : MonoBehaviour
{
private List<Card> deck;
    private Player player1, player2;
    private bool isPlayer1Turn; // 현재 차례
    private int LossCount; // 현재 패배 횟수

    public Button highButton; // 버튼을 에디터에서 연결
    public Button lowButton;
    public Button coinButton;

    private bool isHighSelected; // 선택 결과를 저장
    private string playerChoice; // 플레이어의 선택 (앞면 or 뒷면)

    void Start()
    {
        InitializeGame();

        // 버튼 클릭 이벤트 설정
        highButton.onClick.AddListener(() => OnHighLowSelected(true));
        lowButton.onClick.AddListener(() => OnHighLowSelected(false));
        coinButton.onClick.AddListener(OnFlipButtonClicked);

        coinButton.interactable = false;
        highButton.interactable = false;
        lowButton.interactable = false;
    }

    void InitializeGame()
    {
        // 덱 초기화
        deck = new List<Card>();
        for (int i = 1; i <= 12; i++)
        {
            deck.Add(new Card(i));
        }

        // 플레이어 초기화
        player1 = new Player();
        player2 = new Player();

        // 카드 3장씩 나눠주기
        DealCards(player1);
        DealCards(player2);

        Debug.Log("동전의 앞면, 뒷면을 선택하세요");
    }

    public void SetPlayerChoice(string choice)
    {
        // 플레이어가 앞면 or 뒷면 선택
        playerChoice = choice;
        Debug.Log($"플레이어가 {playerChoice}를 선택했습니다.");
    }
    
    void OnFlipButtonClicked()
    {
        // 동전 던지기
        // CoinFlip.FlipCoin() 호출로 결과를 받아옴
        bool isHeads = CoinFlip.FlipCoin();
        string result = isHeads ? "앞면" : "뒷면";
        
        // 플레이어의 선택과 결과를 비교하여 차례 결정
        if (playerChoice == result)
        {
            isPlayer1Turn = true;
            Debug.Log("플레이어 1 차례입니다");
        }
        else
        {
            isPlayer1Turn = false;
            Debug.Log("플레이어 2 차례입니다");
        }
        
        highButton.interactable = true;
        lowButton.interactable = true;
        coinButton.interactable = false;
    }

    void DealCards(Player player)
    {
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, deck.Count);
            Card card = deck[randomIndex];
            player.AddCard(card);
            deck.RemoveAt(randomIndex);
        }
    }

    void OnHighLowSelected(bool isHigh)
    {
        isHighSelected = isHigh;
        Debug.Log(isHigh ? " High가 선택되었습니다. " : " Low가 선택되었습니다. ");

        // 초기 설정 완료 및 하이로우 선택 후 첫 라운드 시작
        StartRound();
    }


    void StartRound()
    {
        Debug.Log(" 새 게임을 시작합니다 " + (isPlayer1Turn ? "Player 1 차례입니다" : "플레이어 2 차례입니다"));
        // high/low 선택 단계
        // UI 로직 추가
    }

    public void PlayTurn(Card player1Card, Card player2Card, bool isHigh)
    {
        int player1Value = player1Card.Value;
        int player2Value = player2Card.Value;

        bool player1Wins = isHigh ? player1Value > player2Value : player1Value < player2Value;

        if (player1Wins)
        {
            Debug.Log(" 이번 라운드에서 Player1이 승리하였습니다. ");
            player2.LossCount++;
        }
        else
        {
            Debug.Log(" 이번 라운드에서 Player2가 승리하였습니다. ");
            player1.LossCount++;
        }

        // 패배 횟수 확인
        if (player1.LossCount >= 3)
        {
            EndGame(false); // Player2 승리
        }
        else if (player2.LossCount >= 3)
        {
            EndGame(true); // Player1 승리
        }
        else
        {
            // 패배한 사람이 다음 라운드의 High/Low를 선택
            isPlayer1Turn = player1Wins ? false : true;
            StartRound();
        }
    }

    void EndGame(bool player1Wins)
    {
        Debug.Log(player1Wins ? " Player1이 승리하였습니다 " : " Player2가 승리하였습니다 " );
        // 게임 종료 화면 표시 로직 추가
    }

}

