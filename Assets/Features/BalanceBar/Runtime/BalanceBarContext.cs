using System;
using UnityEngine;

namespace Features.BalanceBar
{
    [Serializable]
    public class BalanceBarContext
    {
        [SerializeField]
        private string _balanceBarPrefabAddress;
        
        public string BalanceBarPrefabAddress => _balanceBarPrefabAddress;
    }
}