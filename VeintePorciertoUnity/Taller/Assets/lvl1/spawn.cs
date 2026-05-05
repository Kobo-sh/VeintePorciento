using UnityEngine;

public class spawn : MonoBehaviour
{

    [SerializeField] public Transform spawnTransform;
    [SerializeField] public Transform PlayerPos;

    public void Reespawn()
    {
        PlayerPos.position = spawnTransform.position;
    }

