using System;
using System.Globalization;

namespace Core
{
    public class ConsumableSkuHandler : SkuHandlerWithTransactions
    {
        private CultureInfo CultureInfo => CultureInfo.InvariantCulture;

        public override bool IsValidTransaction(IConvertible amount)
        {
            return Add(Balance, amount).ToInt32(CultureInfo)  >= 0;
        }

        public override IConvertible Add(IConvertible a, IConvertible b)
        {
            return a.ToInt32(CultureInfo) + b.ToInt32(CultureInfo);
        }

        protected override IConvertible CalculateBalanceFromTransactions()
        {
            var summ = Balance;
            foreach (var transaction in TransactionsQueue)
            {
                summ = Add(summ, transaction);
            }

            return summ;
        }

        protected override string GetBalanceString(IConvertible amount)
        {
            return amount.ToString(CultureInfo);
        }
    }
}