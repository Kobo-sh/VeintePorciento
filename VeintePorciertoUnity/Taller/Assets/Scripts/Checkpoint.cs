using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Materiales")]
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    private MeshRenderer meshRenderer;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Activate()
    {
        IsActive = true;
        if (activeMaterial != null)
            meshRenderer.material = activeMaterial;
        Debug.Log($"[Checkpoint] {gameObject.name} activado.");
    }

    public void Deactivate()
    {
        IsActive = false;
        if (inactiveMaterial != null)
            meshRenderer.material = inactiveMaterial;
        Debug.Log($"[Checkpoint] {gameObject.name} desactivado.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance?.RegisterCheckpoint(this);
    }
}