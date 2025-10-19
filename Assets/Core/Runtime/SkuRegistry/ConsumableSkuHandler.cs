using System;
using System.Globalization;

namespace Core
{
    public class ConsumableSkuHandler : SkuHandlerWithTransactions
    {
        private CultureInfo CultureInfo => CultureInfo.InvariantCulture;

        public override bool IsValidTransaction(IConvertible amount)
        {
            return amount.ToInt32(CultureInfo) >= CurrentBalance.ToInt32(CultureInfo);
        }

        protected override IConvertible CalculateBalanceFromTransactions()
        {
            var current = Convert.ToInt32(CurrentBalance, CultureInfo);
            foreach (var transaction in TransactionsQueue)
            {
                current += Convert.ToInt32(transaction, CultureInfo);
            }

            return current;
        }

        protected override string GetBalanceString(IConvertible amount)
        {
            return amount.ToString(CultureInfo);
        }
    }
}