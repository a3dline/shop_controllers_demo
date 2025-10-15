using UnityEngine;

namespace Features.GameShop
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField]
        private Transform _cardsContainer;
        
        public Transform CardsContainer => _cardsContainer;
    }
}