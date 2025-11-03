using UnityEngine;

namespace AppScripts.Test
{
    public class NotDestroyableEditorContainer : MonoBehaviour
    {
        void Awake()
        {
#if UNITY_EDITOR
            DontDestroyOnLoad(this);
#else
            Destroy(this);
#endif
        }
    }
}