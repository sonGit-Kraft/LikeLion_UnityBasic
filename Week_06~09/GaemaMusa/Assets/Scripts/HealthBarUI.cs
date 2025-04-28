using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity; // Entity 컴포넌트 참조
    private RectTransform myTransform; // RectTransform 컴포넌트 참조
    private Slider slider; // 슬라이더 컴포넌트 참조
    private CharacterStats myStats;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>(); // RectTransform 컴포넌트 가져오기
        entity = GetComponentInParent<Entity>(); // 부모 오브젝트에서 Entity 컴포넌트 가져오기
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();
        entity.onFlipped += FlipUI; // Entity의 onFlipped 델리게이트에 FlipUI 메서드 등록
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void Update()
    {
    }

    public void FlipUI() => myTransform.Rotate(0, 180, 0); // UI를 180도 회전시켜서 반대 방향으로 표시

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI; // Entity의 onFlipped 델리게이트에서 FlipUI 메서드 등록 해제
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }
}