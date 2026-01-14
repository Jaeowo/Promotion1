using TMPro;
using UnityEngine;

public class UIInitManager : MonoBehaviour
{
    [SerializeField] TextMeshPro damageTextPrefab;

    public static UIInitManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void InitDamageText(Vector3 position, float damage)
    {
        TextMeshPro text = Instantiate(damageTextPrefab, position, Quaternion.identity);
        text.text = damage.ToString();
    }
}
