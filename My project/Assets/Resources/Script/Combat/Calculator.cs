using UnityEngine;


// 공부용 메모 : 상태를 저장할 필요가 없는 도구를 만들 때 Static Class 사용
// new로 객체 생성 불가능
// 결론 -> 단순한 계산은 static class, 플레이어 데이터 저장, 매니저 클래스는 늘 쓰던 Singleton으로
public static class Calculator
{

    // 데미지 편차. 공식을 더 복잡하고 자세하게 하고 싶으면 BaseStat에 항목 추가해보자
    private const float DamageVariance = 0.15f;

   public static int ExecuteAttack(BaseStat attacker, BaseStat defender)
   {
        if (attacker == null || defender == null )
        {
            return 0;
        }


        float minDamage = attacker.ATK * (1f - DamageVariance);
        float maxDamage = attacker.ATK * (1f + DamageVariance);
        float randomDamage = Random.Range(minDamage, maxDamage);

        bool isCritical;

        if (Random.value <= attacker.CriticalProbability)
        {
            isCritical = true;
        }
        else
        {
            isCritical = false;
        }

        if (isCritical)
        {
            randomDamage *= attacker.CriticalMultiplier;
        }

        // 방깍 !!! 공식 방어력 100당 데미지 효율 반감
        float defenseEfficiency = 100f / (100f + defender.DEF);
        float finalDamage = randomDamage * defenseEfficiency;

        // 데미지 보정
        finalDamage = Mathf.Max(finalDamage, 1f);

        int roundDamage = Mathf.RoundToInt(finalDamage);
        return roundDamage;
        //defender.TakeDamage(roundDamage);
   }

}
