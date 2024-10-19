using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2f;
    public Color fadeColor = Color.black; // 기본적으로 검은색 설정
    private Renderer rend;
    private Material fadeMaterial;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Material을 인스턴스화하여 다른 오브젝트에 영향을 주지 않도록 함
        fadeMaterial = rend.material;

        if (fadeOnStart)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        Fade(1f, 0f); // 1: 완전히 불투명 -> 0: 완전히 투명
    }

    public void FadeOut()
    {
        Fade(0f, 1f); // 0: 완전히 투명 -> 1: 완전히 불투명
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0f;

        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            fadeMaterial.color = newColor; // Material의 색상 속성 변경

            timer += Time.deltaTime;
            yield return null;
        }

        // 마지막에 정확히 목표 알파 값으로 설정
        Color finalColor = fadeColor;
        finalColor.a = alphaOut;
        fadeMaterial.color = finalColor;
    }
}
