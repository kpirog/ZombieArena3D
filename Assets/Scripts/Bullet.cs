using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float velocity = 500f;
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    public void Release() => rb.AddForce(transform.forward * velocity, ForceMode.Impulse);
}
