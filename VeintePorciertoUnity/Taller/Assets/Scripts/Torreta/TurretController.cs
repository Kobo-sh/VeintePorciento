using UnityEngine;
using System.Collections.Generic;

public class TurretController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TurretModel turretModel;
    [SerializeField] private TurretView turretView;

    [Header("Proyectil")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int poolSize = 6;

    private Queue<Projectile> projectilePool = new Queue<Projectile>();
    private Transform player;
    private float fireTimer = 0f;

    private void Start()
    {
        if (turretModel == null)
            Debug.LogError("[TurretController] Falta asignar TurretModel.");

        if (turretView == null)
            Debug.LogError("[TurretController] Falta asignar TurretView.");

        player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
            Debug.LogError("[TurretController] No se encontró el Player.");

        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Projectile p = Instantiate(projectilePrefab);
            p.gameObject.SetActive(false);
            projectilePool.Enqueue(p);
        }

        Debug.Log($"[TurretController] Pool inicializado con {poolSize} proyectiles.");
    }

    private void Update()
    {
        if (!turretModel.IsAlive || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool playerInRange = distanceToPlayer <= turretModel.DetectionRange;

        if (playerInRange)
        {
            turretModel.CurrentState = TurretModel.TurretState.Attacking;
            turretView.RotateTowards(player.position);

            fireTimer += Time.deltaTime;
            if (fireTimer >= turretModel.FireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
        else
        {
            turretModel.CurrentState = TurretModel.TurretState.Idle;
            fireTimer = 0f;
        }
    }

    private void Shoot()
    {
        if (projectilePool.Count == 0)
        {
            Debug.LogWarning("[TurretController] No hay proyectiles disponibles en el pool.");
            return;
        }

        Projectile p = projectilePool.Dequeue();
        p.transform.position = turretView.FirePoint.position;
        p.transform.rotation = turretView.FirePoint.rotation;
        p.gameObject.SetActive(true);
        p.Initialize(turretModel.Damage, this);

        turretView.PlayMuzzleFlash();

        Debug.Log("[TurretController] Proyectil disparado.");
    }

    public void ReturnProjectile(Projectile p)
    {
        projectilePool.Enqueue(p);
        Debug.Log("[TurretController] Proyectil devuelto al pool.");
    }
}