using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Core
{
    internal class InMemoryRepository : IRepository
    {
        private readonly Dictionary<string, string> _storage = new();
        
        public UniTask UpsetAsync(string key, string value)
        {
            _storage.TryAdd(key, value);
            return UniTask.CompletedTask;
        }

        public UniTask<string> GetAsync(string key)
        {
            _storage.TryGetValue(key, out var value);
            return UniTask.FromResult(value);
        }
    }
}