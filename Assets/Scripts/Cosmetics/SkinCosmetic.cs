namespace Cosmetics
{
    using UnityEngine;
    
    [CreateAssetMenu]
    public class SkinCosmetic : ScriptableObject
    {
        public Sprite texture;
        public SkinCosmeticType type;
    }
}