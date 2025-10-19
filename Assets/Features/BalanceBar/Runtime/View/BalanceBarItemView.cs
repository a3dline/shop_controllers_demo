using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.BalanceBar
{
    public class BalanceBarItemView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _balanceText;

        [SerializeField]
        private TMP_Text _labelText;

        [SerializeField]
        private Button _plusButton;

        private void Awake()
        {
            _plusButton.onClick.AddListener(() => PlusButtonWasClicked?.Invoke());
        }

        public event Action PlusButtonWasClicked;

        public void SetBalance(string balance)
        {
            _balanceText.text = balance;
        }

        public void SetLabel(string label)
        {
            _labelText.text = label;
        }
    }
}