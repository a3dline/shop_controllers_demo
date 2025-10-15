using System;
using UnityEngine;

namespace Features.GameShop
{
    [Serializable]
    public class GameShopContext
    {
        [SerializeField]
        private string _buildInDataResourcesAddress;
        
        public string BuildInDataResourcesAddress => _buildInDataResourcesAddress;
    }
}