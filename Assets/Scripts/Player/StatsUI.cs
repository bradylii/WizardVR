using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Player player; // Drag your Player script here
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI killsText;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Player>();
            if (player == null)
                Debug.LogError("[Stats] Player couldn't be found is null"); 
        }
    }

    void Update()
    {
        if (player != null)
        {
            healthText.text = $"Health: {Mathf.RoundToInt(player.playerHealth)}";
            killsText.text = $"Kills: {player.kills}";
        }
    }
}
