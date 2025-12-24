using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    [Header("Basic Stat")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float currentHP = 100f;
    [SerializeField] private float attack = 3f;
    [SerializeField] private float defence = 3f;

    [Header("Probability")]
    [SerializeField] private float criticalProbability = 0.1f;

    [Header("Multiplier")]
    [SerializeField] private float skillMultiplier = 1.2f;
    [SerializeField] private float criticalMultiplier = 1.2f;

    [Header("Bool Set")]
    [SerializeField] private bool isInvincible = false;

    public static PlayerStatManager instance;

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

    public void IncreaseMaxHP(float value)
    {
        maxHP += value;
        currentHP += value;
    }

    public void DecreaseMaxHP(float value)
    {
        maxHP -= value;
        if (maxHP < 0f)
            maxHP = 1f;

        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void IncreaseCurrentHP(float value)
    {
        currentHP += value;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void DecreaseCurrentHP(float value)
    {
        currentHP -= value;
        if (currentHP < 0f)
            currentHP = 0f;
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
