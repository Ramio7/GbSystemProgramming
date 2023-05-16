using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    protected GameObject playerCharacter;

    private void Start()
    {
        SpawnCharacter();
    }

    protected virtual void SpawnCharacter()
    {
        if (!IsServer)
        {
            return;
        }
        playerCharacter = Instantiate(_playerPrefab, EntryPoint.SpawnPointsManager.GetSpawnPoint().position, Quaternion.identity, transform);
        playerCharacter.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId, playerCharacter);
    }
}

