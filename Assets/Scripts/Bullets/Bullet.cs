using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float velocity = 500f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private BulletHole bulletHolePrefab;

    [HideInInspector] public Rigidbody rb;
    private IObjectPool<Bullet> bulletPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        Instantiate(bulletHolePrefab, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));
        
        if (gameObject.activeInHierarchy) bulletPool.Release(this);
    }
    private void OnBecameInvisible()
    {
        if (gameObject.activeInHierarchy) bulletPool.Release(this);
    }
    public void Release() => rb.AddForce(transform.forward * velocity, ForceMode.Impulse);
    public void SetPool(IObjectPool<Bullet> pool) => bulletPool = pool;
}
