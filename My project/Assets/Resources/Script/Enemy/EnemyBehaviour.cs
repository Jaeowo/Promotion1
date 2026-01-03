using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, ICombatEntity
{
    [Header("Basic Stat")]
    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;
    [SerializeField] private float attack;
    [SerializeField] private float defence;

    [Header("Probability")]
    [SerializeField] private float criticalProbability;

    [Header("Multiplier")]
    [SerializeField] private float skillMultiplier;
    [SerializeField] private float criticalMultiplier;

    private Enemy enemy;

    private void Awake()
    {
        enemy = new Enemy(maxHP, attack, defence, criticalProbability, criticalMultiplier);
    }

    public float ATK => enemy.ATK;
    public float DEF => enemy.DEF;
    public float CriticalProbability => enemy.CriticalProbability;
    public float CriticalMultiplier => enemy.CriticalMultiplier;
    public void TakeDamage(float damage)
    {
        enemy.TakeDamage(damage);
        if (enemy.CurrentHP <= 0)
        {

        }
    }

}
