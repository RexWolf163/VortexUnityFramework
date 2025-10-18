#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Chains.Model;
using Vortex.Database;

namespace Vortex.Chains.Controllers
{
    public partial class ChainsController
    {
        /// <summary>
        /// Дропдаун список для инспектора
        /// </summary>
        /// <returns></returns>
        public static ValueDropdownList<string> GetChains()
        {
            if (Application.isPlaying)
                return new ValueDropdownList<string>();

            var result = new ValueDropdownList<string>();

            var list = DatabaseController.GetRecordsStatic<ChainData>();
            result.Add("None", string.Empty);
            foreach (var stage in list)
                result.Add(stage.GetName(), stage.GetGuid());

            return result;
        }
    }
}
#endif