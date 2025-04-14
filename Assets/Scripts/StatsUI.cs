using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public Player player; // Drag your Player script here
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI killsText;

    void Update()
    {
        if (player != null)
        {
            healthText.text = $"Health: {Mathf.RoundToInt(player.playerHealth)}";
            killsText.text = $"Kills: {player.kills}";
        }
    }
}
