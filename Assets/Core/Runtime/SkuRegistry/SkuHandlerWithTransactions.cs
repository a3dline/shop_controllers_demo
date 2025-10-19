using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Core
{
    public abstract class SkuHandlerWithTransactions : ISkuHandlerInternal
    {
        protected readonly AsyncReactiveProperty<string> BalanceStringProperty = new("0");
        protected readonly List<IConvertible> TransactionsQueue = new();

        private IConvertible _balance;

        public IConvertible TakeNewBalanceAndClear()
        {
            var balance = CalculateBalanceFromTransactions();
            TransactionsQueue.Clear();
            return balance;
        }

        public bool IsDirty => TransactionsQueue.Count > 0;

        public virtual void UpdateBalance(IConvertible amount)
        {
            _balance = amount;
            BalanceStringProperty.Value = GetBalanceString(_balance);
        }

        public void AddTransaction(IConvertible amount)
        {
            TransactionsQueue.Add(amount);
        }

        public abstract bool IsValidTransaction(IConvertible amount);

        public IReadOnlyAsyncReactiveProperty<string> BalanceString => BalanceStringProperty;

        string ISkuHandler.SkuId { get; set; }
        public IConvertible DefaultBalance { get; set; }
        public IConvertible Balance => _balance;
        public abstract IConvertible Add(IConvertible a, IConvertible b);
        protected abstract IConvertible CalculateBalanceFromTransactions();
        protected abstract string GetBalanceString(IConvertible amount);
    }
}