using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float lifetime = 2;
    [SerializeField] private float velocity = 50;

    private Rigidbody rb;
    private Vector3 launchForce = Vector3.right;
    private float timer = 0;
    
    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        LaunchObject();
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
        launchForce = Vector3.right * velocity;
        rb.AddForce(launchForce, ForceMode.Impulse);
    }
}