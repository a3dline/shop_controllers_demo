using System;
using System.IO;
using UnityEditor;
#if UNITY_EDITOR
#endif

namespace Core
{
    internal class BuildInSceneProvider : ISceneProvider
    {
        public ISceneReference GetSceneReference(string sceneName)
        {
        #if UNITY_EDITOR
            if (!IsSceneInBuild(sceneName))
            {
                throw new Exception($"Scene '{sceneName}' is not in build settings.");
            }
        #endif

            return new BuildInSceneReference(sceneName);
        }

    #if UNITY_EDITOR
        private static bool IsSceneInBuild(string sceneName)
        {
            foreach (var scene in EditorBuildSettings.scenes)
            {
                var name = Path.GetFileNameWithoutExtension(scene.path);
                if (name == sceneName)
                {
                    return true;
                }
            }

            return false;
        }
    #endif
    }
}