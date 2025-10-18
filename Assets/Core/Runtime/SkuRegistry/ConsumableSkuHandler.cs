using System;
using System.Globalization;

namespace Core
{
    public abstract class ConsumableSkuHandler : SkuHandler
    {
        public abstract override string Name { get; }

        protected override CultureInfo CultureInfo => CultureInfo.InvariantCulture;

        public override bool IsValidTransaction(IConvertible amount)
        {
            var balance = CalculateBalance();
            var newBalance = balance.ToInt32(CultureInfo) + amount.ToInt32(CultureInfo);
            return newBalance >= 0;
        }

        protected override IConvertible CalculateBalance()
        {
            var current = Convert.ToInt32(CurrentBalance, CultureInfo);
            foreach (var transaction in TransactionsQueue)
            {
                current += Convert.ToInt32(transaction, CultureInfo);
            }

            return current;
        }
    }
}