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

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }

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
            await foreach (var _ in UniTaskAsyncEnumerable.Interval(TimeSpan.FromSeconds(1))
                                                          .WithCancellation(_cts.Token))
            {
                var current = Convert.ToInt64(Balance, CultureInfo);
                var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (current >= now)
                {
                    BalanceStringProperty.Value = GetBalanceString(Balance);
                }
                else
                {
                    break;
                }
            }
        }

        public override bool IsValidTransaction(IConvertible amount)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return Add(Balance, amount).ToInt64(CultureInfo) > now;
        }

        public override IConvertible Add(IConvertible a, IConvertible b)
        {
            return Convert.ToInt64(a, CultureInfo) + Convert.ToInt64(b, CultureInfo);
        }

        protected override IConvertible CalculateBalanceFromTransactions()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var current = Convert.ToInt64(Balance, CultureInfo);

            IConvertible summ = current < now ? now : current;

            foreach (var transaction in TransactionsQueue)
            {
                summ = Add(summ, transaction);
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