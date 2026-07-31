using System;
using Input;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace App
{
    public interface IInputBridge
    {
        void InitializeInputs();
    }

    public class InputBridge : MonoBehaviour, IInputBridge
    {
        enum CallbackType { Started, Cancelled, Performed }

        [Header("UI Reference")]
        [SerializeField] private PanelRenderer _panelRenderer;

        [Header("Input Action References")]
        [SerializeField] private InputActionReference _doubleClickAction;
        [SerializeField] private InputActionReference _longPressAction;
        [SerializeField] private InputActionReference _pointAction;

        private VisualElement _root;
        private bool _isDragging = false;

        private void Awake()
        {
            Assert.IsNotNull(_panelRenderer);
        }

        public void InitializeInputs()
        {
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }

        private void OnUIReload(PanelRenderer panelRenderer, VisualElement rootElement)
        {
            _root = rootElement;

            RegisterAction(_doubleClickAction, OnDoubleClick);
            RegisterAction(_longPressAction, OnLongPressStarted);
            RegisterAction(_longPressAction, OnLongPressCancelled, CallbackType.Cancelled);
            RegisterAction(_pointAction, OnPoint);
        }

        private void OnDisable()
        {
            UnregisterAction(_doubleClickAction, OnDoubleClick);
            UnregisterAction(_longPressAction, OnLongPressStarted);
            UnregisterAction(_longPressAction, OnLongPressCancelled, CallbackType.Cancelled);
            UnregisterAction(_pointAction, OnPoint);
        }

        private void RegisterAction(InputActionReference actionRef, Action<InputAction.CallbackContext> callback, CallbackType callbackType = CallbackType.Performed)
        {
            if (actionRef == null) return;
            actionRef.action.Enable();

            switch (callbackType)
            {
                case CallbackType.Started: actionRef.action.started += callback; break;
                case CallbackType.Cancelled: actionRef.action.canceled += callback; break;
                case CallbackType.Performed: actionRef.action.performed += callback; break;
            }
        }

        private void UnregisterAction(InputActionReference actionRef, Action<InputAction.CallbackContext> callback, CallbackType callbackType = CallbackType.Performed)
        {
            if (actionRef == null) return;

            switch (callbackType)
            {
                case CallbackType.Started: actionRef.action.started -= callback; break;
                case CallbackType.Cancelled: actionRef.action.canceled -= callback; break;
                case CallbackType.Performed: actionRef.action.performed -= callback; break;
            }

            actionRef.action.Disable();
        }

        private void OnDoubleClick(InputAction.CallbackContext ctx)
        {
            SendCustomUIEvent(DoubleClickEvent.GetPooled());
        }

        private void OnLongPressStarted(InputAction.CallbackContext ctx)
        {
            _isDragging = true;
            Vector2 screenPos = GetCurrentInputPosition();
            SendCustomUIEvent(LongPressEventStarted.GetPooled(screenPos));
        }

        private void OnLongPressCancelled(InputAction.CallbackContext ctx)
        {
            if (_isDragging)
            {
                _isDragging = false;
                Vector2 screenPos = GetCurrentInputPosition();
                SendCustomUIEvent(LongPressEventEnded.GetPooled(screenPos));
            }
        }

        private void OnPoint(InputAction.CallbackContext context)
        {
            if (_isDragging)
            {
                Vector2 screenPos = GetCurrentInputPosition();
                SendCustomUIEvent(DragEvent.GetPooled(screenPos));
            }
        }

        private Vector2 GetCurrentInputPosition()
        {
            if (Pointer.current != null)
            {
                return Pointer.current.position.ReadValue();
            }

            return Vector2.zero;
        }

        private void SendCustomUIEvent<TEvent>(TEvent evt) where TEvent : EventBase<TEvent>, new()
        {
            if (_root == null || _root.panel == null) return;

            Vector2 screenMousePosition = Pointer.current.position.ReadValue();

            Vector2 panelMousePosition = RuntimePanelUtils.ScreenToPanel(
                _root.panel,
                new Vector2(screenMousePosition.x, Screen.height - screenMousePosition.y)
            );

            VisualElement targetElement = _root.panel.Pick(panelMousePosition);
            if (targetElement != null)
            {
                evt.target = targetElement;
                targetElement.SendEvent(evt);
            }
        }
    }
}
