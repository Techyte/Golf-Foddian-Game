using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cosmetics
{
    public class CosmeticManager : MonoBehaviour
    {
        public static CosmeticManager Instance;
        
        private Dictionary<HatCosmeticType, HatCosmetic> _hatCosmetics = new Dictionary<HatCosmeticType, HatCosmetic>();
        private Dictionary<SkinCosmeticType, SkinCosmetic> _skinCosmetics = new Dictionary<SkinCosmeticType, SkinCosmetic>();
        private Dictionary<BodyCosmeticType, BodyCosmetic> _bodyCosmetics = new Dictionary<BodyCosmeticType, BodyCosmetic>();

        private List<HatCosmetic> _hatSelection = new List<HatCosmetic>();
        private List<SkinCosmetic> _skinSelection = new List<SkinCosmetic>();
        private List<BodyCosmetic> _bodySelection = new List<BodyCosmetic>();

        private int _currentHatSelection = 0;
        private int _currentSkinSelection = 0;
        private int _currentBodySelection = 0;
        
        public int CurrentHatSelection => _currentHatSelection;
        public int CurrentSkinSelection => _currentSkinSelection;
        public int CurrentBodySelection => _currentBodySelection;

        [SerializeField] private Transform originalHatDisplayPos;

        [SerializeField] private Image hatDisplay;
        [SerializeField] private Image bodyDisplay;
        [SerializeField] private Image skinDisplay;

        [Space] [SerializeField] private float hatSizeMultiplier;
        [SerializeField] private float skinSizeMultiplier;
        [SerializeField] private float bodySizeMultiplier;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
            }
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
            
            HatCosmetic[] hatCosmetics = Resources.LoadAll<HatCosmetic>("Cosmetics/Hat");
            SkinCosmetic[] skinCosmetics = Resources.LoadAll<SkinCosmetic>("Cosmetics/Skin");
            BodyCosmetic[] bodyCosmetics = Resources.LoadAll<BodyCosmetic>("Cosmetics/Body");

            for (int i = 0; i < hatCosmetics.Length; i++)
            {
                if (hatCosmetics[i].type == HatCosmeticType.Default)
                {
                    _currentHatSelection = i;
                }
                _hatCosmetics.Add(hatCosmetics[i].type, hatCosmetics[i]);
                _hatSelection.Add(hatCosmetics[i]);
            }
            
            for (int i = 0; i < skinCosmetics.Length; i++)
            {
                if (skinCosmetics[i].type == SkinCosmeticType.Default)
                {
                    _currentSkinSelection = i;
                }
                _skinCosmetics.Add(skinCosmetics[i].type, skinCosmetics[i]);
                _skinSelection.Add(skinCosmetics[i]);
            }
            
            for (int i = 0; i < bodyCosmetics.Length; i++)
            {
                if (bodyCosmetics[i].type == BodyCosmeticType.Default)
                {
                    _currentBodySelection = i;
                }
                _bodyCosmetics.Add(bodyCosmetics[i].type, bodyCosmetics[i]);
                _bodySelection.Add(bodyCosmetics[i]);
            }
            
            ReloadAll();
        }

        public HatCosmetic GetHatCosmetic(HatCosmeticType hatType)
        {
            if (_hatCosmetics.TryGetValue(hatType, out HatCosmetic cosmetic))
            {
                return cosmetic;
            }
            
            Debug.LogWarning($"Hat Cosmetic: {hatType} could not be found");
            return null;
        }

        public SkinCosmetic GetSkinCosmetic(SkinCosmeticType skinType)
        {
            if (_skinCosmetics.TryGetValue(skinType, out SkinCosmetic cosmetic))
            {
                return cosmetic;
            }
            
            Debug.LogWarning($"Hat Cosmetic: {skinType} could not be found");
            return null;
        }

        public BodyCosmetic GetBodyCosmetic(BodyCosmeticType bodyType)
        {
            if (_bodyCosmetics.TryGetValue(bodyType, out BodyCosmetic cosmetic))
            {
                return cosmetic;
            }
            
            Debug.LogWarning($"Hat Cosmetic: {bodyType} could not be found");
            return null;
        }

        public void HatCycleLeft()
        {
            if (_currentHatSelection == 0)
            {
                _currentHatSelection = _hatSelection.Count - 1;
            }
            else
            {
                _currentHatSelection--;
            }
            
            ReloadHat();
        }

        public void HatCycleRight()
        {
            if (_currentHatSelection == _hatSelection.Count - 1)
            {
                _currentHatSelection = 0;
            }
            else
            {
                _currentHatSelection++;
            }
            
            ReloadHat();
        }

        public void BodyCycleLeft()
        {
            if (_currentBodySelection == 0)
            {
                _currentBodySelection = _bodySelection.Count - 1;
            }
            else
            {
                _currentBodySelection--;
            }
            
            ReloadBody();
        }

        public void BodyCycleRight()
        {
            if (_currentBodySelection == _bodySelection.Count - 1)
            {
                _currentBodySelection = 0;
            }
            else
            {
                _currentBodySelection++;
            }
            
            ReloadBody();
        }

        public void SkinCycleLeft()
        {
            if (_currentSkinSelection == 0)
            {
                _currentSkinSelection = _skinSelection.Count - 1;
            }
            else
            {
                _currentSkinSelection--;
            }
            
            ReloadSkin();
        }

        public void SkinCycleRight()
        {
            if (_currentSkinSelection == _skinSelection.Count - 1)
            {
                _currentSkinSelection = 0;
            }
            else
            {
                _currentSkinSelection++;
            }
            
            ReloadSkin();
        }

        private void ReloadHat()
        {
            if (_hatSelection[_currentHatSelection].type == HatCosmeticType.Default)
            {
                Color color = hatDisplay.color;
                color.a = 0;
                hatDisplay.color = color;
            }
            else
            {
                Color color = hatDisplay.color;
                color.a = 255;
                hatDisplay.color = color;

                Vector3 newScale = new Vector3();
            
                newScale.x = _hatSelection[_currentHatSelection].texture.bounds.extents.x * 2;
                newScale.y = _hatSelection[_currentHatSelection].texture.bounds.extents.y * 2;

                newScale *= hatSizeMultiplier;
                
                hatDisplay.transform.localScale = newScale;

                Vector3 newPosition = originalHatDisplayPos.position;
                newPosition.y += _hatSelection[_currentHatSelection].yOffset;
                hatDisplay.transform.position = newPosition;
            }
            hatDisplay.sprite = _hatSelection[_currentHatSelection].texture;
        }

        private void ReloadSkin()
        {
            skinDisplay.sprite = _skinSelection[_currentSkinSelection].texture;

            Vector3 newScale = new Vector3();
            
            newScale.x = _skinSelection[_currentSkinSelection].texture.bounds.extents.x * 2;
            newScale.y = _skinSelection[_currentSkinSelection].texture.bounds.extents.y * 2;
            
            newScale *= skinSizeMultiplier;

            skinDisplay.transform.localScale = newScale;
        }

        private void ReloadBody()
        {
            if (_bodySelection[_currentBodySelection].type == BodyCosmeticType.Default)
            {
                Color color = bodyDisplay.color;
                color.a = 0;
                bodyDisplay.color = color;
            }
            else
            {
                Color color = bodyDisplay.color;
                color.a = 255;
                bodyDisplay.color = color;

                Vector3 newScale = new Vector3();
            
                newScale.x = _bodySelection[_currentBodySelection].texture.bounds.extents.x * 2;
                newScale.y = _bodySelection[_currentBodySelection].texture.bounds.extents.y * 2;

                newScale *= bodySizeMultiplier;

                bodyDisplay.transform.localScale = newScale;
            }
            bodyDisplay.sprite = _bodySelection[_currentBodySelection].texture;
        }

        private void ReloadAll()
        {
            ReloadHat();
            ReloadSkin();
            ReloadBody();
        }
    }
}