using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Core
{
    public abstract class SkuHandlerWithTransactions : ISkuHandlerInternal
    {
        protected readonly AsyncReactiveProperty<string> BalanceStringProperty = new("0");
        protected readonly List<IConvertible> TransactionsQueue = new();

        protected IConvertible CurrentBalance;

        public IConvertible TakeNewBalanceAndClear()
        {
            var balance = CalculateBalanceFromTransactions();
            TransactionsQueue.Clear();
            return balance;
        }

        public bool IsDirty => TransactionsQueue.Count > 0;

        public virtual void UpdateBalance(IConvertible amount)
        {
            CurrentBalance = amount;
            BalanceStringProperty.Value = GetBalanceString(CurrentBalance);
        }

        public void AddTransaction(IConvertible amount)
        {
            TransactionsQueue.Add(amount);
        }

        public abstract bool IsValidTransaction(IConvertible amount);

        public IReadOnlyAsyncReactiveProperty<string> BalanceString => BalanceStringProperty;

        string ISkuHandler.SkuId { get; set; }

        protected abstract IConvertible CalculateBalanceFromTransactions();
        protected abstract string GetBalanceString(IConvertible amount);
    }
}