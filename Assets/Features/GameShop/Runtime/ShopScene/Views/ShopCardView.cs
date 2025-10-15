using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.GameShop
{
    public class ShopCardView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _headerText;

        [SerializeField]
        private Button _infoBtn;

        [SerializeField]
        private Button _purchaseBtn;

        private void Awake()
        {
            _infoBtn.onClick.AddListener(() => InfoBtnWasClicked?.Invoke());
            _purchaseBtn.onClick.AddListener(() => PurchaseBtnWasClicked?.Invoke());
        }

        public event Action InfoBtnWasClicked;
        public event Action PurchaseBtnWasClicked;

        public void SetHeaderText(string text)
        {
            _headerText.text = text;
        }
    }
}