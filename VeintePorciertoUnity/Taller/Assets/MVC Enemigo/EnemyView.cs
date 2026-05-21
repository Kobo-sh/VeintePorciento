using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    private readonly string idleParam = "IsIdle";
    private readonly string runParam = "IsRunning";
    private readonly string attackParam = "IsAttacking";

    private int _idleHash;
    private int _runHash;
    private int _attackHash;

    private void Start()
    {
        _idleHash = Animator.StringToHash(idleParam);
        _runHash = Animator.StringToHash(runParam);
        _attackHash = Animator.StringToHash(attackParam);

        if (animator == null)
            Debug.LogError("[EnemyView] Falta asignar Animator.");
    }

    public void SetIdle()
    {
        animator.SetBool(_idleHash, true);
        animator.SetBool(_runHash, false);
    }

    public void SetChasing()
    {
        animator.SetBool(_idleHash, false);
        animator.SetBool(_runHash, true);
    }

    public void SetAttacking()
    {
        animator.SetBool(_idleHash, false);
        animator.SetBool(_runHash, false);
        animator.SetTrigger(_attackHash);
    }
}