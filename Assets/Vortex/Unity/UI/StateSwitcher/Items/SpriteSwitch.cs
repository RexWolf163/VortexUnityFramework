using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Vortex.UI.Components.StateSwitcher.Items
{
    public class SpriteSwitch : StateItem
    {
        [SerializeField] [PreviewField(60f, ObjectFieldAlignment.Left)] [HorizontalGroup("h1", 70f)] [HideLabel]
        private Sprite _sprite;

        [SerializeField] [HorizontalGroup("h1")] [HideLabel] [OnValueChanged("CheckObject")]
        private Object _image;

        public override void Set()
        {
            switch (_image)
            {
                case SpriteRenderer sprRend:
                    sprRend.sprite = _sprite;
                    break;
                case Image img:
                    img.sprite = _sprite;
                    break;
            }
        }

        public override void DefaultState()
        {
            switch (_image)
            {
                case SpriteRenderer sprRend:
                    sprRend.sprite = null;
                    break;
                case Image img:
                    img.sprite = null;
                    break;
            }
        }

#if UNITY_EDITOR
        public override StateItem Clone()
        {
            var clone = new SpriteSwitch
            {
                _sprite = _sprite,
                _image = _image
            };
            return clone;
        }

        [ShowIf("@_image != null")]
        [Button("Get from...")]
        [GUIColor("@Color.green")]
        [HorizontalGroup("h1")]
        private void SetCurrent()
        {
            switch (_image)
            {
                case SpriteRenderer sprRend:
                    _sprite = sprRend.sprite;
                    break;
                case Image img:
                    _sprite = img.sprite;
                    break;
            }
        }

        public override string DropDownItemName => "Switch Sprite";
        public override string DropDownGroupName => "Graphics";

        private void CheckObject()
        {
            switch (_image)
            {
                case SpriteRenderer sprRen:
                case Image img:
                    break;
                default:
                    var go = (_image as GameObject)?.gameObject;
                    if (go == null)
                        go = (_image as UnityEngine.Component)?.gameObject;
                    if (go == null)
                    {
                        _image = null;
                        return;
                    }

                    Object cmp = go.GetComponent<SpriteRenderer>();
                    if (cmp == null)
                        cmp = go.GetComponent<Image>();

                    if (cmp != null)
                        _image = cmp;
                    break;
            }
        }
#endif
    }
}