using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPointsManager: IDisposable
{
    private readonly List<SpawnPoint> _spawnPoints = new();

    public SpawnPointsManager(List<Vector3> spawnPoints) 
    {
        foreach (Vector3 t in spawnPoints)
        {
            _spawnPoints.Add(new(t));
        }
    }

    public void Dispose()
    {
        _spawnPoints.Clear();
    }

    public Vector3 GetSpawnPoint()
    {
        var freeSpawnPoints = GetFreeSpawnPoints();

        var spawnPointIndex = Random.Range(0, freeSpawnPoints.Count);
        freeSpawnPoints[spawnPointIndex].SpawnPointUsed = true;

        return freeSpawnPoints[spawnPointIndex].SpawnPointPosition;
    }

    private List<SpawnPoint> GetFreeSpawnPoints()
    {
        List<SpawnPoint> freeSpawnPoints = new();

        foreach (SpawnPoint p in _spawnPoints)
        {
            if (!p.SpawnPointUsed) freeSpawnPoints.Add(p);
        }

        if (freeSpawnPoints.Count == 0)
        {
            foreach (SpawnPoint p in _spawnPoints) p.SpawnPointUsed = false;
            return _spawnPoints;
        }
        else return freeSpawnPoints;
    }
}
