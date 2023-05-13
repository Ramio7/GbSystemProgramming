using System;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [field: SerializeField] public LevelView LevelView { get; private set; }
    public static SpawnPointsManager SpawnPointsManager { get; private set; }

    public static event Action OnUpdate;
    public static event Action OnFixedUpdate;
    public static event Action OnLateUpdate;
    public static event Action OnGui;

    private void Start()
    {
        SpawnPointsManager = new(LevelView.SpawnPoints);
    }

    private void Update() => OnUpdate?.Invoke();
    private void FixedUpdate() => OnFixedUpdate?.Invoke();
    private void LateUpdate() => OnLateUpdate?.Invoke();
    private void OnGUI() => OnGui?.Invoke();
}
