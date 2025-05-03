using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnOffOnUI : MonoBehaviour
{
    public GameObject victoryUI;
    public GameObject retryUI;
    public GameObject secretTextUI;
    [SerializeField] private bool victoryActive = false;
    [SerializeField] private bool retryActive = false;
    [SerializeField] private bool secretTextActive = false;


    private void Start()
    {
        Debug.Log("[TurnOffOn] Scene Loaded");

        if (victoryUI == null)
            retryUI = GameObject.FindGameObjectWithTag("UI").transform.Find("RetryScreen")?.gameObject;
        victoryUI.SetActive(false);

        if (victoryUI == null)
            retryUI = GameObject.FindGameObjectWithTag("UI").transform.Find("VictoryScreen")?.gameObject;
        retryUI.SetActive(false);

        if (secretTextUI == null)
            secretTextUI = GameObject.FindGameObjectWithTag("UI").transform.Find("SecretText")?.gameObject;
        secretTextUI.SetActive(false);
    }


    public void SwitchRetryUI()
    {
        retryUI.SetActive(!retryUI.activeSelf);
    }

    public void SwitchVictoryUI()
    {
        victoryUI.SetActive(!victoryUI.activeSelf);
    }

    public void SwitchSecretTextUI()
    {
        if (secretTextUI != null)
            secretTextUI.SetActive(!secretTextUI.activeSelf);
    }
}
