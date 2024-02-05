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
}

public enum PlayerToServer : ushort
{
    Location = 0,
}

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    private ushort maxCLientCount = 4;

    public Server server = new Server();
    public Client client = new Client();
    private ushort port;

    public bool playingOnline;

    private void Awake()
    {
        Instance = this;

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        
        DontDestroyOnLoad(gameObject);
    }

    public void StartAsHost()
    {
        playingOnline = true;
        server = new Server();
        client = new Client();
        port = FreeTcpPort();

        server.ClientConnected += (sender, args) =>
        {
            PlayerManager.Instace.SendCurrentPlayers(args.Client.Id);
            PlayerManager.Instace.PlayerJoined(args.Client.Id);
        };
        
        server.ClientDisconnected += (sender, args) =>
        {
            PlayerManager.Instace.PlayerLeft(args.Client.Id);
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
        client = new Client();
        client.Connect(adress);
        SceneManager.LoadScene("Online");

        client.Disconnected += (sender, args) =>
        {
            playingOnline = false;

            SceneManager.LoadScene("MainMenu");
        };
        
        client.Connected += (sender, args) =>
        {
            PlayerManager.Instace.SelfConnected(client.Id);
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
        Debug.Log("receiving new player location");
        PlayerManager.Instace.NewPlayerLocation(message.GetUShort(), message.GetVector2());
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
}
