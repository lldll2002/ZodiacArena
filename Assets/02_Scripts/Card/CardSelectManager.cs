using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CardSelectManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private TMP_Text playerNamesText; // 플레이어 이름 표시
    [SerializeField] private TMP_Text winConditionText; // 승리 조건 표시
    [SerializeField] private TMP_Text infoText; // 안내 메시지 표시

    [Header("Card Buttons")]
    [SerializeField] private Button[] cardButtons; // 카드 버튼 배열

    private List<int> selectedCards = new List<int>(); // 플레이어가 선택한 카드
    private int totalCardsToSelect = 3; // 선택해야 할 카드 수

    private void Start()
    {
        UpdatePlayerNames();
        UpdateWinConditionText();
        ShuffleCardButtons();

        infoText.text = "카드를 3장 선택하세요.";
    }

    private void UpdatePlayerNames()
    {
        // 플레이어 이름 표시
        string otherPlayerNickName = PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.LocalPlayer.NickName
            ? PhotonNetwork.PlayerList[1].NickName
            : PhotonNetwork.PlayerList[0].NickName;

        playerNamesText.text = $"{PhotonNetwork.LocalPlayer.NickName} vs. {otherPlayerNickName}";
    }

    private void UpdateWinConditionText()
    {
        // 승리 조건 표시 (예시: "High")
        string winCondition = "High"; // 여기에 실제 CoinFlip 결과를 가져오는 로직 추가
        winConditionText.text = $"승리 조건: {winCondition}";
    }

    private void ShuffleCardButtons()
    {
        // 카드 버튼에 1~12 랜덤 번호 할당
        List<int> numbers = new List<int>();
        for (int i = 1; i <= cardButtons.Length; i++)
        {
            numbers.Add(i);
        }
        Shuffle(numbers);

        // 버튼에 번호 할당 및 클릭 이벤트 등록
        for (int i = 0; i < cardButtons.Length; i++)
        {
            int cardNumber = numbers[i];
            int buttonIndex = i; // 클로저 문제를 피하기 위해

            cardButtons[i].onClick.AddListener(() => OnCardButtonClicked(cardNumber, buttonIndex));
            cardButtons[i].gameObject.SetActive(true); // 버튼 활성화
        }
    }

    private void Shuffle(List<int> list)
    {
        // 리스트를 랜덤하게 섞는 메서드
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void OnCardButtonClicked(int cardNumber, int buttonIndex)
    {
        // 카드 선택 처리
        if (selectedCards.Count < totalCardsToSelect)
        {
            selectedCards.Add(cardNumber);
            cardButtons[buttonIndex].interactable = false; // 버튼 비활성화
                                                           // 선택된 카드를 표시하기 위해 버튼의 텍스트를 변경
                                                           //cardButtons[buttonIndex].GetComponentInChildren<TMP_Text>().text = $"선택됨: {cardNumber}";
            UpdateInfoText();
        }

        if (selectedCards.Count >= totalCardsToSelect)
        {
            // 모든 카드 선택 완료
            infoText.text = "모든 카드를 선택했습니다. 다른 플레이어가 선택하기를 기다리는 중...";

            // 선택한 카드를 Photon Custom Properties에 저장
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties.Add("selectedCards", selectedCards.ToArray()); // 카드 배열로 저장
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

            // 다른 플레이어가 선택할 때까지 대기 후 씬 전환
            StartCoroutine(CheckAllPlayersReady());
        }
    }

    private void UpdateInfoText()
    {
        // 현재 선택된 카드 수에 따라 안내 메시지 업데이트
        int remainingCards = totalCardsToSelect - selectedCards.Count;

        if (remainingCards > 0)
        {
            infoText.text = $"{remainingCards}장 더 선택해주세요.";
        }
        else
        {
            infoText.text = "모든 카드를 선택했습니다. 다른 플레이어가 선택하기를 기다리는 중...";
        }
    }

    // 모든 플레이어가 선택 완료했는지 확인하는 코루틴
    private IEnumerator CheckAllPlayersReady()
    {
        bool allPlayersReady = false;

        while (!allPlayersReady)
        {
            allPlayersReady = true;
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                // 각 플레이어가 선택한 카드가 있는지 확인
                if (!player.CustomProperties.ContainsKey("selectedCards"))
                {
                    allPlayersReady = false;
                    break;
                }
            }
            yield return new WaitForSeconds(1f); // 1초 대기 후 다시 확인
        }

        // 모든 플레이어가 카드를 선택 완료한 경우 씬 전환
        PhotonNetwork.LoadLevel("FightCardSelect");
    }
}
