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

        Task.Run(() => { var task1 = Task1(_task1Duration, cancellationTokenSourceForTask1.Token); });
        Task.Run(() => { var task2 = Task2(_task2Duration, cancellationTokenSourceForTask2.Token); });
    }

    private void OnApplicationQuit()
    {
        cancellationTokenSourceForTask1?.Dispose();
        cancellationTokenSourceForTask2?.Dispose();
    }

    private async Task Task1(int taskDuration, CancellationToken cancellationToken)
    {
        await Task.Delay(taskDuration, cancellationToken);
        Debug.Log("Task1 completed");
    }
    
    private Task Task2(int taskDuration, CancellationToken cancellationToken)
    {
        for (int i = 0; i < taskDuration; i++)
        {
            if (cancellationToken.IsCancellationRequested) return Task.FromResult(false);
            Debug.Log($"{i + 1} frames passed");
            Task.Yield();
        }
        Debug.Log("Task2 completed");
        return Task.FromResult(true);
    }
}
