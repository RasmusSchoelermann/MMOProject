using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
public class DebugNetworkUiManager : MonoBehaviour
{
    [SerializeField]
    private Toggle steamToggle;

    [SerializeField]
    private NetworkTransport steamTransport;

    [SerializeField]
    private NetworkTransport unityTransport;

    [SerializeField]
    private GameObject canvas;
    public void Host()
    {
        if (steamToggle.isOn)
        {
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = steamTransport;
            SteamNetworkManager.Instance.StartHost(5);
        }
        else
        {
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = unityTransport;
            NetworkManager.Singleton.StartHost();
        }
    }

    public void Join()
    {
        if (!steamToggle.isOn)
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
