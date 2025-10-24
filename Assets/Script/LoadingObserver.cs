using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class LoadingObserver : MonoBehaviour
{
    [FormerlySerializedAs("machine")] [SerializeField]
    private Variables variables;

    [SerializeField, ValueDropdown("GetList")]
    private string loadCompleteVar;

    private void Awake()
    {
        Vortex.Core.AppSystem.Bus.App.OnStart += OnStart;
    }

    void OnDestroy()
    {
        Vortex.Core.AppSystem.Bus.App.OnStart -= OnStart;
    }

    [Button]
    private void OnStart()
    {
        variables.declarations.Set(loadCompleteVar, true);
    }

    private ValueDropdownList<string> GetList()
    {
        var res = new ValueDropdownList<string>();
        foreach (var variable in variables.declarations) res.Add(variable.name);
        return res;
    }
}