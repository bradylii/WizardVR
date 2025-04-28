using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StatsUI : MonoBehaviour
{
    public Player player; // Drag your Player script here
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI killsText;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (player == null)
        {
            Debug.Log("[Stats] Player is null... trying to find it now");
            player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Player>();
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
