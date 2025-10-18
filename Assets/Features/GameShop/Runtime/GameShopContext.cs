using System;
using UnityEngine;

namespace Features.GameShop
{
    [Serializable]
    public class GameShopContext
    {
        [SerializeField]
        private string _buildInDataResourcesAddress;
        
        [SerializeField]
        private string _cardPrefabAddress = "CardPrefab";
        
        [SerializeField]
        private string _gameShopSceneName = "GameShop";
        
        public string BuildInDataResourcesAddress => _buildInDataResourcesAddress;
        public string CardPrefabAddress => _cardPrefabAddress;
        public string GameShopSceneName => _gameShopSceneName;
    }
}