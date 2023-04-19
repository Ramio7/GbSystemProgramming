using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using System;
using Unity.Jobs;
using System.Threading;

public class Unit2 : MonoBehaviour
{
    [SerializeField] private List<Vector3> _positions = new();
    [SerializeField] private List<Vector3> _velocities = new();
    [SerializeField] private List<Vector3> _finalPositions = new();
    
    private void Start()
    {
        ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads);
        if (_positions.Count == _velocities.Count) throw new Exception("Make same count of positions and velocities");

        NativeArray<Vector3> _positionsContainer = new(_positions.Count, Allocator.Persistent);
        NativeArray<Vector3> _velocitiesContainer = new(_velocities.Count, Allocator.Persistent);

        _positionsContainer.CopyFrom(_positions.ToArray());
        _velocitiesContainer.CopyFrom(_velocities.ToArray());

        NativeArray<Vector3> _finalPositionsContainer = new(_positionsContainer.Length, Allocator.Persistent);

        var job = new SimpleMover
        {
            m_positions = _positionsContainer,
            m_velocities = _velocitiesContainer,
            m_finalPositions = _finalPositionsContainer
        };
        var jobHandle = job.Schedule(_positionsContainer.Length, _positionsContainer.Length / workerThreads);

        _finalPositionsContainer.CopyTo(_finalPositions.ToArray());

        _positionsContainer.Dispose();
        _velocitiesContainer.Dispose();
        _finalPositionsContainer.Dispose();
    }
}
