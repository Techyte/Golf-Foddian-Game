using Cosmetics;

namespace Multiplayer
{
    using TMPro;
    using UnityEngine;

    public class NetworkPlayer : MonoBehaviour
    {
        public ushort id;
        public TextMeshPro username;
        
        public SkinCosmetic skin;
        public HatCosmetic hat;
        public BodyCosmetic body;
    }

    public class NetworkPlayerData
    {
        public ushort id;
        public Vector2 position;
        public string username;
        
        public SkinCosmetic skin;
        public HatCosmetic hat;
        public BodyCosmetic body;
    }   
}