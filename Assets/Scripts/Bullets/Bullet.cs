using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float velocity = 500f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private BulletHole bulletHolePrefab;

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
        ContactPoint contact = collision.GetContact(0);
        Instantiate(bulletHolePrefab, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));

        Destroy(gameObject);
    }
    public void Release() => rb.AddForce(transform.forward * velocity, ForceMode.Impulse);
}
