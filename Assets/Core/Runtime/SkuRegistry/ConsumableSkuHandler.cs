using System;
using System.Globalization;

namespace Core
{
    public class ConsumableSkuHandler : SkuHandler
    {
        protected override CultureInfo CultureInfo => CultureInfo.InvariantCulture;

        public override bool IsValidTransaction(IConvertible amount)
        {
            return amount.ToInt32(CultureInfo) >= CurrentBalance.ToInt32(CultureInfo);
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