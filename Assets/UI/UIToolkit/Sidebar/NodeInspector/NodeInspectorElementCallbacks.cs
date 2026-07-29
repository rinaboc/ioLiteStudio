using System;

namespace UI.UIToolkit
{
#nullable enable
    public record NodeInspectorElementCallbacks(
        Action? OnClick = null,
        Action? OnDoubleClick = null,
        Action<string>? OnSubmit = null,
        Action? OnVisibilityToggled = null,
        Action? OnFolderHandleToggled = null
    ) : InputCallbacks(OnClick, OnDoubleClick);
}