using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.DatabaseSystem.Presets;
using Vortex.Unity.UIProviderSystem.Enums;
using Vortex.Unity.UIProviderSystem.Model;
using Vortex.Unity.UIProviderSystem.Model.Conditions;

namespace Vortex.Unity.UIProviderSystem.Presets
{
    [CreateAssetMenu(fileName = "UiPreset", menuName = "Database/UserInterface Preset")]
    public class UserInterfacePreset : RecordPreset<UserInterfaceData>
    {
        [SerializeField] private UserInterfaceTypes uiType;

        public UserInterfaceTypes UIType => uiType;

        [SerializeReference, HideReferenceObjectPicker]
        private UserInterfaceCondition[] conditions = new UserInterfaceCondition[0];

        public UserInterfaceCondition[] Conditions => conditions.ToArray();
    }
}