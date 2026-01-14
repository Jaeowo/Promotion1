using UnityEngine;

public class SlashColliderAttack : MonoBehaviour
{
    [SerializeField] private BaseStat playerStat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BaseStat>(out var targetStat))
        {
            if (targetStat != playerStat)
            {
                int damage = Calculator.ExecuteAttack(playerStat, targetStat);
                targetStat.TakeDamage(damage);
                UIInitManager.instance.InitDamageText(targetStat.transform.position, damage);
                Debug.Log(damage);
            }
        }
    }
}
