using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //public int damage;

    public Stat strength;
    public Stat maxHealth;
    public Stat damage;

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.Getvalue();


    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        int totalDamage = damage.Getvalue() + strength.Getvalue();
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        Debug.Log(_damage);

        if (currentHealth < 0)
            Die();
    }

    protected virtual void Die()
    {

    }
}
