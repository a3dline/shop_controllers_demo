using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
{
    internal class PlayerPrefsRepository : IRepository
    {
        public UniTask UpsetAsync(string key, string value, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            PlayerPrefs.SetString(key, value);
            return UniTask.CompletedTask;
        }

        public UniTask<string> GetAsync(string key, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var value = PlayerPrefs.GetString(key, string.Empty);
            return UniTask.FromResult(value);
        }
    }
}