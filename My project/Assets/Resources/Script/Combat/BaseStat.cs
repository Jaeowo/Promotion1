using System;
using UnityEngine;

public abstract class BaseStat : MonoBehaviour
{
    [Header("Basic Stat")]
    [SerializeField] protected int maxHP = 100;
    [SerializeField] protected int currentHP = 100;
    [SerializeField] protected float attack = 3f;
    [SerializeField] protected float defence = 3f;

    [Header("Probability")]
    [SerializeField] protected float criticalProbability = 0.1f;

    [Header("Multiplier")]
    [SerializeField] protected float skillMultiplier = 1.2f;
    [SerializeField] protected float criticalMultiplier = 1.2f;

    [Header("Bool Set")]
    [SerializeField] protected bool isInvincible = false;

    // Event
    public event Action<float, float> OnHPChanged;

    protected virtual void Awake()
    {
        SetDefaultStats();
        currentHP = maxHP;
    }

    // 변경하고싶은 스탯은 이쪽에서 구현하기
    // 굳이 이쪽 변경하지 않더라도 Inspector상에서 변경가능 (코드상으로 변경하고 싶을 때만 사용하자 !)
    protected virtual void SetDefaultStats() { }

    // 죽음처리는 TakeDamage함수 안에서 보다 각각의 클래스 내부에서 해주는게 좋을듯
    // Controller안에서 Getter로 받도록... 
    public virtual void TakeDamage(int damage)
    {
        if(isInvincible)
        {
            return;
        }

        DecreaseCurrentHP(damage);
    }

    #region Getter

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public float ATK => attack;
    public float DEF => defence;
    public float CriticalProbability => criticalProbability;
    public float SkillMultiplier => skillMultiplier;
    public float CriticalMultiplier => criticalMultiplier;
    public bool IsInvincible => isInvincible;

    #endregion

    #region BasicStat Setter

    public void IncreaseMaxHP(int value)
    {
        maxHP += value;
        currentHP += value;

        OnHPChanged?.Invoke(CurrentHP, MaxHP);
    }

    public void DecreaseMaxHP(int value)
    {
        maxHP -= value;
        if (maxHP < 0)
            maxHP = 1;

        if (currentHP > maxHP)
            currentHP = maxHP;

        OnHPChanged?.Invoke(CurrentHP, MaxHP);
    }

    public void IncreaseCurrentHP(int value)
    {
        currentHP += value;
        if (currentHP > maxHP)
            currentHP = maxHP;

        OnHPChanged?.Invoke(CurrentHP, MaxHP);
    }

    public void DecreaseCurrentHP(int value)
    {
        currentHP -= value;
        if (currentHP < 0)
            currentHP = 0;

        OnHPChanged?.Invoke(CurrentHP, MaxHP);
    }

    public void IncreaseATK(float value)
    {
        attack += value;
    }

    public void DecreaseATK(float value)
    {
        attack -= value;
    }

    public void IncreaseDEF(float value)
    {
        defence += value;
    }

    public void DecreaseDEF(float value)
    {
        defence -= value;
    }

    #endregion

    #region Probability Setter

    public void IncreaseCriticalProbability(float value)
    {
        criticalProbability += value;

        if (criticalProbability > 1.0f)
        {
            criticalProbability = 1.0f;
        }
    }

    public void DecreaseCriticalProbability(float value)
    {
        criticalProbability -= value;

        if (criticalProbability < 0.0f)
        {
            criticalProbability = 0.0f;
        }
    }
    #endregion

    #region Multiplier Setter
    public void IncreaseSkillMultiplier(float value)
    {
        skillMultiplier += value;
    }

    public void DecreaseSkillMultiplier(float value)
    {
        skillMultiplier -= value;
    }

    public void IncreaseCriticalMultiplier(float value)
    {
        criticalMultiplier += value;
    }

    public void DecreaseCriticalMultiplier(float value)
    {
        criticalMultiplier -= value;
    }
    #endregion

    #region Bool Setter

    public void ChangeInvencible(bool value)
    {
        this.isInvincible = value;
    }

    #endregion

}
