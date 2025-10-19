using System;
using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace Core
{
    public class ConsumableTimeSpanSkuHandler : SkuHandlerWithTransactions, IDisposable
    {
        private readonly CancellationTokenSource _cts = new();

        private CultureInfo CultureInfo => CultureInfo.InvariantCulture;

        public override void UpdateBalance(IConvertible amount)
        {
            base.UpdateBalance(amount);
            var current = Convert.ToInt64(amount, CultureInfo);
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (current >= now)
            {
                UpdateBalanceStringFlow().Forget();    
            }
        }

        private async UniTaskVoid UpdateBalanceStringFlow()
        {
            await foreach (var _ in UniTaskAsyncEnumerable.Interval(TimeSpan.FromSeconds(1)).WithCancellation(_cts.Token))
            {
                var current = Convert.ToInt64(CurrentBalance, CultureInfo);
                var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (current >= now)
                {
                    BalanceStringProperty.Value = GetBalanceString(CurrentBalance);    
                }
                else
                {
                    break;
                }
            }
        }

        public override bool IsValidTransaction(IConvertible amount)
        {
            var a = Convert.ToInt64(amount, CultureInfo);
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var current = Convert.ToInt64(CurrentBalance, CultureInfo);
            return current + a > now;
        }

        protected override IConvertible CalculateBalanceFromTransactions()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var current = Convert.ToInt64(CurrentBalance, CultureInfo);

            var summ = current < now ? now : current;
            
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

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}