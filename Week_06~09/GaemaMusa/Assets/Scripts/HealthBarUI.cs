using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity; // Entity ������Ʈ ����
    private RectTransform myTransform; // RectTransform ������Ʈ ����
    private Slider slider; // �����̴� ������Ʈ ����
    private CharacterStats myStats;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>(); // RectTransform ������Ʈ ��������
        entity = GetComponentInParent<Entity>(); // �θ� ������Ʈ���� Entity ������Ʈ ��������
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();
        entity.onFlipped += FlipUI; // Entity�� onFlipped ��������Ʈ�� FlipUI �޼��� ���
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void Update()
    {
    }

    public void FlipUI() => myTransform.Rotate(0, 180, 0); // UI�� 180�� ȸ�����Ѽ� �ݴ� �������� ǥ��

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI; // Entity�� onFlipped ��������Ʈ���� FlipUI �޼��� ��� ����
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }
}