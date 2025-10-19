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

        [SerializeField]
        private TMP_Text _purchaseBtnText;

        public bool EnablePurchaseBtn
        {
            get => _purchaseBtn.interactable;
            set => _purchaseBtn.interactable = value;
        }

        public string PurchaseBtnText
        {
            get => _purchaseBtnText.text;
            set => _purchaseBtnText.text = value;
        }

        public bool EnableInfoBtn
        {
            get => _infoBtn.gameObject.activeSelf;
            set => _infoBtn.gameObject.SetActive(value);
        }

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