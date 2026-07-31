using System;
using Core.Domain;
using Input;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.UIToolkit
{
    public enum DropPosition
    {
        Above, Inside, Below
    }

    public static class DropZoneHelper
    {
        public static DropPosition GetDropPosition(Vector2 pos, VisualElement targetElement)
        {
            float elementHeight = targetElement.resolvedStyle.height;
            float localY = Mathf.Abs(pos.y);

            float normalizedY = Mathf.Clamp01(localY / elementHeight);

            if (normalizedY < 0.25f)
                return DropPosition.Above;
            if (normalizedY > 0.75f)
                return DropPosition.Below;

            return DropPosition.Inside;
        }

        public static Vector2 GetLocalInputPosition(Vector2 mousePos, VisualElement targetElement)
        {
            if (targetElement != null && targetElement.panel != null)
            {
                Vector2 panelPos = RuntimePanelUtils.ScreenToPanel(targetElement.panel, new Vector2(mousePos.x, Screen.height - mousePos.y));
                Vector2 localPos = targetElement.WorldToLocal(panelPos);

                return localPos;
            }
            return Vector2.zero;
        }

        public static DropPosition GetLocalDropPosition(Vector2 mousePos, VisualElement targetElement)
        {
            var localPos = GetLocalInputPosition(mousePos, targetElement);
            return GetDropPosition(localPos, targetElement);
        }
    }
}
