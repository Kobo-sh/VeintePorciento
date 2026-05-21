using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float attackRange = 2f;

    [Header("Ataque")]
    [SerializeField] private float damage = 40f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 4f;

    public float DetectionRange => detectionRange;
    public float AttackRange => attackRange;
    public float Damage => damage;
    public float AttackCooldown => attackCooldown;
    public float MoveSpeed => moveSpeed;

    public enum EnemyState { Idle, Chasing, Attacking }
    public EnemyState CurrentState { get; set; }

    private void Awake()
    {
        CurrentState = EnemyState.Idle;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}