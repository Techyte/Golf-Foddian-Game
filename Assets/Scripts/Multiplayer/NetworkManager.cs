using System.Net;
using System.Net.Sockets;
using Riptide;
using Riptide.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ServerToPlayer : ushort
{
    CurrentPlayers = 0,
    PlayerJoined = 1,
    PlayerLeft = 2,
    PlayerLocation = 3,
    Port,
    Username,
}

public enum PlayerToServer : ushort
{
    Location = 0,
    Username = 1,
}

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    private ushort maxCLientCount = 10;

    public Server server = new Server();
    public Client client = new Client();
    private ushort port;

    public bool playingOnline;
    public bool host;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        
        DontDestroyOnLoad(gameObject);
    }

    public void StartAsHost()
    {
        playingOnline = true;
        host = true;
        server = new Server();
        client = new Client();
        port = FreeTcpPort();

        server.ClientConnected += (sender, args) =>
        {
            PlayerManager.Instace.SendCurrentPlayers(args.Client.Id);   
            PlayerManager.Instace.PlayerJoined(args.Client.Id);
            
            Message message = Message.Create(MessageSendMode.Reliable, ServerToPlayer.Port);
            message.AddUShort(port);
            server.Send(message, args.Client.Id);
        };
        
        server.ClientDisconnected += (sender, args) =>
        {
            PlayerManager.Instace.PlayerLeft(args.Client.Id);
        };
        
        client.Connected += (sender, args) =>
        {
            PlayerManager.Instace.SelfConnected(client.Id);
        };
        
        server.Start(port, maxCLientCount);
        client.Connect($"127.0.0.1:{port}");
        SceneManager.LoadScene("Online");
    }

    private void FixedUpdate()
    {
        server.Update();
        client.Update();
    }

    public void StartAsClient(string adress)
    {
        playingOnline = true;
        host = false;
        client = new Client();
        client.Connect(adress);
        SceneManager.LoadScene("Online");

        client.Disconnected += (sender, args) =>
        {
            playingOnline = false;

            SceneManager.LoadScene("MainMenu");
            server = new Server();
            client = new Client();
        };
        
        client.Connected += (sender, args) =>
        {
            Message message = Message.Create(MessageSendMode.Reliable, PlayerToServer.Username);
            message.AddString(PlayerPrefs.GetString("Username", "Guest"));
            client.Send(message);
        };
    }

    private void OnApplicationQuit()
    {
        server.Stop();
        client.Disconnect();
    }

    static ushort FreeTcpPort()
    {
        TcpListener l = new TcpListener(IPAddress.Loopback, 0);
        l.Start();
        ushort port = (ushort)((IPEndPoint)l.LocalEndpoint).Port;
        l.Stop();
        return port;
    }

    [MessageHandler((ushort)ServerToPlayer.CurrentPlayers)]
    private static void CurrentPlayers(Message message)
    {
        PlayerManager.Instace.HandleCurrentPlayers(message.GetPlayerInfos());
    }

    [MessageHandler((ushort)ServerToPlayer.PlayerJoined)]
    private static void PlayerJoined(Message message)
    {
        PlayerManager.Instace.HandNewPlayer(message);
    }

    [MessageHandler((ushort)ServerToPlayer.PlayerLeft)]
    private static void PlayerLeft(Message message)
    {
        PlayerManager.Instace.HandlePlayerLeft(message);
    }

    [MessageHandler((ushort)ServerToPlayer.PlayerLocation)]
    private static void NewPlayerLocation(Message message)
    {
        PlayerManager.Instace.NewPlayerLocation(message.GetUShort(), message.GetVector2());
    }

    [MessageHandler((ushort)ServerToPlayer.Port)]
    private static void Port(Message message)
    {
        PlayerManager.Instace.port = message.GetUShort();
    }

    [MessageHandler((ushort)ServerToPlayer.Username)]
    private static void Username(Message message)
    {
        PlayerManager.Instace.SetPlayerUsername(message.GetUShort(), message.GetString());
    }

    [MessageHandler((ushort)PlayerToServer.Location)]
    private static void PlayerLocation(ushort fromPlayerId, Message message)
    {
        Vector2 position = message.GetVector2();
        
        Message forwardMessage = Message.Create(MessageSendMode.Unreliable, ServerToPlayer.PlayerLocation);
        forwardMessage.AddUShort(fromPlayerId);
        forwardMessage.AddVector2(position);
        
        Instance.server.SendToAll(forwardMessage, fromPlayerId);
    }

    [MessageHandler((ushort)PlayerToServer.Username)]
    private static void PlayerUsername(ushort fromPlayerId, Message message)
    {
        string username = message.GetString();
        
        PlayerManager.Instace.SetPlayerUsername(fromPlayerId, username);
        
        Message forwardMessage = Message.Create(MessageSendMode.Unreliable, ServerToPlayer.Username);
        forwardMessage.AddUShort(fromPlayerId);
        forwardMessage.AddString(username);
        
        Instance.server.SendToAll(forwardMessage, fromPlayerId);
    }
}
