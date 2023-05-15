using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private GameObject playerCharacter;

    private void Start()
    {
        SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        if (!IsServer)
        {
            return;
        }
        playerCharacter = Instantiate(playerPrefab, EntryPoint.SpawnPointsManager.GetSpawnPoint().position, Quaternion.identity, transform);
        playerCharacter.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId, playerCharacter);
    }
}

