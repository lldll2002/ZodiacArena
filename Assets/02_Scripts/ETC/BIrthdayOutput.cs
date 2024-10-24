using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ZodiacSign : MonoBehaviour
{
    public TMP_InputField birthdayInput; // 생일을 입력받는 TMP_InputField
    public Button checkButton; // 버튼


    [Header("Constellation Prefab")]
    // 각 별자리에 대응하는 프리팹
    public GameObject aquariusPrefab;  // 물병자리 프리팹
    public GameObject piscesPrefab;    // 물고기자리 프리팹
    public GameObject ariesPrefab;     // 양자리 프리팹
    public GameObject taurusPrefab;    // 황소자리 프리팹
    public GameObject geminiPrefab;    // 쌍둥이자리 프리팹
    public GameObject cancerPrefab;    // 게자리 프리팹
    public GameObject leoPrefab;       // 사자자리 프리팹
    public GameObject virgoPrefab;     // 처녀자리 프리팹
    public GameObject libraPrefab;     // 천칭자리 프리팹
    public GameObject scorpioPrefab;   // 전갈자리 프리팹
    public GameObject sagittariusPrefab; // 사수자리 프리팹
    public GameObject capricornPrefab; // 염소자리 프리팹

    public Transform spawnPoint; // 모델이 나타날 위치(SpawnPoint)
    private string zodiacSign;

    void Start()
    {
        // 버튼 클릭 이벤트 등록
        checkButton.onClick.AddListener(CheckZodiacSign);
    }

    void CheckZodiacSign()
    {
        string input = birthdayInput.text;

        // 입력이 4자리 형식인지 확인
        if (input.Length == 4)
        {
            // 월과 일을 나눠서 정수로 변환
            int month = int.Parse(input.Substring(0, 2));
            int day = int.Parse(input.Substring(2, 2));

            // 별자리 계산
            zodiacSign = GetZodiacSign(month, day);

            // 결과 출력
            Debug.Log($"생일: {input}, 별자리: {zodiacSign}");

            // 해당 별자리에 맞는 프리팹 생성
            InstantiateZodiacModel(zodiacSign);
        }
        else
        {
            Debug.Log("생일을 MMDD 형식으로 입력해주세요.");
        }
    }

    string GetZodiacSign(int month, int day)
    {
        if ((month == 1 && day >= 20) || (month == 2 && day <= 18))
            return "물병자리";
        else if ((month == 2 && day >= 19) || (month == 3 && day <= 20))
            return "물고기자리";
        else if ((month == 3 && day >= 21) || (month == 4 && day <= 19))
            return "양자리";
        else if ((month == 4 && day >= 20) || (month == 5 && day <= 20))
            return "황소자리";
        else if ((month == 5 && day >= 21) || (month == 6 && day <= 20))
            return "쌍둥이자리";
        else if ((month == 6 && day >= 21) || (month == 7 && day <= 22))
            return "게자리";
        else if ((month == 7 && day >= 23) || (month == 8 && day <= 22))
            return "사자자리";
        else if ((month == 8 && day >= 23) || (month == 9 && day <= 22))
            return "처녀자리";
        else if ((month == 9 && day >= 23) || (month == 10 && day <= 22))
            return "천칭자리";
        else if ((month == 10 && day >= 23) || (month == 11 && day <= 21))
            return "전갈자리";
        else if ((month == 11 && day >= 22) || (month == 12 && day <= 21))
            return "사수자리";
        else if ((month == 12 && day >= 22) || (month == 1 && day <= 19))
            return "염소자리";

        return "알 수 없음"; // 잘못된 입력 처리
    }

    // 별자리에 해당하는 프리팹을 Instantiate하는 함수
    void InstantiateZodiacModel(string zodiac)
    {
        GameObject prefabToInstantiate = null;

        switch (zodiac)
        {
            case "물병자리":
                prefabToInstantiate = aquariusPrefab;
                break;
            case "물고기자리":
                prefabToInstantiate = piscesPrefab;
                break;
            case "양자리":
                prefabToInstantiate = ariesPrefab;
                break;
            case "황소자리":
                prefabToInstantiate = taurusPrefab;
                break;
            case "쌍둥이자리":
                prefabToInstantiate = geminiPrefab;
                break;
            case "게자리":
                prefabToInstantiate = cancerPrefab;
                break;
            case "사자자리":
                prefabToInstantiate = leoPrefab;
                break;
            case "처녀자리":
                prefabToInstantiate = virgoPrefab;
                break;
            case "천칭자리":
                prefabToInstantiate = libraPrefab;
                break;
            case "전갈자리":
                prefabToInstantiate = scorpioPrefab;
                break;
            case "사수자리":
                prefabToInstantiate = sagittariusPrefab;
                break;
            case "염소자리":
                prefabToInstantiate = capricornPrefab;
                break;
            default:
                Debug.Log("잘못된 별자리입니다.");
                return;
        }

        if (prefabToInstantiate != null)
        {
            // 모델을 SpawnPoint에서 Instantiate
            //Instantiate(prefabToInstantiate, spawnPoint.position, spawnPoint.rotation);

            // 프리팹을 SpawnPoint에서 Instantiate
            GameObject spawnedObject = Instantiate(prefabToInstantiate, spawnPoint.position, spawnPoint.rotation);

            // 특정 스크립트 컴포넌트를 추가 (여기서는 MyZodiacScript라는 스크립트를 예로 듬)
            spawnedObject.AddComponent<RotateObject>();
        }
    }
}
