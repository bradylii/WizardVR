using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.Data;
using TMPro;
using UnityEngine;

public class TextSizeGrow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private float originalFontSize;
    private float changingValue = 1;
    private float maxPercent = 1.2f;
    private float minPercent = 0.8f;
    private float speed = 1.5f;

    private void Awake()
    {
        originalFontSize = text.fontSize;
    }
    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1) / 2;
        float changingValue = Mathf.Lerp(minPercent, maxPercent, t);
        text.fontSize = originalFontSize * changingValue;
    }
}
