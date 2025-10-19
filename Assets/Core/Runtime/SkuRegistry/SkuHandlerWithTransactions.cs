using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Core
{
    public abstract class SkuHandlerWithTransactions : ISkuHandlerInternal
    {
        private readonly AsyncReactiveProperty<string> _balanceStringProperty = new("0");
        protected readonly List<IConvertible> TransactionsQueue = new();

        protected IConvertible CurrentBalance;

        public IConvertible TakeNewBalanceAndClear()
        {
            var balance = CalculateBalanceFromTransactions();
            TransactionsQueue.Clear();
            return balance;
        }

        public bool IsDirty => TransactionsQueue.Count > 0;

        public void UpdateBalance(IConvertible amount)
        {
            CurrentBalance = amount;
            RaiseBalanceStringChange();
        }

        public void RaiseBalanceStringChange()
        {
            _balanceStringProperty.Value = GetBalanceString(CurrentBalance);
        }

        public void AddTransaction(IConvertible amount)
        {
            TransactionsQueue.Add(amount);
        }

        public abstract bool IsValidTransaction(IConvertible amount);

        public IReadOnlyAsyncReactiveProperty<string> BalanceStringProperty => _balanceStringProperty;

        string ISkuHandler.Id { get; set; }

        protected abstract IConvertible CalculateBalanceFromTransactions();
        protected abstract string GetBalanceString(IConvertible amount);
    }
}