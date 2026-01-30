using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private BaseStat attackerStat;

    private void Start()
    {
        if(!attackerStat)
        {
            attackerStat = GetComponentInParent<BaseStat>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BaseStat>(out var targetStat))
        {
            if (targetStat != attackerStat)
            {
                int damage = Calculator.ExecuteAttack(attackerStat, targetStat);
                targetStat.TakeDamage(damage);
                UIInitManager.instance.InitDamageText(targetStat.transform.position, damage);
                Debug.Log(damage);
            }
        }
    }
}
