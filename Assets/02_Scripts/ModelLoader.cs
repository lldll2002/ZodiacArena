using UnityEngine;
using UnityEngine.UI; // UI를 사용하기 위해 필요
using UnityEngine.SceneManagement;
using TMPro;

public class ModelLoader : MonoBehaviour
{
    public Transform spawnLocation; // 모델을 스폰할 위치
    public TMP_InputField inputField;
    private GameObject currentModel; // 현재 화면에 표시된 모델


    void Start()
    {
        // InputField에 이벤트 리스너 추가 (InputField에 값이 변경될 때 호출)
        inputField.onEndEdit.AddListener(OnInputFieldSubmit);
    }

    // InputField에 숫자가 입력된 후 호출되는 함수
    public void OnInputFieldSubmit(string inputText)
    {
        // 입력된 텍스트를 지우기 (InputField 초기화)
        // TMP_InputField = "";
        Debug.Log("InputField text cleared");

        if (!string.IsNullOrEmpty(inputText))
        {
            if (currentModel != null)
            {
                Destroy(currentModel);
                Debug.Log("Previous model Destroyed.");
            }

            int modelNumber;
            // 입력된 텍스트를 정수로 변환 시도
            if (int.TryParse(inputText, out modelNumber))
            {
                // 모델 그룹 번호에 맞는 프리팹 로드
                string modelGroup = GetModelGroupByRange(modelNumber);
                if (modelGroup != null)
                {
                    // 모델 번호에 맞는 프리팹 로드
                    LoadModel(modelGroup);
                }
                else
                {
                    Debug.LogError("Invaild model number. Please enter a valid number.");
                }
            }
            else
            {
                Debug.LogError("Invalid input! Please enter a valid number.");
            }

        }
        else
        {
            Debug.LogError("Input is Empty.");
        }
    }

    // 모델 번호를 받아서 해당하는 프리팹을 로드하여 보여주는 함수
    public void LoadModel(string modelGroup)
    {
        // 만약 이미 모델이 화면에 있다면 제거
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        // Resuources 폴더에서 프리팹 로드
        // 이거 쓰면 안됨 ........... 용량 많이 차지함
        GameObject modelPrefab = Resources.Load<GameObject>("models/" + modelGroup);

        if (modelPrefab != null)
        {
            // 스폰 위치에 프리팹을 인스턴스화
            currentModel = Instantiate(modelPrefab, spawnLocation.position, spawnLocation.rotation);
            Debug.Log("Model " + modelGroup + " loaded successfully.");
        }
        else
        {
            Debug.LogError("Model " + modelGroup + " not found in Resources folder.");
        }
    }



    // 입력된 숫자에 따라 다른 모델 이름을 반환하는 함수
    private string GetModelGroupByRange(int modelNumber)
    {
        if (modelNumber >= 0120 && modelNumber <= 0218)
        {
            return "modelGroup1"; // 0120부터 0218까지는 model1
        }
        if (modelNumber >= 0219 && modelNumber <= 0320)
        {
            return "modelGroup2"; // 0120부터 0218까지는 model2
        }
        if (modelNumber >= 0321 && modelNumber <= 0420)
        {
            return "modelGroup3"; // 0120부터 0218까지는 model3
        }
        if (modelNumber >= 0421 && modelNumber <= 0520)
        {
            return "modelGroup4"; // 0120부터 0218까지는 model4
        }
        if (modelNumber >= 0521 && modelNumber <= 0621)
        {
            return "modelGroup5"; // 0120부터 0218까지는 model5
        }
        if (modelNumber >= 0622 && modelNumber <= 0722)
        {
            return "modelGroup6"; // 0120부터 0218까지는 model6
        }
        if (modelNumber >= 0723 && modelNumber <= 0822)
        {
            return "modelGroup7"; // 0120부터 0218까지는 model7
        }
        if (modelNumber >= 0823 && modelNumber <= 0922)
        {
            return "modelGroup8"; // 0120부터 0218까지는 model8
        }
        if (modelNumber >= 0923 && modelNumber <= 1021)
        {
            return "modelGroup9"; // 0120부터 0218까지는 model9
        }
        if (modelNumber >= 1022 && modelNumber <= 1121)
        {
            return "modelGroup10"; // 0120부터 0218까지는 model10
        }
        if (modelNumber >= 1122 && modelNumber <= 1221)
        {
            return "modelGroup11"; // 0120부터 0218까지는 model11
        }
        else if (modelNumber >= 1222 && modelNumber <= 0119)
        {
            return "modelGroup12"; // 1001부터 2000까지는 model2
        }
        else
        {
            return null; // 유효하지 않은 숫자일 경우 null 반환
        }
    }
}