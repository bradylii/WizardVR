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
            if (player == null)
                Debug.Log("[Stats] Player couldn't be found is null");
        }
    }

    private void manualSceneInit()
    {
        
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Player>();
            if (player == null)
                Debug.Log("[Stats] -Update()- Player couldn't be found is null"); 
        }

        if (player != null)
        {
            Debug.Log("[Stats] -UPDATE- Setting Health");
            healthText.text = $"Health: {Mathf.RoundToInt(player.playerHealth)}";
            killsText.text = $"Kills: {player.kills}";

            Debug.Log("[Stats] -UPDATE- PlayerHealth: " + player.playerHealth + " | PlayerKills: " + player.kills);
        }
    }

    public void manualResetStats()
    {
         Debug.Log("[Stats] -Manual- Setting Health");
        healthText.text = $"Health: {Mathf.RoundToInt(player.playerHealth)}";
        killsText.text = $"Kills: {player.kills}";
    }


}
