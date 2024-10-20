using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class NumberPlacer : MonoBehaviour
{
    public GameObject cubePrefab; // 숫자를 표시할 큐브 프리팹
    public List<Transform> positions; // 12개의 고정된 위치를 Transform으로 저장
    private List<int> numbers = new List<int>();

    void Start()
    {
        // 1부터 12까지의 숫자를 리스트에 추가
        for (int i = 1; i <= 12; i++)
        {
            numbers.Add(i);
        }

        // 숫자 배치
        PlaceNumbers();
    }

    void PlaceNumbers()
    {
        // 숫자를 랜덤하게 섞음
        Shuffle(numbers);

        // 고정된 위치에 숫자를 배치
        for (int i = 0; i < positions.Count; i++)
        {
            // 쿼드를 생성하여 해당 위치에 배치
            GameObject newCube = Instantiate(cubePrefab, positions[i].position, Quaternion.identity);

            newCube.name = "Cube" + numbers[i];

            // TextMeshPro 텍스트 설정
            TMP_Text cubeText = newCube.GetComponentInChildren<TMP_Text>();
            if (cubeText != null)
            {
                cubeText.text = numbers[i].ToString(); // 숫자 설정
            }
            else
            {
                Debug.LogError("TextMeshPro 컴포넌트를 찾을 수 없습니다.");
            }
        }
    }

    // 리스트를 랜덤하게 섞는 메서드
    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}