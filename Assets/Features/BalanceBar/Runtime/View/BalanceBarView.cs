using UnityEngine;

namespace Features.BalanceBar
{
    public class BalanceBarView : MonoBehaviour
    {
        [SerializeField]
        private Transform _itemsContainer;
        
        [SerializeField]
        private BalanceBarItemView _itemPrefab;
        
        public Transform ItemsContainer => _itemsContainer;
        public BalanceBarItemView ItemPrefab => _itemPrefab;
    }
}