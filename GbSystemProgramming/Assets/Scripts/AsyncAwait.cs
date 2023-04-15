using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncAwait : MonoBehaviour
{
    private readonly CancellationTokenSource cancellationTokenSourceForTask1 = new();
    private readonly CancellationTokenSource cancellationTokenSourceForTask2 = new();

    [SerializeField] private int _task1Duration = 1000;
    [SerializeField] private int _task2Duration = 60;



    private void Start()
    {
        Debug.Log("Tasks started");
        Task.Run(() => { var task1 = Task1(cancellationTokenSourceForTask1.Token); });
        Task.Run(() => { var task2 = Task2(cancellationTokenSourceForTask2.Token); });
    }

    private async Task Task1(CancellationToken cancellationToken)
    {
        await Task.Delay(_task1Duration, cancellationToken);
        Debug.Log("Task1 completed");
    }
    
    private async Task Task2(CancellationToken cancellationToken)
    {
        var frameAwaiter = Task.Run(() => { TargetFrameCountReached(Time.frameCount); }, cancellationToken);
        await frameAwaiter;
        Debug.Log("Task2 completed");
    }

    private IEnumerator TargetFrameCountReached(int startingFrame)
    {
        while (Time.frameCount - startingFrame <= _task2Duration) yield return null;
        yield break;
    }
}
