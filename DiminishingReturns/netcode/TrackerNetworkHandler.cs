using System;
using JackEhttack.service;
using Unity.Netcode;

namespace JackEhttack.netcode;

public class TrackerNetworkHandler : NetworkBehaviour
{
    public static TrackerNetworkHandler Instance { get; private set; }

    public override void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            Instance?.gameObject.GetComponent<NetworkObject>().Despawn();
        Instance = this;
        
        base.OnNetworkSpawn();
    }

    [ClientRpc]
    public void TrackerUpdateClientRpc(string text)
    {
        Plugin.Instance.Log.LogInfo($"Received: {text}");
        MoonTracker.Instance.SetText(text);
    }

}