#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Vortex.Unity.UI.UIComponents;

public class AppExitHandler : MonoBehaviour
{
    [SerializeField] private UIComponent button;

    private void Awake()
    {
        button.SetAction(Exit);
    }

    private void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}