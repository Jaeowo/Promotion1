using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager instance;

    private void Awake()
    {
        // Singleton Setting
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Attack(ICombatEntity attacker, ICombatEntity defender)
    {
        float damage = CalculateDamage(attacker, defender);
        defender.TakeDamage(damage);
    }

    // 데미지 계산후 어택하도록...
    // 방어력 공격력 공식 조금 수정할까 고민중
    private float CalculateDamage(ICombatEntity attacker, ICombatEntity defender)
    {
        float minDamage = attacker.ATK * 0.8f;
        float maxDamage = attacker.ATK * 1.2f;
        float damage = Random.Range(minDamage, maxDamage);

        damage -= defender.DEF;
        damage = Mathf.Max(damage, 1f);

        float criticalValue = Random.value;
        float CriticalProbability = Mathf.Clamp01(attacker.CriticalProbability);
        if (criticalValue <= CriticalProbability)
        {
            damage *= attacker.CriticalMultiplier;
        }

        // 최종수치 디버깅용..
        float finalDamage = Mathf.Floor(damage);

        return finalDamage;
    }

}
