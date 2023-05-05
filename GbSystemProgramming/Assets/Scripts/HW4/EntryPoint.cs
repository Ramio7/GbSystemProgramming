using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [field: SerializeField] public LevelView LevelView { get; private set; }
    public static SpawnPointsManager SpawnPointsManager { get; private set; }

    private void Start()
    {
        SpawnPointsManager = new(LevelView.SpawnPoints);
    }
}
