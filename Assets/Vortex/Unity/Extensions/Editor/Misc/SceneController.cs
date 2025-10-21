using System.Collections.Generic;

namespace Vortex.Unity.Extensions.Editor.Misc
{
    public static class SceneController
    {
#if UNITY_EDITOR

        public static List<string> GetScenes()
        {
            var result = new List<string>();

            var scenes = UnityEditor.EditorBuildSettings.scenes;

            foreach (var scene in scenes)
            {
                var sceneName = scene.path.Split("/")[^1].Split('.')[0];
                result.Add(sceneName);
            }

            return result;
        }
    }
#endif
}