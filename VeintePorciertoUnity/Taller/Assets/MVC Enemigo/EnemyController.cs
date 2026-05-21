using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private EnemyModel enemyModel;
    [SerializeField] private EnemyView enemyView;

    private NavMeshAgent agent;
    private Transform player;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
            Debug.LogError("[EnemyController] Falta NavMeshAgent.");

        if (enemyModel == null)
            Debug.LogError("[EnemyController] Falta EnemyModel.");

        if (enemyView == null)
            Debug.LogError("[EnemyController] Falta EnemyView.");

        player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
            Debug.LogError("[EnemyController] No se encontró el Player.");

        agent.speed = enemyModel.MoveSpeed;
    }

    private void Update()
    {
        if (player == null || enemyModel == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= enemyModel.AttackRange)
        {
            enemyModel.CurrentState = EnemyModel.EnemyState.Attacking;
            agent.isStopped = true;

            attackTimer += Time.deltaTime;
            if (attackTimer >= enemyModel.AttackCooldown)
            {
                Attack();
                attackTimer = 0f;
            }
        }
        else if (distanceToPlayer <= enemyModel.DetectionRange)
        {
            enemyModel.CurrentState = EnemyModel.EnemyState.Chasing;
            agent.isStopped = false;
            agent.SetDestination(player.position);
            enemyView.SetChasing();
            attackTimer = 0f;
        }
        else
        {
            enemyModel.CurrentState = EnemyModel.EnemyState.Idle;
            agent.isStopped = true;
            enemyView.SetIdle();
            attackTimer = 0f;
        }
    }

    private void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= enemyModel.AttackRange)
        {
            enemyView.SetAttacking();

            HealthSystem health = player.GetComponent<HealthSystem>();
            if (health != null)
                health.TakeDamage(enemyModel.Damage);

            Debug.Log($"[EnemyController] Atacó al jugador por {enemyModel.Damage} de daño.");
        }
    }
}