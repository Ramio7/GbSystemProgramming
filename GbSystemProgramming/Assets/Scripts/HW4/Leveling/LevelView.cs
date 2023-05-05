using System.Collections.Generic;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [field: SerializeField] public List<Transform> SpawnPoints { get; private set; } = new();
}
