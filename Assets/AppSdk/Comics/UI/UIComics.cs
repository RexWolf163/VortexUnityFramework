using System;
using System.Collections.Generic;
using AppSdk.Comics.Database;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Vortex.Unity.UI.UIComponents;

namespace AppSdk.Comics.UI
{
    /// <summary>
    /// Интерфейс отображения комикса - заставки (катсценки)
    ///
    /// При активации цепляет данные из шины ComicsController
    /// и обеспечивает их отображение
    ///
    /// Органы управления - кнопка следующего слайда и кнопка прерывания
    ///
    /// TODO: управление музыкой
    /// </summary>
    public class UIComics : MonoBehaviour
    {
        /// <summary>
        /// Указатель на текущий комикс
        /// </summary>
        private ComicsData _comicsData;

        /// <summary>
        /// Текущий отображаемый этап
        /// </summary>
        private ComicsStage _currentStage;

        /// <summary>
        /// номер текущего кадра комикса
        /// </summary>
        private short _numberStage = 0;

        /// <summary>
        /// Картинка кадра комикса
        /// </summary>
        [SerializeField] private Image comics;

        /// <summary>
        /// Картинка "тени" комикса.
        /// Заполняется "устаревшим кадром" при смене этапов комикса, для плавного перехода между кадрами
        /// </summary>
        [SerializeField] private Image comicsShade;

        /// <summary>
        /// Кнопка перехода на следующий слайд
        /// </summary>
        [SerializeField] private UIComponent nextBtn;

        /// <summary>
        /// Кнопка прерывания показа
        /// </summary>
        [SerializeField] private UIComponent cancelBtn;

        /// <summary>
        /// Блок вывода текста 
        /// </summary>
        [SerializeField] private UIComponent textBox;

        /// <summary>
        /// Блок "тени" текста для плавной смены кадра 
        /// </summary>
        [SerializeField] private UIComponent textBoxShade;

        [SerializeField] private Animator animator;

        [SerializeField, ValueDropdown("GetTriggers"),
         InfoBox("Укажите триггер аниматора для запуска анимации переключения на следующий слайд")]
        private string triggerNextSlide;

        private void Awake()
        {
            comicsShade.sprite = null;
            comics.sprite = null;
            nextBtn.SetAction(NextSlide);
            cancelBtn.SetAction(ExitComics);
            textBox.SetText("");
            textBoxShade.SetText("");
        }

        private void OnEnable()
        {
            _comicsData = ComicsController.GetCurrentComics();
            _numberStage = 0;
            if (_comicsData == null)
                return;
            _currentStage = _comicsData.ComicsStages[_numberStage];
            textBox.SetText(_currentStage.Text);
            comics.sprite = _currentStage.Picture;
            /*
            if(!_currentStage.MusicId.IsNullOrWhitespace())
                AudioPlayer.Play();
        */
        }

        private void OnDisable()
        {
            _comicsData = null;
        }

        /// <summary>
        /// Вызов следующего слайда
        /// </summary>
        private void NextSlide()
        {
            comicsShade.sprite = _currentStage.Picture;
            textBoxShade.SetText(_currentStage.Text);
            if (++_numberStage == _comicsData.ComicsStages.Length)
            {
                ExitComics();
                return;
            }

            _currentStage = _comicsData.ComicsStages[_numberStage];
            comics.sprite = _currentStage.Picture;
            textBox.SetText(_currentStage.Text);
            animator.SetTrigger(triggerNextSlide);
            //TODO доделать переключение музыки
        }

        private void ExitComics() => ComicsController.StopComics();

#if UNITY_EDITOR
        private bool IsNotActive => !gameObject.activeSelf;

        private void OnValidate()
        {
            if (animator == null)
                animator = transform.GetComponent<Animator>();
        }

        private List<string> GetTriggers()
        {
            var result = new List<string>();
            if (IsNotActive)
                return result;
            foreach (var param in animator.parameters)
                if (param.type == AnimatorControllerParameterType.Trigger)
                    result.Add(param.name);
            return result;
        }
#endif
    }
}