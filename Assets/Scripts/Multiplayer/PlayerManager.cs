using System;
using System.Collections.Generic;
using Riptide;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instace;

    [SerializeField] private Transform playerStartPosition;
    [SerializeField] private NetworkPlayer proxyPlayer;
    [SerializeField] private Transform localPlayer;
    [SerializeField] private TextMeshProUGUI portDisplay;
    
    private Dictionary<ushort, NetworkPlayer> _players = new Dictionary<ushort, NetworkPlayer>();
    
    private void Awake()
    {
        Instace = this;
    }

    private void Start()
    {
        portDisplay.text = NetworkManager.Instance.server.Port.ToString();
    }

    private void Update()
    {
        SendLocationAsClient();
    }

    public void SendLocationAsClient()
    {
        Message message = Message.Create(MessageSendMode.Unreliable, PlayerToServer.Location);

        message.AddVector2(localPlayer.position);

        NetworkManager.Instance.client.Send(message);
    }

    public void SelfConnected(ushort id)
    {
        _players.Add(id, localPlayer.GetComponent<NetworkPlayer>());
    }

    public void SendCurrentPlayers(ushort id)
    {
        Message message = Message.Create(MessageSendMode.Reliable, ServerToPlayer.CurrentPlayers);

        NetworkPlayerData[] playerData = new NetworkPlayerData[_players.Count];

        for (int i = 0; i < _players.Count; i++)
        {
            if (_players.TryGetValue((ushort)i, out NetworkPlayer player))
            {
                playerData[i].id = player.id;
                playerData[i].position = player.transform.position;
            }
        }

        message.AddPlayerInfos(playerData);

        NetworkManager.Instance.server.Send(message, id);
    }
    
    public void PlayerLeft(ushort id)
    {
        Message message = Message.Create(MessageSendMode.Reliable, ServerToPlayer.PlayerLeft);
        message.AddUShort(id);

        if (_players.TryGetValue(id, out NetworkPlayer player))
        {
            Destroy(player.gameObject);
        }

        _players.Remove(id);
        
        NetworkManager.Instance.server.SendToAll(message, NetworkManager.Instance.client.Id);
    }
    
    public void PlayerJoined(ushort id)
    {
        Message message = Message.Create(MessageSendMode.Reliable, ServerToPlayer.PlayerJoined);
        message.AddUShort(id);

        message.AddVector2(playerStartPosition.position);
        
        NetworkManager.Instance.server.SendToAll(message, id);
    }
    
    public void HandleCurrentPlayers(NetworkPlayerData[] datas)
    {
        for (int i = 0; i < datas.Length; i++)
        {
            ushort id = datas[i].id;
            NetworkPlayer newPlayer = Instantiate(proxyPlayer, datas[i].position, quaternion.identity, transform);
            
            _players.Add(id, newPlayer);
        }
    }

    public void HandNewPlayer(Message message)
    {
        ushort id = message.GetUShort();
        NetworkPlayer newPlayer = Instantiate(proxyPlayer, message.GetVector2(), quaternion.identity, transform);
        
        _players.Add(id, newPlayer);
    }

    public void HandlePlayerLeft(Message message)
    {
        ushort id = message.GetUShort();
        
        if (_players.TryGetValue(id, out NetworkPlayer player))
        {
            Destroy(player.gameObject);
        }

        _players.Remove(id);
    }

    public void NewPlayerLocation(ushort id, Vector2 position)
    {
        if (_players.TryGetValue(id, out NetworkPlayer player))
        {
            player.transform.position = position;
        }
        else
        {
            NetworkPlayer newPlayer = Instantiate(proxyPlayer, position, quaternion.identity, transform);
            _players.Add(id, newPlayer);
        }
    }
}
