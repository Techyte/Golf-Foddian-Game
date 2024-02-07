using Cosmetics;

namespace Multiplayer
{
    using System.Collections.Generic;
    using Riptide;
    using TMPro;
    using Unity.Mathematics;
    using UnityEngine;

    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;

        [SerializeField] private Transform playerStartPosition;
        [SerializeField] private NetworkPlayer proxyPlayer;
        [SerializeField] private NetworkPlayer localPlayer;
        [SerializeField] private TextMeshProUGUI portDisplay;
        
        private Dictionary<ushort, NetworkPlayer> _players = new Dictionary<ushort, NetworkPlayer>();

        public Transform PlayerStartPosition => playerStartPosition;

        public ushort port;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (NetworkManager.Instance.host)
            {
                portDisplay.text = NetworkManager.Instance.server.Port.ToString();
            }

            localPlayer.username.text = PlayerPrefs.GetString("Username", "Guest");
        }

        private void Update()
        {
            SendLocationAsClient();
            
            if (!NetworkManager.Instance.host)
            {
                portDisplay.text = port.ToString();
            }
        }

        private void SendLocationAsClient()
        {
            Message message = Message.Create(MessageSendMode.Unreliable, PlayerToServer.Location);

            message.AddVector2(localPlayer.transform.position);

            NetworkManager.Instance.client.Send(message);
        }

        public void SelfConnected(ushort id)
        {
            Debug.Log("adding self to players");
            localPlayer.id = id;
            localPlayer.username.text = PlayerPrefs.GetString("Username", "Guest");
            localPlayer.hat = CosmeticManager.Instance.GetCurrentHat();
            localPlayer.skin = CosmeticManager.Instance.GetCurrentSkin();
            localPlayer.body = CosmeticManager.Instance.GetCurrentBody();
            _players.Add(id, localPlayer);
            Debug.Log($"Adding {id} to players");
        }

        public void SendCurrentPlayers(ushort id)
        {
            Debug.Log("sending list of players");
            Message message = Message.Create(MessageSendMode.Reliable, ServerToPlayer.CurrentPlayers);

            Debug.Log(_players.Count);
            
            List<NetworkPlayerData> playerData = new List<NetworkPlayerData>();

            foreach (var player in _players.Values)
            {
                NetworkPlayerData data = new NetworkPlayerData();
                
                data.id = player.id;
                data.position = player.transform.position;
                data.username = player.username.text;
                data.hat = player.hat;
                data.skin = player.skin;
                data.body = player.body;
                
                playerData.Add(data);
            }

            message.AddPlayerInfos(playerData.ToArray());

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
            Debug.Log("setting up existing players");

            foreach (var data in datas)
            {
                Debug.Log("setuping up AN existing player");
                Debug.Log(data.id);
                if (!_players.ContainsKey(data.id))
                {
                    Debug.Log(data.username);
                    ushort id = data.id;
                    NetworkPlayer newPlayer = Instantiate(proxyPlayer, data.position, quaternion.identity, transform);
                    newPlayer.username.text = data.username;
                    newPlayer.id = id;
                    newPlayer.hat = data.hat;
                    newPlayer.skin = data.skin;
                    newPlayer.body = data.body;
                    
                    newPlayer.SetCosmetics();
                
                    _players.Add(id, newPlayer);
                    Debug.Log($"Adding {id} to players");
                }
            }

            if (!NetworkManager.Instance.host)
            {
                SelfConnected(NetworkManager.Instance.client.Id);   
            }
        }

        public void HandNewPlayer(Message message)
        {
            ushort id = message.GetUShort();
            NetworkPlayer newPlayer = Instantiate(proxyPlayer, message.GetVector2(), quaternion.identity, transform);
            
            _players.Add(id, newPlayer);
            Debug.Log($"Adding {id} to players");
        }

        public void SetPlayerUsername(ushort id, string username, HatCosmeticType hat, SkinCosmeticType skin, BodyCosmeticType body)
        {
            if (_players.TryGetValue(id, out NetworkPlayer player))
            {
                player.username.text = username;
                player.hat = CosmeticManager.Instance.GetHatCosmetic(hat);
                player.skin = CosmeticManager.Instance.GetSkinCosmetic(skin);
                player.body = CosmeticManager.Instance.GetBodyCosmetic(body);
                player.SetCosmetics();
            }
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
        }
    }
}