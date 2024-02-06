namespace Cosmetics
{
    using UnityEngine;
    
    [CreateAssetMenu]
    public class HatCosmetic : ScriptableObject
    {
        public Sprite texture;
        public HatCosmeticType type;
        public float yOffset;
    }
}