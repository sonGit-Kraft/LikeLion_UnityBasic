using System.Collections;
using TMPro;
using UnityEngine;

public class TMPColor : MonoBehaviour
{
    // 색상 전환에 걸리는 시간
    [SerializeField]
    float lerpTime = 0.1f;

    // 텍스트 컴포넌트
    TextMeshProUGUI textBossWarning;

    // Awake 메서드: 컴포넌트 초기화
    private void Awake()
    {
        textBossWarning = GetComponent<TextMeshProUGUI>();    
    }

    // OnEnable 메서드: 오브젝트가 활성화될 때 호출
    private void OnEnable()
    {
        StartCoroutine("ColorLerpLoop");
    }

    // 색상 전환 루프 코루틴
    IEnumerator ColorLerpLoop()
    {
        while(true)
        {
            yield return StartCoroutine(ColorLerp(Color.white, Color.red));
            yield return StartCoroutine(ColorLerp(Color.red, Color.white));
        }
    }

    IEnumerator ColorLerp(Color start, Color end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;
        
        while(percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / lerpTime;
            textBossWarning.color = Color.Lerp(start, end, percent);
            yield return null;
        }
    }
}
