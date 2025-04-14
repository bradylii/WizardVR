using UnityEngine;
using TMPro;

public class GolemStats : MonoBehaviour
{
    public Golem golem; // Drag your Player script here
    public TextMeshProUGUI healthText;

    void Update()
    {
        if (golem != null)
        {
            healthText.text = $"Health: {Mathf.RoundToInt(golem.health)}";
        }
    }
}
