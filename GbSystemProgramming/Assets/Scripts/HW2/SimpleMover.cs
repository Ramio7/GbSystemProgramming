using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct SimpleMover : IJobParallelFor
{
    [ReadOnly] public NativeArray<Vector3> m_positions;
    [ReadOnly] public NativeArray<Vector3> m_velocities;
    public NativeArray<Vector3> m_finalPositions;

    public void Execute(int index)
    {
        m_finalPositions[index] = m_positions[index] + m_velocities[index];
    }
}
