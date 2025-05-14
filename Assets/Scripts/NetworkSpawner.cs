using UnityEngine;
using Unity.Netcode;

public class NetworkSpawner : MonoBehaviour {
    [SerializeField]
    private GameObject myPrefab;
    
    public void spawn() {
        var instance = Instantiate(myPrefab);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }
}
