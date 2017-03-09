using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Input
{
    public class TouchInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        enum ETouchState
        {
            Idle,
            Pressed
        }

        private ETouchState _state = ETouchState.Idle;
        public Camera Camera;

        public static Vector2 TouchPosition { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            _state = ETouchState.Pressed;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
#if UNITY_EDITOR
            HandleCancelTouch();
#endif
        }

        void Update()
        {
#if !UNITY_EDITOR
                 HandleCancelTouch();
#endif
            if (_state == ETouchState.Pressed)
            {
                Vector2 screenCoords;
#if UNITY_EDITOR

                screenCoords = UnityEngine.Input.mousePosition;
#else
                screenCoords = UnityEngine.Input.touches[UnityEngine.Input.touches.Length-1].position;
#endif
                TouchPosition = Camera.main.ScreenToWorldPoint(screenCoords);
            }
        }

        private void HandleCancelTouch()
        {
#if UNITY_ANDROID
            if (UnityEngine.Input.touches.Length <= 0)
            {
                _state = ETouchState.Idle;
                TouchPosition = Vector3.zero;
            }
#endif
        }
    }

}
