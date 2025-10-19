using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;

namespace Features.GameShop
{
    internal class CanPurchaseItemProperty : IReadOnlyAsyncReactivePropertyDisposable<bool>
    {
        private readonly Func<bool> _canPurchaseItemFunc;
        private readonly CancellationTokenSource _cts = new();
        private readonly IEnumerable<(ISkuHandler, IConvertible)> _handlers;
        private readonly AsyncReactiveProperty<bool> _property;

        public CanPurchaseItemProperty(List<(ISkuHandler, IConvertible)> handlers)
        {
            _handlers = handlers;
            _property = new AsyncReactiveProperty<bool>(CanPurchaseItem());
            foreach (var handler in handlers)
            {
                HandleBalanceChange(handler).Forget();
            }
        }

        public void Dispose()
        {
            _property?.Dispose();
            _cts?.Cancel();
            _cts?.Dispose();
        }

        IUniTaskAsyncEnumerator<bool> IUniTaskAsyncEnumerable<bool>.GetAsyncEnumerator(
            CancellationToken cancellationToken)
        {
            return _property.GetAsyncEnumerator(cancellationToken);
        }

        bool IReadOnlyAsyncReactiveProperty<bool>.Value => _property.Value;

        IUniTaskAsyncEnumerable<bool> IReadOnlyAsyncReactiveProperty<bool>.WithoutCurrent()
        {
            return _property.WithoutCurrent();
        }

        UniTask<bool> IReadOnlyAsyncReactiveProperty<bool>.WaitAsync(CancellationToken cancellationToken)
        {
            return _property.WaitAsync(cancellationToken);
        }

        private bool CanPurchaseItem()
        {
            return _handlers.All(x => x.Item1.IsValidTransaction(x.Item2));
        }

        private async UniTask HandleBalanceChange((ISkuHandler, IConvertible) value)
        {
            await foreach (var _ in value.Item1.BalanceString.WithCancellation(_cts.Token))
            {
                _property.Value = CanPurchaseItem();
            }
        }
    }
}