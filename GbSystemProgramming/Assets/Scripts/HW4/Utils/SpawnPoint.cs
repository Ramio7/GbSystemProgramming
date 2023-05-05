using System;
using UnityEngine;

public class SpawnPoint: IDisposable
{
    [field: SerializeField] public Transform SpawnPointTransform { get; private set; }
    [field: SerializeField] public bool SpawnPointUsed { get; set; }

    public SpawnPoint(Transform transform)
    {
        SpawnPointTransform = transform;
        SpawnPointUsed = false;
    }

    public void Dispose()
    {
        SpawnPointTransform = null;
    }
}
