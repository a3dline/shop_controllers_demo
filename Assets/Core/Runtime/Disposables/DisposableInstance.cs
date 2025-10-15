using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public static class DisposableInstanceExtensions
    {
        public static IDisposable ToDisposable(this Object instance) => new DisposableInstance(instance);
    }
    
    public sealed class DisposableInstance : DisposableBase
    {
        private readonly Object _instance;
        public DisposableInstance(Object instance)
        {
            _instance = instance;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_instance != null)
                {
                    if (Application.isPlaying)
                    {
                        Object.Destroy(_instance);
                    }
                    else
                    {
                        Object.DestroyImmediate(_instance);
                    }
                }
            }
        }
    }
}