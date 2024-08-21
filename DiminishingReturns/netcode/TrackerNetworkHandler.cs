using System;
using Unity.Netcode;

namespace JackEhttack.netcode;

public class TrackerNetworkHandler : NetworkBehaviour
{
    public static TrackerNetworkHandler Instance { get; private set; }
    public static event Action<string> TrackerEvent;

    public override void OnNetworkSpawn()
    {
        TrackerEvent = null;

        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            Instance?.gameObject.GetComponent<NetworkObject>().Despawn();
        Instance = this;
        
        base.OnNetworkSpawn();
    }

    [ClientRpc]
    public void TrackerUpdateClientRpc(string text)
    {
        Plugin.Instance.Log.LogInfo($"Received: {text}");
        TrackerEvent?.Invoke(text);
    }

}