using System;
using System.Globalization;
using Cysharp.Threading.Tasks;

namespace Core
{
    public class FixedSkuHandler : ISkuHandlerInternal
    {
        private readonly AsyncReactiveProperty<string> _balanceStringProperty = new("default");
        private IConvertible _value;
        private IConvertible _newValue;
        private bool _isDirty;

        public void AddTransaction(IConvertible amount)
        {
            _newValue = amount;
            _isDirty = true;
        }
        public bool IsValidTransaction(IConvertible amount)
        {
            return true;
        }
        public IReadOnlyAsyncReactiveProperty<string> BalanceString => _balanceStringProperty;

        string ISkuHandler.Id { get; set; }

        public IConvertible TakeNewBalanceAndClear()
        {
            _isDirty = false;
            return _newValue;
        }
        public bool IsDirty => _isDirty;
        public void UpdateBalance(IConvertible amount)
        {
            _value = amount;
            RaiseBalanceStringChange();
        }
        public void RaiseBalanceStringChange()
        {
            _balanceStringProperty.Value = _value.ToString(CultureInfo.InvariantCulture);
        }
    }
}