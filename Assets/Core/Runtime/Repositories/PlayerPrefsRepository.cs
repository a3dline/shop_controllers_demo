using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
{
    internal class PlayerPrefsRepository : IRepository
    {
        public UniTask UpsetAsync(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            return UniTask.CompletedTask;
        }

        public UniTask<string> GetAsync(string key)
        {
            var value = PlayerPrefs.GetString(key, string.Empty);
            return UniTask.FromResult(value);
        }
    }
}