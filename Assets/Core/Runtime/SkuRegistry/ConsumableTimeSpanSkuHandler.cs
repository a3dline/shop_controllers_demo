using System;
using System.Globalization;

namespace Core
{
    public class ConsumableTimeSpanSkuHandler : SkuHandlerWithTransactions
    {
        private CultureInfo CultureInfo => CultureInfo.InvariantCulture;

        public override bool IsValidTransaction(IConvertible amount)
        {
            var current = Convert.ToInt64(CurrentBalance, CultureInfo);
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds() < current;
        }

        protected override IConvertible CalculateBalanceFromTransactions()
        {
            var summ = Convert.ToInt64(CurrentBalance, CultureInfo);
            foreach (var transaction in TransactionsQueue)
            {
                summ += Convert.ToInt64(transaction, CultureInfo);
            }

            return summ;
        }

        protected override string GetBalanceString(IConvertible amount)
        {
            var value = Convert.ToInt64(amount, CultureInfo);
            var current = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (value < current)
            {
                return "00:00:00";
            }

            var delta = value - current;
            var timeSpan = TimeSpan.FromSeconds(delta);
            var stringDelta = timeSpan.ToString(@"hh\:mm\:ss");
            return stringDelta;
        }
    }
}