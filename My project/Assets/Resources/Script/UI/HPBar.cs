using System;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    //[SerializeField] Vector3 pivot;
    [SerializeField] Transform fill;

    private GameObject parent;
    BaseStat targetStat;

    private void Start()
    {
        parent = transform.parent.gameObject;

        if (parent.TryGetComponent<BaseStat>(out targetStat))
        {
            //targetStat.MaxHP
        }
        targetStat.OnHPChanged += UpdateHPBar;
    }

    private void UpdateHPBar(float currentHP, float maxHP)
    {
        float ratio = Mathf.Clamp01(currentHP / maxHP);

        Vector3 scale = fill.localScale;
        scale.x = ratio;
        fill.localScale = scale;
    }

    private void Update()
    {
     
    }

}
