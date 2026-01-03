using UnityEngine;

public interface ICombatEntity
{
    float ATK { get; }
    float DEF { get; }

    float CriticalProbability { get; }
    float CriticalMultiplier { get; }

    void TakeDamage(float damage);
}
