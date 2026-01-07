using UnityEngine;

public class PlayerStat : BaseStat
{
    // 공부용 : Action은 미리정의된 Delegate같은 것...
    // delegat는 내가 직접 정의해야 하지만 System.Action은 미리 만들어둔 완제품
    // void 반환형 함수를 담을 때 사용
    public System.Action OnHit;

    // 차후 해야할일 : 플레이어쪽에서 처리하고 있는 death를 action으로 변경해주자..
    public System.Action OnDath;

    public override void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        DecreaseCurrentHP(damage);

        if (currentHP > 0)
        {
            OnHit?.Invoke();
        }

    }

}
