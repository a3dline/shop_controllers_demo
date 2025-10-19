using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.GameShop
{
    public class ShopCardInfoView : MonoBehaviour
    {
        [SerializeField]
        private Transform _cardContainer;

        [SerializeField]
        private Button _exitBtn;

        public Transform CardContainer => _cardContainer;

        private void Awake()
        {
            _exitBtn.onClick.AddListener(() => { ExitButtonWasClicked?.Invoke(); });
        }

        public event Action ExitButtonWasClicked;
    }
}