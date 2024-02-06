namespace Multiplayer
{
    using Riptide;
    using UnityEngine;

    public static class MessageExtensions
    {
        public static Message AddVector2(this Message message, Vector2 value) => Add(message, value);

        public static Message Add(this Message message, Vector2 value)
        {
            message.AddFloat(value.x);
            message.AddFloat(value.y);
            return message;
        }

        public static Vector2 GetVector2(this Message message)
        {
            return new Vector2(message.GetFloat(), message.GetFloat());
        }
        
        public static Message AddPlayerInfos(this Message message, NetworkPlayerData[] value) => Add(message, value);
             
        public static Message Add(this Message message, NetworkPlayerData[] value)
        {
            message.AddUShort((ushort)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                Debug.Log($"Adding value: {value[i].id} at: {i}");
                Debug.Log($"Adding value: {value[i].position} at: {i}");
                Debug.Log($"Adding value: {value[i].username} at: {i}");
                message.AddUShort(value[i].id);
                message.AddVector2(value[i].position);
                message.AddString(value[i].username);
            }
                 
            return message;
        }

        public static NetworkPlayerData[] GetPlayerInfos(this Message message)
        {
            ushort length = message.GetUShort();
            
            NetworkPlayerData[] playerData = new NetworkPlayerData[length];

            for (int i = 0; i < length; i++)
            {
                playerData[0] = new NetworkPlayerData();
                playerData[i].id = message.GetUShort();
                playerData[i].position = message.GetVector2();
                playerData[i].username = message.GetString();
            }

            return playerData;
        }
    }
}