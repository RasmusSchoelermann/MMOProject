using UnityEngine;
using Steamworks;
using Steamworks.Data;
using Netcode.Transports.Facepunch;
using Unity.Netcode;

public class SteamNetworkManager : MonoBehaviour
{
    public static SteamNetworkManager Instance;

    private FacepunchTransport transport = null;

    public Lobby? CurrentLobby { get; private set; } = null;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        transport = GetComponent<FacepunchTransport>();

        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite += OnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated += OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
    }

    

    private void OnDestroy()
    {
        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite -= OnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated -= OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequested;

        if (NetworkManager.Singleton == null)
        {
            return;
        }

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
    }

    public void StartClient(SteamId id)
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

        transport.targetSteamId = id;

        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client has joined",this);
        }
    }

    public async void StartHost(int maxMembers = 100)
    {
        
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;

        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Started Host");
        }

        CurrentLobby = await SteamMatchmaking.CreateLobbyAsync(maxMembers);
    }

    public void Disconnect()
    {
        CurrentLobby?.Leave();

        if (NetworkManager.Singleton == null)
        {
            return;
        }

        NetworkManager.Singleton.Shutdown();
    }

    private void OnApplicationQuit() => Disconnect();

   
   

    #region Network Callbacks
    private void OnServerStarted()
    {
        Debug.Log("Server started", this);
    }

    private void OnClientDisconnectCallback(ulong clientid)
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        Debug.Log("Client disconnected", this);

    }

    private void OnClientConnectedCallback(ulong clientid)
    {
        Debug.Log("Client connected", this);
    }
    #endregion

    #region Steam Callbacks
    private void OnGameLobbyJoinRequested(Lobby lobby, SteamId id)
    {
        StartClient(lobby.Id);
        Debug.Log("Joined friends lobby", this);
    }

    private void OnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId id)
    {
        Debug.Log("Lobby created", this);
    }

    private void OnLobbyInvite(Friend friend, Lobby lobby)
    {
        Debug.Log("Got an invite", this);
    }

    private void OnLobbyMemberLeave(Lobby lobby1, Friend friend)
    {
        Debug.Log("Friend left lobby", this);
    }

    private void OnLobbyMemberJoined(Lobby lobby, Friend friend)
    {
        Debug.Log("Friedn joined lobby", this);
    }

    private void OnLobbyEntered(Lobby lobby)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            return;
        }
        StartClient(lobby.Id);
    }

    private void OnLobbyCreated(Result result, Lobby lobby)
    {
        if (result != Result.OK)
        {
            Debug.LogError($"Lobby couldn´t be created, {result}",this);
        }

        lobby.SetFriendsOnly();
        lobby.SetData("name","Lobby");
        lobby.SetJoinable(true);

        Debug.Log("Lobby created",this);
    }
    #endregion
}
