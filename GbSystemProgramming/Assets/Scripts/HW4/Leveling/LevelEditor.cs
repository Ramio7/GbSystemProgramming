using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelView)), CanEditMultipleObjects]
public class LevelEditor : Editor
{
    private SerializedProperty _spawnPoints;
    private SerializedProperty _starPrefab;
    private SerializedProperty _planetsPrefabs;
    private SerializedProperty _satelites;

    private void OnEnable()
    {
        _spawnPoints = serializedObject.FindProperty("_spawnPoints");
        _starPrefab = serializedObject.FindProperty("_starPrefab");
        _planetsPrefabs = serializedObject.FindProperty("_planetsPrefabs");
        _satelites = serializedObject.FindProperty("_planetSatelitesInPlanetPrefabs");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.BeginVertical();
        EditorGUILayout.PropertyField(_starPrefab);
        EditorGUILayout.PropertyField(_spawnPoints);
        EditorGUILayout.PropertyField(_satelites);
        EditorGUILayout.PropertyField(_planetsPrefabs);
        GUILayout.EndVertical();

        if (!serializedObject.ApplyModifiedProperties() &&
        (Event.current.type != EventType.ExecuteCommand ||
        Event.current.commandName != "UndoRedoPerformed"))
        {
            return;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        if (target is not LevelView level) return;

        var objectsInLevelView = 1 + level.SpawnPoints.Count + level.PlanetsPrefabs.Count + level.PlanetSatelitesInPlanetPrefabs;
        var levelObjectsTransforms = level.GetComponentsInChildren<Transform>();
        var levelObjects = new List<GameObject>();
        if (objectsInLevelView != levelObjectsTransforms.Length - 1)
        {
            var star = Instantiate(level.StarPrefab, level.transform);
            levelObjects.Add(star);

            foreach (var planet in level.PlanetsPrefabs)
            {
                var newPlanet = Instantiate(planet.Prefab, planet.SpawnPosition, Quaternion.identity, level.transform);
                newPlanet.GetComponent<OrbitalMovement>().AroundPoint = star.transform;
                levelObjects.Add(newPlanet);
            }

            for (int i = 0; i < level.SpawnPoints.Count; i++)
            {
                var newSpawnPoint = new GameObject($"SpawnPoint{i}");
                newSpawnPoint.transform.SetPositionAndRotation(level.SpawnPoints[i], Quaternion.identity);
                newSpawnPoint.transform.SetParent(level.transform, true);
                levelObjects.Add(newSpawnPoint);
            }
        }

        for (var i = 0; i < levelObjects.Count; i++)
        {
            var oldPoint = levelObjects[i].transform.position;
            Vector3 pointSnap = Vector3.one * 0.1f;
            var newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, 100000f, pointSnap, Handles.DotHandleCap);
            if (oldPoint == newPoint)
            {
                continue;
            }
            levelObjects[i].transform.position = levelObjects[i].transform.InverseTransformPoint(newPoint);
        }
    }
}
