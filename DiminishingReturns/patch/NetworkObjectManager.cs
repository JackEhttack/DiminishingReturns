using UnityEngine;
using Unity.Netcode;

namespace JackEhttack.patch;

public static class NetworkObjectManager
{
    static GameObject networkPrefab;

    public static void ApplyPatches()
    {
        On.GameNetworkManager.Start += NetworkPatch;
        On.StartOfRound.Awake += SpawnNetworkHandler;
    }

    private static void NetworkPatch(On.GameNetworkManager.orig_Start orig, GameNetworkManager self)
    {
        orig(self);

        if (networkPrefab != null)
        {
            return;
        }

        networkPrefab = (GameObject) Plugin.Instance.MainAssetBundle.LoadAsset("TrackerNetworkHandler");
        networkPrefab.AddComponent<netcode.NetworkHandler>();

        NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);
    }
    
    private static void SpawnNetworkHandler(On.StartOfRound.orig_Awake orig, StartOfRound self)
    {
        orig(self);
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            var networkHandlerHost = Object.Instantiate(networkPrefab, Vector3.zero, Quaternion.identity);
            networkHandlerHost.GetComponent<NetworkObject>().Spawn();
        }
    }
}