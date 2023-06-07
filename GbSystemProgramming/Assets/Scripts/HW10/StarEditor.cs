using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Star)), CanEditMultipleObjects]
public class StarEditor : Editor
{
    private SerializedProperty _center;
    private SerializedProperty _points;
    private SerializedProperty _frequency;

    private void OnEnable()
    {
        _center = serializedObject.FindProperty("_center");
        _points = serializedObject.FindProperty("_points");
        _frequency = serializedObject.FindProperty("_frequency");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(_center);
        EditorGUILayout.PropertyField(_points);
        EditorGUILayout.IntSlider(_frequency, 1, 20);
        EditorGUILayout.EndVertical();
        var totalPoints = _frequency.intValue * _points.arraySize;
        if (totalPoints < 3)
        {
            EditorGUILayout.HelpBox("At least three points are needed.",
            MessageType.Warning);
        }
        else
        {
            EditorGUILayout.HelpBox(totalPoints + " points in total.",
            MessageType.Info);
        }

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
        if (target is not Star star)
        {
            return;
        }
        var starTransform = star.transform;
        var angle = -360f / (_frequency.intValue * star.Points.Length);
        for (var i = 0; i < star.Points.Length; i++)
        {
            var rotation = Quaternion.Euler(0f, 0f, angle * i);
            var oldPoint = starTransform.TransformPoint(rotation * star.Points[i].Position);
            Vector3 pointSnap = Vector3.one * 0.1f;
            var newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, 0.02f, pointSnap, Handles.DotHandleCap);
            if (oldPoint == newPoint)
            {
                continue;
            }
            star.Points[i].Position = Quaternion.Inverse(rotation) * starTransform.InverseTransformPoint(newPoint);
            star.UpdateMesh();
        }
    }
}
