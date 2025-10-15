using System;
using UnityEngine;

namespace Core
{
    [DisallowMultipleComponent]
    public abstract class SceneContextBase : MonoBehaviour
    {
        private void Awake()
        {
            if (transform.parent != null)
            {
                throw new InvalidOperationException("SceneScopeBase must be a root object in the scene.");
            }
        }
    }
}