using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float lifetime = 2;
    [SerializeField] private float velocity = 50;
    [SerializeField] private float sizePercentage = 1;
    [SerializeField] private List<string> tagsToDetroyOn = new List<string>();

    private Rigidbody rb;
    private Vector3 launchForce = Vector3.right;
    private Vector3 originalSize;
    private float timer = 0;
    private bool hasLaunched = false;


    void Start()
    {
        Debug.Log("2");
        SetOriginalSizeValue();
        UpdateSize();
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        LaunchObject();
    }

    private void FixedUpdate()
    {
        if (!hasLaunched && rb != null)
        {
            LaunchObject();
            hasLaunched = true;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifetime)
        {
            Destroy(this.gameObject);
        }
    }

    private void LaunchObject()
    {
        if (rb == null) return;
        launchForce = transform.forward * velocity;
        rb.AddForce(launchForce, ForceMode.Impulse);
    }

    private void SetOriginalSizeValue()
    {
        originalSize = transform.localScale;
    }

    private void UpdateSize()
    {
        this.transform.localScale = originalSize * sizePercentage;
    }

    void OnTriggerEnter(Collider other)
    {
        if ("wall" == other.gameObject.tag || "Enemy" == other.gameObject.tag)
        {
           Destroy(gameObject);
        }

        
    }
}