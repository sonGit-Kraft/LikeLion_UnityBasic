using UnityEngine;

public class UIHealthDisplay : MonoBehaviour
{
    void Start()
    {
        // �̺�Ʈ ����
        EventManager.Instance.AddListener("PlayerHealthChanged", OnPlayerHealthChanged);
        EventManager.Instance.AddListener("PlayerDied", OnPlayerDied);
    }

    private void OnDestroy()
    {
        // ��ü�� ������ �� �����ϴ� �Լ�
        EventManager.Instance.RemoveListener("PlayerHealthChanged", OnPlayerHealthChanged);
        EventManager.Instance.RemoveListener("PlayerDied", OnPlayerDied);
    }

    private void OnPlayerHealthChanged(object data)
    {
        int health = (int)data;
        Debug.Log($"UI ������Ʈ: �÷��̾� ü���� {health}�� ����Ǿ����ϴ�.");
    }

    private void OnPlayerDied(object data)
    {
        Debug.Log("UI ������Ʈ: �÷��̾ ����߽��ϴ�!");
    }
}
