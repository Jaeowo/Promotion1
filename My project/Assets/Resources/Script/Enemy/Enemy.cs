using UnityEngine;

public class Enemy
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

    //[Header("BoxCollider")]
    //[SerializeField] protected GameObject attackCollider;
    //[SerializeField] protected Vector2 radius;
    //[SerializeField] protected Vector2 offset;

    public Enemy(float maxHP, float attack, float defence, 
                 float criticalProbability, 
                 float criticalMultiplier)
    {
        this.maxHP = maxHP;
        this.currentHP = maxHP;
        this.attack = attack;
        this.defence = defence;

        this.criticalProbability = criticalProbability;
        this.criticalMultiplier = criticalMultiplier;
    }

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public float ATK => attack;
    public float DEF => defence;
    public float CriticalProbability => criticalProbability;
    public float SkillMultiplier => skillMultiplier;
    public float CriticalMultiplier => criticalMultiplier;

    public void TakeDamage(float damage)
    {
        currentHP = Mathf.Max(currentHP - damage, 0f);
    }

    //#region Getter

    //public float MaxHP => maxHP;
    //public float CurrentHP => currentHP;
    //public float ATK => attack;
    //public float DEF => defence;
    //public float CriticalProbability => criticalProbability;
    //public float SkillMultiplier => skillMultiplier;
    //public float CriticalMultiplier => criticalMultiplier;

    //#endregion

    //#region Setter

    //public void IncreaseCurrentHP(float value)
    //{
    //    currentHP += value;
    //    if (currentHP > maxHP)
    //        currentHP = maxHP;
    //}

    //public void DecreaseCurrentHP(float value)
    //{
    //    currentHP -= value;
    //    if (currentHP < 0f)
    //        currentHP = 0f;
    //}

    //#endregion


}
