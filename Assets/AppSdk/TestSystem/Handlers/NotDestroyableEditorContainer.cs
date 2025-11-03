using UnityEngine;

namespace AppSdk.TestSystem.Handlers
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