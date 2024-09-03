using HarmonyLib;
using UnityEngine;
using Unity.Netcode;

namespace JackEhttack.patch;

static class NetworkObjectManager
{
    static GameObject networkPrefab;

    [HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.Start))]
    [HarmonyPostfix]
    private static void NetworkPatch(GameNetworkManager __instance)
    {
        if (networkPrefab != null)
        {
            return;
        }

        networkPrefab = (GameObject) Plugin.Instance.MainAssetBundle.LoadAsset("TrackerNetworkHandler");
        networkPrefab.AddComponent<netcode.NetworkHandler>();

        NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);
    }
    
    [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Awake))]
    [HarmonyPostfix] 
    private static void SpawnNetworkHandler(StartOfRound __instance)
    {
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            var networkHandlerHost = Object.Instantiate(networkPrefab, Vector3.zero, Quaternion.identity);
            networkHandlerHost.GetComponent<NetworkObject>().Spawn();
        }
    }
}