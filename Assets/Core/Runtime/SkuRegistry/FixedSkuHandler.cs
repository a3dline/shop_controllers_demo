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
            return amount.ToString(CultureInfo.InvariantCulture) == _value.ToString(CultureInfo.InvariantCulture);
        }
        public IReadOnlyAsyncReactiveProperty<string> BalanceString => _balanceStringProperty;

        string ISkuHandler.SkuId { get; set; }
        public IConvertible DefaultBalance { get; set; }
        public IConvertible Balance => _value;
        public IConvertible Add(IConvertible a, IConvertible b)
        {
            return b;
        }

        public IConvertible TakeNewBalanceAndClear()
        {
            _isDirty = false;
            return _newValue;
        }
        public bool IsDirty => _isDirty;
        public void UpdateBalance(IConvertible amount)
        {
            _value = amount;
            _balanceStringProperty.Value = _value.ToString(CultureInfo.InvariantCulture);
        }
    }
}