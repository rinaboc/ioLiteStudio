using System;
using Input;
using NUnit.Framework;
using UnityEngine;
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
        [Header("UI Reference")]
        [SerializeField] private PanelRenderer _panelRenderer;

        [Header("Input Action References")]
        [SerializeField] private InputActionReference _doubleClickAction;
        [SerializeField] private InputActionReference _longPressAction;

        private VisualElement _root;

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
            RegisterAction(_longPressAction, OnLongPress);
        }

        private void OnDisable()
        {
            UnregisterAction(_doubleClickAction, OnDoubleClick);
            UnregisterAction(_longPressAction, OnLongPress);
        }

        private void RegisterAction(InputActionReference actionRef, Action<InputAction.CallbackContext> callback)
        {
            if (actionRef == null) return;
            actionRef.action.Enable();
            actionRef.action.performed += callback;
        }

        private void UnregisterAction(InputActionReference actionRef, Action<InputAction.CallbackContext> callback)
        {
            if (actionRef == null) return;
            actionRef.action.performed -= callback;
            actionRef.action.Disable();
        }

        private void OnDoubleClick(InputAction.CallbackContext ctx)
        {
            SendCustomUIEvent<DoubleClickEvent>();
        }

        private void OnLongPress(InputAction.CallbackContext ctx)
        {

        }

        private void SendCustomUIEvent<TEvent>() where TEvent : EventBase<TEvent>, new()
        {
            if (_root == null || _root.panel == null) return;

            Vector2 screenMousePosition = Mouse.current.position.ReadValue();

            Vector2 panelMousePosition = RuntimePanelUtils.ScreenToPanel(
                _root.panel,
                new Vector2(screenMousePosition.x, Screen.height - screenMousePosition.y)
            );

            VisualElement targetElement = _root.panel.Pick(panelMousePosition);
            if (targetElement != null)
            {
                using (var evt = EventBase<TEvent>.GetPooled())
                {
                    evt.target = targetElement;
                    targetElement.SendEvent(evt);
                }
            }
        }
    }
}
