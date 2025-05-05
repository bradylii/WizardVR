using UnityEngine;
using TMPro;

public class GolemStats : MonoBehaviour
{
    [SerializeField] private Golem golem; // Drag your Player script here
    [SerializeField] private TextMeshProUGUI healthText;

    void Update()
    {
        if (golem != null)
        {
            healthText.text = $"Health: {Mathf.RoundToInt(golem.health)}";
        }
    }
}
