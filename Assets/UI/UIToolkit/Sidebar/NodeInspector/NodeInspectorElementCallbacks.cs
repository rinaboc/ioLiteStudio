using System;

namespace UI.UIToolkit
{
#nullable enable
    public record NodeInspectorElementCallbacks(
        Action? OnClick = null,
        Action? OnDoubleClick = null,
        Action<string>? OnSubmit = null,
        Action? OnVisibilityToggled = null
    ) : InputCallbacks(OnClick, OnDoubleClick);
}