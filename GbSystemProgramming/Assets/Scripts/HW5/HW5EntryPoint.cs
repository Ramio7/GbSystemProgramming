using UnityEngine;

public class HW5EntryPoint : EntryPoint
{
    [field: SerializeField] public SpaceShipSettings SpaceShipSettings { get; private set; }
    public static SpaceShipSettings ShipSettings { get; private set; }

    protected new void Awake()
    {
        base.Awake();
        ShipSettings = SpaceShipSettings;
    }
}
