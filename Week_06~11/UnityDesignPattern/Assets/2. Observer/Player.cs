using UnityEngine;

public class Player : MonoBehaviour
{
    private int _health = 100;
    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            EventManager.Instance.TriggerEvent("PlayerHealthChanged", _health);

            if (_health <= 0)
            {
                // �÷��̾� ��� �̺�Ʈ �߻�
                EventManager.Instance.TriggerEvent("PlayerDied");
            }
        }
    }
    private void TakeDamage(int damage)
    {
        Health -= damage;
    }
    private void Update()
    {
        // �׽�Ʈ��: �����̽� Ű�� ������ ������ �ޱ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }
}