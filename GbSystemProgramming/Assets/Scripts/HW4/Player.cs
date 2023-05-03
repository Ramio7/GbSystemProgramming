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
        playerCharacter = Instantiate(playerPrefab);
        playerCharacter.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.LocalClientId);
    }
}

