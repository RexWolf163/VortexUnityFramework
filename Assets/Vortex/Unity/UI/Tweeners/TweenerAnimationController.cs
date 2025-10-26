using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Vortex.Unity.UI.Tweeners
{
    [RequireComponent(typeof(Animator))]
    public class TweenerAnimationController : TweenerBase
    {
        [InfoBox("Для выбора ключа сделайте активным объект. Используются только Bool поля", InfoMessageType.Warning,
            "$IsNotActive")]
        [SerializeField]
        [ValueDropdown("$GetAnimatorKeys")]
        private string activeKey = "";

        [SerializeField] [ValueDropdown("$GetAnimatorKeys")]
        private string skipKey = "";

        [SerializeField] private Animator animator;

        [SerializeField] private UnityEvent beforeTween;

        public override void Forward(bool skip = false)
        {
            beforeTween?.Invoke();
            animator.SetBool(skipKey, skip);
            animator.SetBool(activeKey, true);
        }

        public override void Back(bool skip = false)
        {
            beforeTween?.Invoke();
            animator.SetBool(skipKey, skip);
            animator.SetBool(activeKey, false);
        }

#if UNITY_EDITOR
        private bool IsNotActive => !gameObject.activeSelf;

        private void OnValidate()
        {
            if (animator == null)
                animator = transform.GetComponent<Animator>();
        }

        private List<string> GetAnimatorKeys()
        {
            var result = new List<string>();
            if (IsNotActive)
                return result;
            foreach (var param in animator.parameters)
                if (param.type == AnimatorControllerParameterType.Bool)
                    result.Add(param.name);
            return result;
        }
#endif
    }
}