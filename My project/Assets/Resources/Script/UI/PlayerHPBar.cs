using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Image fill;

    private BaseStat targetStat;

    private void Start()
    {
        if(!targetStat)
        {
            targetStat = player.GetComponent<BaseStat>();
        }

        targetStat.OnHPChanged += UpdateHPBar;
        UpdateHPBar(targetStat.CurrentHP, targetStat.MaxHP);
        ShowHP();
    }

    private void UpdateHPBar(float currentHP, float maxHP)
    {
        float ratio = Mathf.Clamp01(currentHP / maxHP);
        fill.fillAmount = ratio;
        ShowHP();
    }

    private void ShowHP()
    {
        HPText.text = targetStat.CurrentHP.ToString() + "/" + targetStat.MaxHP.ToString();
    }

}
