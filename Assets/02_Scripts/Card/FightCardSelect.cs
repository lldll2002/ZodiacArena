using Photon.Pun;
using UnityEngine;
using UnityEngine.UI; // Button을 사용하기 위해 추가
using TMPro; // TMP_Text를 사용하기 위해 추가
using ExitGames.Client.Photon; // Hashtable을 사용하기 위해 추가
using System.Collections;

public class FightCardSelect : MonoBehaviourPunCallbacks
{
    [Header("CardButtons")]
    [SerializeField] private Button[] cardButtons; // 카드 버튼 배열

    [Header("CardButtonText")]
    [SerializeField] private TMP_Text[] cardButtonTexts; // 카드 버튼 텍스트 배열

    [Header("UI")]
    [SerializeField] private TMP_Text playerNamesText; // 플레이어 이름 표시할 텍스트
    [SerializeField] private TMP_Text winConditionText; // 승리 조건 텍스트
    [SerializeField] private TMP_Text opponentSelectionText; // 상대방 선택 상태 텍스트
    [SerializeField] private Button confirmButton; // 선택 확인 버튼

    [Header("ConstellationCard")]
    [SerializeField] private Sprite[] cardSprites; // 카드 이미지 배열

    [Header("Effects")]
    [SerializeField] private AudioClip summonSound; // 카드 소환 효과음
    [SerializeField] private GameObject summonEffectPrefab; // 카드 소환 이펙트 프리팹
    private AudioSource audioSource; // AudioSource 컴포넌트

    private int[] selectedCards;
    private int? selectedCard = null; // 선택된 카드
    private bool canSelectCard = true; // 카드 선택 가능 여부
    private int playersConfirmed = 0; // 선택을 확인한 플레이어 수

    private void Start()
    {
        // 로컬 플레이어의 선택된 카드를 가져오기
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("selectedCards"))
        {
            selectedCards = (int[])PhotonNetwork.LocalPlayer.CustomProperties["selectedCards"];

            // 카드 배열을 오름차순으로 정렬
            System.Array.Sort(selectedCards);

            // 버튼에 카드 이미지 할당
            for (int i = 0; i < cardButtons.Length; i++)
            {
                if (i < selectedCards.Length)
                {
                    int cardValue = selectedCards[i]; // 캡처를 위한 변수

                    // 버튼 클릭 이벤트 추가
                    cardButtons[i].onClick.AddListener(() => SelectCard(cardValue));

                    // 카드 이미지 설정 (cardValue - 1)
                    if (cardValue - 1 < cardSprites.Length) // 카드 이미지가 존재하는지 확인
                    {
                        cardButtons[i].GetComponent<Image>().sprite = cardSprites[cardValue - 1]; // 카드 이미지 설정
                    }

                    // 카드 숫자 텍스트 설정 (cardValue 그대로 사용)
                    cardButtonTexts[i].text = cardValue.ToString(); // 카드 숫자 텍스트 설정
                }
                else
                {
                    // 선택할 카드가 부족한 경우 버튼 비활성화
                    cardButtons[i].gameObject.SetActive(false);
                }
            }

            // AudioSource 초기화
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = summonSound; // 효과음을 설정합니다.
        }

        // 플레이어 이름 표시
        UpdatePlayerNames();

        // 승리 조건을 PlayerPrefs에서 불러오기
        string winCondition = PlayerPrefs.GetString("WinCondition", "DefaultCondition");
        UpdateWinConditionText(winCondition);

        // 선택 확인 버튼 클릭 이벤트 추가
        confirmButton.onClick.AddListener(ConfirmSelection);
    }

    private void UpdatePlayerNames()
    {
        string otherPlayerNickName = PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.LocalPlayer.NickName
            ? PhotonNetwork.PlayerList[1].NickName
            : PhotonNetwork.PlayerList[0].NickName;

        playerNamesText.text = $"{PhotonNetwork.LocalPlayer.NickName} vs. {otherPlayerNickName}";
    }

    private void UpdateWinConditionText(string winCondition)
    {
        winConditionText.text = $"승리 조건: {winCondition}";
    }

    private void SelectCard(int cardValue)
    {
        if (canSelectCard) // 카드 선택이 가능한 경우
        {
            // 이전에 선택한 카드가 있다면 비활성화
            if (selectedCard.HasValue)
            {
                int previousCardIndex = System.Array.IndexOf(selectedCards, selectedCard.Value);
                if (previousCardIndex >= 0)
                {
                    cardButtons[previousCardIndex].interactable = true; // 이전 카드 버튼 활성화
                }
            }

            // 새 카드 선택 및 비활성화
            selectedCard = cardValue; // 선택된 카드 저장
            winConditionText.text = $"Selected Card: {selectedCard}"; // 승리 조건 텍스트 업데이트
            Debug.Log($"Selected Card: {selectedCard}");

            // 새로 선택된 카드 버튼 비활성화
            int newCardIndex = System.Array.IndexOf(selectedCards, selectedCard.Value);
            if (newCardIndex >= 0)
            {
                cardButtons[newCardIndex].interactable = false; // 새 카드 버튼 비활성화
            }
        }
    }

    private void ConfirmSelection()
    {
        if (selectedCard.HasValue && canSelectCard) // 카드가 선택되었고 선택 가능 상태인 경우
        {
            canSelectCard = false; // 더 이상 카드 선택 불가능하게 설정
            winConditionText.text = $"Confirmed Selection: {selectedCard}";

            // 선택된 카드를 CustomProperties에 저장
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "selectedCard", selectedCard.Value } });

            // 모든 플레이어에게 선택을 전파하는 RPC 호출 추가
            photonView.RPC("NotifyCardSelection", RpcTarget.All, selectedCard.Value);

            // 효과음 재생
            audioSource.Play();

            // 카드 소환 이펙트 생성
            GameObject effect = Instantiate(summonEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 3f); // 3초 후 이펙트 삭제
        }
        else if (!canSelectCard)
        {
            winConditionText.text = "You have already confirmed your selection!";
        }
        else
        {
            winConditionText.text = "Please select a card before confirming!";
        }
    }


    [PunRPC]
    private void NotifyCardSelection(int cardValue)
    {
        // 선택된 카드에 대한 로직 추가
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} selected card: {cardValue}");

        // 상대방에게 기다리라고 알림
        if (!PhotonNetwork.LocalPlayer.IsLocal)
        {
            opponentSelectionText.text = $"{PhotonNetwork.LocalPlayer.NickName} selected a card. Please wait...";
        }

        // 선택한 플레이어 수 증가
        playersConfirmed++;

        // 두 플레이어 모두 선택했는지 확인
        if (playersConfirmed == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            StartCoroutine(LoadCardFightSceneAfterDelay(1f)); // 1초 대기 후 CardFight 씬으로 이동
        }
    }

    private IEnumerator LoadCardFightSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.LoadLevel("01_Scenes/02CardGameVR/FightCutScene");
    }
}
