namespace Cosmetics
{
    using UnityEngine;
    
    [CreateAssetMenu]
    public class HatCosmetic : ScriptableObject
    {
        public Sprite texture;
        public HatCosmeticType type;
        [Space]
        [Header("Offsets")]
        public float x;
        public float y;
    }
}