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
        if (_positions.Count != _velocities.Count) throw new Exception("Make same count of positions and velocities");

        NativeArray<Vector3> positionsContainer, velocitiesContainer, finalPositionsContainer;

        InitJobContainers(out positionsContainer, out velocitiesContainer, out finalPositionsContainer);
        InitJob(workerThreads, positionsContainer, velocitiesContainer, finalPositionsContainer);
        ConvertJobResultToList(finalPositionsContainer);
        ResultOutput();
        DisposeJobContainers(positionsContainer, velocitiesContainer, finalPositionsContainer);
    }

    private void InitJobContainers(out NativeArray<Vector3> positionsContainer, out NativeArray<Vector3> velocitiesContainer, out NativeArray<Vector3> finalPositionsContainer)
    {
        positionsContainer = new(_positions.Count, Allocator.Persistent);
        velocitiesContainer = new(_velocities.Count, Allocator.Persistent);
        positionsContainer.CopyFrom(_positions.ToArray());
        velocitiesContainer.CopyFrom(_velocities.ToArray());

        finalPositionsContainer = new(positionsContainer.Length, Allocator.Persistent);
    }

    private void InitJob(int workerThreads, NativeArray<Vector3> positionsContainer, NativeArray<Vector3> velocitiesContainer, NativeArray<Vector3> finalPositionsContainer)
    {
        var job = new SimpleMover
        {
            m_positions = positionsContainer,
            m_velocities = velocitiesContainer,
            m_finalPositions = finalPositionsContainer
        };
        var jobHandle = job.Schedule(positionsContainer.Length, positionsContainer.Length / workerThreads);
        jobHandle.Complete();
    }

    private void ConvertJobResultToList(NativeArray<Vector3> resultContainer)
    {
        var tempArray = new Vector3[resultContainer.Length];
        resultContainer.CopyTo(tempArray);
        foreach (var position in tempArray) _finalPositions.Add(position);
    }

    private void ResultOutput()
    {
        string tempString = "Final positions:\n";
        for (int i = 0; i < _finalPositions.Count; i++) tempString += $"{i + 1}. {_finalPositions[i]} ";
        Debug.Log($"{tempString}");
    }

    private void DisposeJobContainers(NativeArray<Vector3> positionsContainer, NativeArray<Vector3> velocitiesContainer, NativeArray<Vector3> finalPositionsContainer)
    {
        positionsContainer.Dispose();
        velocitiesContainer.Dispose();
        finalPositionsContainer.Dispose();
    }
}
