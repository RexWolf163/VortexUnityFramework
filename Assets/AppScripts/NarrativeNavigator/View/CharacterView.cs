using System.Collections.Generic;
using AppScripts.Narrative.Model;
using UnityEngine;
using Vortex.Unity.DatabaseSystem.Attributes;
using Vortex.Unity.UI.StateSwitcher;

namespace AppScripts.NarrativeNavigator.View
{
    public class CharacterView : MonoBehaviour
    {
        [DbRecord(typeof(NrCharacterModel)), SerializeField]
        private string id;

        /// <summary>
        /// Переключатель состояния персонажа (улыбка, злость и т.п.)
        /// </summary>
        [SerializeField] private UIStateSwitcher[] charStates;

        public void Init(IEnumerable<string> keys)
        {
            for (var i = charStates.Length - 1; i >= 0; i--)
            {
                var charState = charStates[i];
                charState.Reset();
            }

            for (var i = charStates.Length - 1; i >= 0; i--)
            {
                var charState = charStates[i];
                foreach (var key in keys)
                {
                    var state = charState.GetState(key);
                    if (state < 0)
                        continue;
                    charState.Set(state);
                    break;
                }
            }
        }
    }
}