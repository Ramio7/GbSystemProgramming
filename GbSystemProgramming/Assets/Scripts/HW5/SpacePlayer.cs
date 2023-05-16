using Unity.Netcode;

public class SpacePlayer : Player
{
    protected override void SpawnCharacter()
    {
        base.SpawnCharacter();
        playerCharacter.name = HW5EntryPoint.NetworkManager.playerName;
        playerCharacter.GetComponent<ShipController>().PlayerName = HW5EntryPoint.NetworkManager.playerName;
    }
    
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        RespawnPlayerServerRpc();
    }

    [ServerRpc]
    private void RespawnPlayerServerRpc()
    {
        gameObject.SetActive(false);
        var spawnPoint = EntryPoint.SpawnPointsManager.GetSpawnPoint();
        gameObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        gameObject.SetActive(true);
    }
}
