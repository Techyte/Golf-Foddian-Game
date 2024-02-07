using System;
using Cosmetics;
using UnityEngine;

namespace Game
{
    public class GameCosmeticSetup : MonoBehaviour
    {
        [SerializeField] private PlayerCosmeticManager _manager;

        private HatCosmetic hat;
        private SkinCosmetic skin;
        private BodyCosmetic body;
        
        private void Start()
        {
            hat = CosmeticManager.Instance.GetCurrentHat();
            skin = CosmeticManager.Instance.GetCurrentSkin();
            body = CosmeticManager.Instance.GetCurrentBody();
        }

        private void Update()
        {
            _manager.SetHat(hat);
            _manager.SetSkin(skin);
            _manager.SetBody(body);
        }
    }
}