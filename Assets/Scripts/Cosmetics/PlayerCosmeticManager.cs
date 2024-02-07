using UnityEngine;

namespace Cosmetics
{
    public class PlayerCosmeticManager : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer hatRenderer;
        [SerializeField] private SpriteRenderer skinRenderer;
        [SerializeField] private SpriteRenderer bodyRenderer;

        private Vector3 _originalHatPos;
        private Vector3 _originalBodyPos;

        private void Awake()
        {
            _originalHatPos = hatRenderer.transform.localPosition;
            _originalBodyPos = bodyRenderer.transform.localPosition;
        }

        public void SetHat(HatCosmetic hat)
        {
            if (hat.type == HatCosmeticType.Default)
            {
                Color color = hatRenderer.color;
                color.a = 0;
                hatRenderer.color = color;
            }
            else
            {
                Color color = hatRenderer.color;
                color.a = 255;
                hatRenderer.color = color;
            }
            
            hatRenderer.sprite = hat.texture;

            Vector3 newPos = _originalHatPos;
            newPos.x += hat.x;
            newPos.y += hat.y;
            hatRenderer.transform.localPosition = newPos;
        }

        public void SetSkin(SkinCosmetic skin)
        {
            skinRenderer.sprite = skin.texture;
        }

        public void SetBody(BodyCosmetic body)
        {
            if (body.type == BodyCosmeticType.Default)
            {
                Color color = bodyRenderer.color;
                color.a = 0;
                bodyRenderer.color = color;
            }
            else
            {
                Color color = bodyRenderer.color;
                color.a = 255;
                bodyRenderer.color = color;
            }
            
            bodyRenderer.sprite = body.texture;

            Vector3 newPos = _originalBodyPos;
            newPos.x += body.x;
            newPos.y += body.y;
            bodyRenderer.transform.localPosition = newPos;
        }
    }
}