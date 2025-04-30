using UnityEngine;
using UnityEngine.Rendering;

public class CharacterStats : MonoBehaviour
{
    [Header("�⺻���� ����")]
    public Stat strength;     // �� - ���� ������ ����
    public Stat agility;      // ��ø�� - ȸ������ ġ��Ÿ Ȯ�� ����
    public Stat intelligence; // ���� - ���� ������ ����
    public Stat vitality;     // ü�� - �ִ� ü�� ����

    [Header("���ݰ��� ����")]
    public Stat damage;       // �⺻ ������
    public Stat critChance;   // ġ��Ÿ Ȯ��
    public Stat critPower;    // ġ��Ÿ ���ط� ������

    [Header("������ ����")]
    public Stat maxHealth;    // �ִ� ü��
    public Stat armor;        // ���� ����
    public Stat evasion;      // ȸ����
    public Stat magicResistance; // ���� ����

    [Header("���� ����")]
    public Stat fireDamage;    // ȭ�� ������
    public Stat iceDamage;     // ���� ������
    public Stat lightingDamage; // ���� ������

    public bool isIgnited;    // ȭ�� ���� ����
    public bool isChilled;    // ���� ���� ����
    public bool isShocked;    // ���� ���� ����

    private float ignitedTimer; // ȭ�� ���� ���� �ð�
    private float chilledTimer; // ���� ���� ���� �ð�
    private float shockedTimer; // ���� ���� ���� �ð�

    private float ailmentTimer; // �����̻� ���� �ð�

    private float igniteDamageCooldown = 1; // ȭ�� ���� ������ ��Ÿ��
    private float igniteDamageTimer; // ȭ�� ���� ������ Ÿ�̸�

    private int igniteDamage; // ȭ�� ���� ������

    public int currentHealth; // ���� ü��

    public System.Action onHealthChanged;
    protected virtual void Start()
    {
        // �⺻ ġ��Ÿ ���ط� ���� (150%)
        critPower.SetDefaultValue(150);
        // ���� ü���� �ִ� ü������ �ʱ�ȭ
        currentHealth = GetMaxHealthValue();

    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime; // ȭ�� ���� ���� �ð� ����
        chilledTimer -= Time.deltaTime; // ���� ���� ���� �ð� ����
        shockedTimer -= Time.deltaTime; // ���� ���� ���� �ð� ����

        igniteDamageTimer -= Time.deltaTime; // ȭ�� ���� ������ ��Ÿ�� ����

        if (ignitedTimer < 0)
            isIgnited = false; // ȭ�� ���� ����

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if (igniteDamageTimer < 0 && isIgnited) // ȭ�� ���� ������ ��Ÿ���� ������ ȭ�� ������ ��
        {
            Debug.Log("ȭ�� ���� ������ �����" + igniteDamage);
            DecreaseHealth(igniteDamage);

            if (currentHealth < 0)
                Die();

            igniteDamageTimer = igniteDamageCooldown; // ��Ÿ�� �ʱ�ȭ          
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        // ����� ������ ȸ���ߴ��� Ȯ��
        if (TargetCanAvoidAttack(_targetStats))
            return;

        // ���� ������ ��� (�⺻ ������ + ��)
        int totalDamage = damage.GetValue() + strength.GetValue();

        // ġ��Ÿ Ȯ�� �� ���
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        // ����� ���� ����
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        // ������ ����
        _targetStats.TakeDamage(totalDamage);
        // ���� ������ ����
        DoMagicalDamage(_targetStats);
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        // �� �Ӽ��� ���� ������ ���
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        // ��ü ���� ������ ��� (�� �Ӽ� ������ + ����)
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        // ����� ���� ���׷� ����
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        // ������ ����
        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return; // ��� �Ӽ� �������� 0 ������ ��� �����̻� �������� ����
        }

        // �����̻� ���� ���� Ȯ��
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;  // ȭ�� �������� �ٸ� ���������� ���� �� ��ȭ ����
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;     // ���� �������� �ٸ� ���������� ���� �� �ð� ����
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage; // ���� �������� �ٸ� ���������� ���� �� ���� ����

        // ��� �����̻� ������ �������� �ʾ��� ��� ����Ǵ� ����
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            // 50% Ȯ���� ȭ�� �������� 0���� ũ�� ��ȭ ���¸� ����
            if (Random.value < 0.35f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // �����̻� ����
                Debug.Log("��ȭ ���� �����");
                return;
            }

            // 50% Ȯ���� ���� �������� 0���� ũ�� ���� ���¸� ����
            if (Random.value < 0.25f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // �����̻� ����
                Debug.Log("���� ���� �����");
                return;
            }

            // 50% Ȯ���� ���� �������� 0���� ũ�� ���� ���¸� ����
            if (Random.value < 0.15f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // �����̻� ����
                Debug.Log("���� ���� �����");
                return;
            }

            // �� �� ���� �� �ϳ��� �������� ������ ������ ��� �����
        }

