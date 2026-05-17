using UnityEngine;

public class TurretView : MonoBehaviour
{
    [Header("Rotación")]
    [SerializeField] private Transform turretHead;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Flash de luz")]
    [SerializeField] private Light muzzleFlash;
    [SerializeField] private float flashDuration = 0.05f;

    [Header("Punto de disparo")]
    [SerializeField] private Transform firePoint;

    public Transform FirePoint => firePoint;

    private void Start()
    {
        if (turretHead == null)
            Debug.LogError("[TurretView] Falta asignar TurretHead.");

        if (muzzleFlash == null)
            Debug.LogError("[TurretView] Falta asignar MuzzleFlash.");

        if (firePoint == null)
            Debug.LogError("[TurretView] Falta asignar FirePoint.");

        if (muzzleFlash != null)
            muzzleFlash.enabled = false;
    }

    public void RotateTowards(Vector3 targetPosition)
    {
        if (turretHead == null) return;

        Vector3 direction = targetPosition - turretHead.position;
        direction.y = 0f;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        turretHead.rotation = Quaternion.Slerp(
            turretHead.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void PlayMuzzleFlash()
    {
        if (muzzleFlash == null) return;
        StartCoroutine(FlashCoroutine());
    }

    private System.Collections.IEnumerator FlashCoroutine()
    {
        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(flashDuration);
        muzzleFlash.enabled = false;
    }
}