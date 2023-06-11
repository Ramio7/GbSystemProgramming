using UnityEngine;

public class SpawnPoint
{
    [field: SerializeField] public Vector3 SpawnPointPosition { get; private set; }
    [field: SerializeField] public bool SpawnPointUsed { get; set; }

    public SpawnPoint(Vector3 position)
    {
        SpawnPointPosition = position;
        SpawnPointUsed = false;
    }
}
