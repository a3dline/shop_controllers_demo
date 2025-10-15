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
        
        public string BuildInDataResourcesAddress => _buildInDataResourcesAddress;
        public string CardPrefabAddress => _cardPrefabAddress;
    }
}