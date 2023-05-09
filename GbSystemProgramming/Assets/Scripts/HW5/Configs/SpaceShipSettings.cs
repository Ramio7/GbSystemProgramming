using UnityEngine;

[CreateAssetMenu(fileName = "SpaceShipSettings", menuName = "Geekbrains/Settings/Space Ship Settings")]
public class SpaceShipSettings : ScriptableObject
{
    [field: SerializeField, Range(.01f, 0.1f)] public float Acceleration { get; private set; }
    [field: SerializeField, Range(1f, 2000f)] public float ShipSpeed { get; private set; }
    [field: SerializeField, Range(1f, 5f)] public int Faster { get; private set; }
    [field: SerializeField, Range(.01f, 179)] public float NormalFov { get; private set; } = 60;
    [field: SerializeField, Range(.01f, 179)] public float FasterFov { get; private set; } = 30;
    [field: SerializeField, Range(.1f, 5f)] public float ChangeFovSpeed { get; private set; } = .5f;
}