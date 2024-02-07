namespace Cosmetics
{
    using UnityEngine;
    
    [CreateAssetMenu]
    public class BodyCosmetic : ScriptableObject
    {
        public Sprite texture;
        public BodyCosmeticType type;
        [Space]
        [Header("Offsets")]
        public float x;
        public float y;
    }
}