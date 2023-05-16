using UnityEngine;

public class HW5EntryPoint : EntryPoint
{
    [field: SerializeField] public SpaceShipSettings SpaceShipSettings { get; private set; }
    [field: SerializeField] public SpaceNetworkManager SpaceNetworkManager { get; private set; }
    public static SpaceShipSettings ShipSettings { get; private set; }
    public static SpaceNetworkManager NetworkManager { get; private set; }

    protected new void Awake()
    {
        base.Awake();
        ShipSettings = SpaceShipSettings;
        NetworkManager = SpaceNetworkManager;
    }
}
