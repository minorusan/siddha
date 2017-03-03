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
            _state = ETouchState.Idle;
            TouchPosition = Vector3.zero;
        }

        void Update()
        {
            if (_state == ETouchState.Pressed)
            {
                Vector2 screenCoords;
#if UNITY_EDITOR
                screenCoords = UnityEngine.Input.mousePosition;
#else
                screenCoords = UnityEngine.Input.touches[0].position;
#endif
                TouchPosition = Camera.main.ScreenToWorldPoint(screenCoords);
            }
        }
    }

}
