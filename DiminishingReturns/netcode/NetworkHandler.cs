using System;
using JackEhttack.patch;
using JackEhttack.service;
using Unity.Netcode;

namespace JackEhttack.netcode;

public class NetworkHandler : NetworkBehaviour
{
    public static NetworkHandler Instance { get; private set; }

    public override void OnNetworkSpawn()
    {
        if ((NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer) & Instance != null)
            Instance?.gameObject.GetComponent<NetworkObject>().Despawn();
        Instance = this;
        
        base.OnNetworkSpawn();
    }

    [ClientRpc]
    public void TrackerUpdateClientRpc(string text)
    {
        Plugin.Instance.Log.LogDebug($"Received tracker text from server.");
        MoonTracker.Instance.SetText(text);
    }

    [ClientRpc]
    public void DiscountUpdateClientRpc(float discount)
    {
        Plugin.Instance.Log.LogDebug($"Received moon discount from server.");
        TerminalPatch.UpdatePrices(discount);
    }

}