using System;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Threading.Tasks;

namespace Core
{
    public abstract class SkuHandler : ISkuHandlerInternal
    {
        private readonly AsyncReactiveProperty<string> _balanceStringProperty = new("0");
        protected readonly List<IConvertible> TransactionsQueue = new();

        protected IConvertible CurrentBalance;

        public IConvertible TakeNewBalanceAndClear()
        {
            var balance = CalculateBalance();
            TransactionsQueue.Clear();
            return balance;
        }

        public bool IsDirty => TransactionsQueue.Count > 0;

        public void UpdateBalance(IConvertible amount)
        {
            CurrentBalance = amount;
            var balanceString = amount.ToString(CultureInfo);
            _balanceStringProperty.Value = balanceString;
        }

        public void AddTransaction(IConvertible amount)
        {
            TransactionsQueue.Add(amount);
        }

        public abstract bool IsValidTransaction(IConvertible amount);

        public IReadOnlyAsyncReactiveProperty<string> BalanceStringProperty => _balanceStringProperty;
        public abstract string Name { get; }
        protected abstract IConvertible CalculateBalance();
        protected abstract CultureInfo CultureInfo { get; }
    }
}