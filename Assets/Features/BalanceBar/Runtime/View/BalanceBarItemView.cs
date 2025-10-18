using TMPro;
using UnityEngine;

namespace Features.BalanceBar
{
    public class BalanceBarItemView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _balanceText;
        [SerializeField]
        private TMP_Text _labelText;
        
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