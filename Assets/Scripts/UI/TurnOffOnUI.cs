using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnOffOnUI : MonoBehaviour
{
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject retryUI;
    [SerializeField] private GameObject secretTextUI;


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
        victoryUI.SetActive(false); // turn off victory ui

        retryUI.SetActive(!retryUI.activeSelf);
    }

    public void SwitchVictoryUI()
    {
        retryUI.SetActive(false); // turn off retry ui

        victoryUI.SetActive(true);
    }

    public void SwitchSecretTextUI()
    {
        if (secretTextUI != null)
            secretTextUI.SetActive(!secretTextUI.activeSelf);
    }
}
