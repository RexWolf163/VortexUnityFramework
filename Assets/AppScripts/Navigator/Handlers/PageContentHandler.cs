using UnityEngine;
using UnityEngine.UI;
using Vortex.Unity.UI.UIComponents;

namespace AppScripts.Navigator.Handlers
{
    public class PageContentHandler : MonoBehaviour
    {
        [SerializeField] private UIComponent title;
        [SerializeField] private UIComponent textContent;

        [SerializeField] private RawImage photo;

        [SerializeField] private RectTransform containerPhoto;

        private int currentText;
        private int currentPhoto;

        private NavigatorPage _page;

        private Texture2D _texture;

        private void Awake()
        {
            _texture = new Texture2D(1, 1);
        }

        private void OnEnable()
        {
            NavigatorController.OnInit += Refresh;
            NavigatorController.OnChangePage += Refresh;
            currentText = 0;
            currentPhoto = 0;
            Refresh();
        }

        private void OnDisable()
        {
            NavigatorController.OnInit -= Refresh;
            NavigatorController.OnChangePage -= Refresh;
        }

        private void Refresh()
        {
            if (NavigatorController.IsHome())
            {
                title.SetText("");
                textContent.SetText("");
                SetTexture();
                return;
            }

            var pageId = NavigatorController.GetCurrentPage();
            _page = NavigatorController.GetPageData(pageId);
            title.SetText(_page.Name);
            textContent.SetText(_page.Content.Length != 0 ? _page.Content[currentText] : string.Empty);
            containerPhoto.sizeDelta = new Vector2(_page.PhotoWidth, containerPhoto.sizeDelta.y);
            SetTexture();
        }

        private void SetTexture()
        {
            if (NavigatorController.IsHome() || _page.Photos.Length == 0)
            {
                photo.color = new Color(0, 0, 0, 0);
                return;
            }

            photo.color = Color.white;
            _texture.LoadImage(_page.Photos[currentPhoto]);
            _texture.Apply();
            photo.texture = _texture;
        }

        public void NextText()
        {
            if (_page.Content.Length == 0)
                return;
            if (++currentText >= _page.Content.Length)
                currentText = _page.Content.Length - 1;
            textContent.SetText(_page.Content[currentText]);
        }

        public void PrevText()
        {
            if (_page.Content.Length == 0)
                return;
            if (--currentText < 0)
                currentText = 0;
            textContent.SetText(_page.Content[currentText]);
        }

        public void NextPhoto()
        {
            if (_page.Photos.Length == 0)
                return;
            if (++currentPhoto >= _page.Photos.Length)
                currentPhoto = _page.Photos.Length - 1;
            SetTexture();
        }

        public void PrevPhoto()
        {
            if (_page.Photos.Length == 0)
                return;
            if (--currentPhoto < 0)
                currentPhoto = 0;
            SetTexture();
        }
    }
}