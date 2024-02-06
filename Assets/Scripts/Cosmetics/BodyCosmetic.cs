namespace Cosmetics
{
    using UnityEngine;
    
    [CreateAssetMenu]
    public class BodyCosmetic : ScriptableObject
    {
        public Sprite texture;
        public BodyCosmeticType type;
    }
}