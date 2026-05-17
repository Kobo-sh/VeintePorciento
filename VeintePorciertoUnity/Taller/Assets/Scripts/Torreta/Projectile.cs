using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float maxLifetime = 3f;

    private float damage;
    private float lifetime;
    private TurretController owner;

    public void Initialize(float damage, TurretController owner)
    {
        this.damage = damage;
        this.owner = owner;
        lifetime = 0f;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime)
            ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
                health.TakeDamage(damage);
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        owner?.ReturnProjectile(this);
    }
}