using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Unit1 : MonoBehaviour
{
    [SerializeField] private List<int> _arrayInput = new();

    private void Start()
    {
        NativeArray<int> jobContainer = new(_arrayInput.Count, Allocator.Persistent);
        jobContainer.CopyFrom(_arrayInput.ToArray());

        var job = new Nulifier()
        {
            m_array = jobContainer,
        };

        job.Schedule().Complete();
        jobContainer.Dispose();
    }
}