        if (canApplyIgnite)
            _targetStats.SetupIngniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f)); // ��ȭ ���� ������ ����

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // �����̻� ����
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        // ���� ���������� ����� ���� ���׷°� ���� ���ʽ��� ����
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        // 0 ���Ϸ� �������� �ʵ��� ����
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        // �̹� �����̻��� ����Ǿ� ������ ����
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        if (_ignite)
        {
            isIgnited = _ignite;  // ��ȭ ���� ����
            ignitedTimer = 2; // ��ȭ ���� ���� �ð� ����
        }

        if (_chill)
        {
            isChilled = _chill; // ���� ���� ����
            chilledTimer = 2; // ���� ���� ���� �ð� ����
        }
        if (_shock)
        {
            isShocked = _shock; // ���� ���� ����
            shockedTimer = 2; // ���� ���� ���� �ð� ����
        }
    }

    public void SetupIngniteDamage(int _damage) => igniteDamage = _damage; // ȭ�� ���� ������ ����

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {

        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f); // ���� ������ ��� ���� 20% ����
        else
            totalDamage -= _targetStats.armor.GetValue(); // ���� ����


        // 0 ���Ϸ� �������� �ʵ��� ����
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        // �� ȸ���� ��� (ȸ�� + ��ø��)
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20; // ���� ������ ��� ȸ���� 20% ����
        }

        // ȸ�� Ȯ�� üũ (0~100 ���� ������ �� ȸ�������� ������ ȸ�� ����)
        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;

        }
        return false;
    }

    public virtual void TakeDamage(int _damage)
    {
        // ���� ü�¿��� ������ ����
        DecreaseHealth(_damage);

        Debug.Log(_damage);

        // ü���� 0 ���ϸ� ��� ó��
        if (currentHealth < 0)
            Die();

        onHealthChanged?.Invoke();
    }

    protected virtual void DecreaseHealth(int _damage)
    {
        currentHealth -= _damage;

        onHealthChanged?.Invoke();
    }

    protected virtual void Die()
    {
        // �ڽ� Ŭ�������� �������̵��Ͽ� ��� ó�� ����
    }

    private bool CanCrit()
    {
        // �� ġ��Ÿ Ȯ�� ��� (ġ��Ÿ Ȯ�� + ��ø��)
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        // ġ��Ÿ Ȯ�� üũ (0~100 ���� ������ �� ġ��Ÿ Ȯ������ �۰ų� ������ ġ��Ÿ ����)
        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        // �� ġ��Ÿ ���� ��� ((ġ��Ÿ �Ŀ� + ��) / 100)
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        // ġ��Ÿ ������ ��� (�⺻ ������ * ġ��Ÿ ����)
        float critDamage = _damage * totalCritPower;

        // ������ �ݿø��Ͽ� ��ȯ
        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
}