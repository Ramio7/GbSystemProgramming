using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelView)), CanEditMultipleObjects]
public class LevelEditor : Editor
{
    private SerializedProperty _spawnPoints;
    private SerializedProperty _starPrefab;
    private SerializedProperty _planetsPrefabs;

    private List<GameObject> _sceneObjects = new();

    private void OnEnable()
    {
        _spawnPoints = serializedObject.FindProperty("_spawnPoints");
        _starPrefab = serializedObject.FindProperty("_starPrefab");
        _planetsPrefabs = serializedObject.FindProperty("_planetsPrefabs");

        if (target is not LevelView level) return;

        var star = Instantiate(level.StarPrefab, level.transform);
        _sceneObjects.Add(star);

        foreach (var planet in level.PlanetsPrefabs)
        {
            var newPlanet = Instantiate(planet.Prefab, planet.SpawnPosition, Quaternion.identity, level.transform);
            newPlanet.GetComponent<OrbitalMovement>().AroundPoint = star.transform;
            _sceneObjects.Add(newPlanet);
        }

        for (int i = 0; i < level.SpawnPoints.Count; i++)
        {
            var newSpawnPoint = new GameObject($"SpawnPoint{i}");
            newSpawnPoint.transform.SetPositionAndRotation(level.SpawnPoints[i], Quaternion.identity);
            newSpawnPoint.transform.SetParent(level.transform, true);
            _sceneObjects.Add(newSpawnPoint);
        }
    }

    private void OnDisable()
    {
        foreach (var levelObject in _sceneObjects) { DestroyImmediate(levelObject); }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.BeginVertical();
        EditorGUILayout.PropertyField(_starPrefab);
        EditorGUILayout.PropertyField(_spawnPoints);
        EditorGUILayout.PropertyField(_planetsPrefabs);
        GUILayout.EndVertical();

        if (!serializedObject.ApplyModifiedProperties() &&
        (Event.current.type != EventType.ExecuteCommand ||
        Event.current.commandName != "UndoRedoPerformed"))
        {
            return;
        }

        serializedObject.ApplyModifiedProperties();

        foreach (var obj in targets)
        {
            if (obj is Star star)
            {
                star.UpdateMesh();
            }
        }
    }

    private void OnSceneGUI()
    {
        if (target is not LevelView level) return;

        
    }
}
