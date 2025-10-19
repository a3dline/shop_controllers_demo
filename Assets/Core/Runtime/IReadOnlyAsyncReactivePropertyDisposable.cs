using System;
using Cysharp.Threading.Tasks;

namespace Core
{
    public interface IReadOnlyAsyncReactivePropertyDisposable<T> : IReadOnlyAsyncReactiveProperty<T>, IDisposable { }
}