using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private NetworkObject playerPrefab;
    private void Start()
    {
        playerPrefab.InstantiateAndSpawn(networkManager: NetworkManager.Singleton, ownerClientId: NetworkManager.Singleton.LocalClientId, isPlayerObject: true, position: Vector3.zero, rotation: Quaternion.identity );
    }
}
