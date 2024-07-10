using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FoodMatch.Input
{
    public class InputService : IDisposable
    {
        public delegate void TouchDelegate(Vector2 touchPosition);
        public event TouchDelegate OnTouchMoved;
        public event TouchDelegate OnTouchReleased;

        private readonly InputActions _inputActions;

        public InputService()
        {
            _inputActions = new InputActions();
            _inputActions.Enable();
            _inputActions.Default.Touch.performed += HandleTouchMoved;
            _inputActions.Default.Touch.canceled += HandleTouchReleased;
            _inputActions.Default.TouchPosition.performed += HandleTouchMoved;
        }
        private void HandleTouchReleased(InputAction.CallbackContext context)
        {
            OnTouchReleased?.Invoke(_inputActions.Default.TouchPosition.ReadValue<Vector2>());
        }

        private void HandleTouchMoved(InputAction.CallbackContext context)
        {
            OnTouchMoved?.Invoke(_inputActions.Default.TouchPosition.ReadValue<Vector2>());
        }

        public void Dispose()
        {
            _inputActions.Default.Touch.performed -= HandleTouchMoved;
            _inputActions.Default.Touch.canceled -= HandleTouchReleased;
            _inputActions.Default.TouchPosition.performed -= HandleTouchMoved;
            _inputActions?.Dispose();
        }

        public void Enable()
        {
            _inputActions.Enable();
        }

        public void Disable()
        {
            _inputActions.Disable();
        }
    }
}
