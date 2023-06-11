using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] private GameObject _starPrefab;
    [SerializeField] private List<Vector3> _spawnPoints;
    [SerializeField] private List<Planet> _planetsPrefabs;

    public GameObject StarPrefab { get => _starPrefab; set => _starPrefab = value; }
    public List<Vector3> SpawnPoints { get => _spawnPoints; private set => _spawnPoints = value; }
    public List<Planet> PlanetsPrefabs { get => _planetsPrefabs; set => _planetsPrefabs = value; }

    [ServerRpc]
    private void OnEnable()
    {
        var star = Instantiate(StarPrefab, transform);
        foreach (var planet in _planetsPrefabs)
        {
            Instantiate(planet.Prefab, planet.SpawnPosition, Quaternion.identity, transform).GetComponent<OrbitalMovement>().AroundPoint = star.transform;
        }
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            var newSpawnPoint = new GameObject($"SpawnPoint{i}");
            newSpawnPoint.transform.SetPositionAndRotation(_spawnPoints[i], Quaternion.identity);
            newSpawnPoint.transform.SetParent(transform, true);
        }
    }
}