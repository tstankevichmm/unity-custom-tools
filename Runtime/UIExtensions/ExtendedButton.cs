using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CustomTools.UIExtensions
{
    public class ExtendedButton : Button
    {
        [SerializeField] private ButtonClickedEvent _onRightClick = new();

        [SerializeField] private ButtonClickedEvent _onMiddleClick = new();

        public ButtonClickedEvent onRightClick
        {
            get => _onRightClick;
            set => _onRightClick = value;
        }

        public ButtonClickedEvent onMiddleClick
        {
            get => _onMiddleClick;
            set => _onMiddleClick = value;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    base.OnPointerClick(eventData);
                    break;
                case PointerEventData.InputButton.Right:
                    PressRight();
                    break;
                case PointerEventData.InputButton.Middle:
                    PressMiddle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PressMiddle()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("ExtendedButton.onMiddleClick", this);
            _onMiddleClick.Invoke();
        }

        private void PressRight()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("ExtendedButton.onRightClick", this);
            _onRightClick.Invoke();
        }
    }
}