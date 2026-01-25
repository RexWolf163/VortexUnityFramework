using UnityEngine;
using UnityEngine.InputSystem;

namespace Vortex.Unity.UI.Misc
{
    /// <summary>
    /// Хэндлер подписок на клавиши управления
    /// </summary>
    public class KeyboardHandler : MonoBehaviour
    {
        [SerializeField] private AdvancedButton button;

        [SerializeField] private Key[] buttonCode;

        private InputAction _inputAction;

        private void Awake()
        {
            _inputAction = new InputAction(
                name: $"ButtonHandler KeyGroup «{name} ({string.Join(";", buttonCode)})»",
                type: InputActionType.Button
            );

            foreach (var code in buttonCode)
                _inputAction.AddBinding($"<Keyboard>/{code}");

            _inputAction.started += OnPressed;
            _inputAction.canceled += OnCanceled;
        }

        private void OnDestroy()
        {
            _inputAction.started -= OnPressed;
            _inputAction.canceled -= OnCanceled;
            _inputAction?.Dispose();
        }

        private void OnEnable() => _inputAction.Enable();

        private void OnDisable() => _inputAction.Disable();

        private void OnPressed(InputAction.CallbackContext context) => button?.Press();

        private void OnCanceled(InputAction.CallbackContext context) => button?.Release();
    }
}