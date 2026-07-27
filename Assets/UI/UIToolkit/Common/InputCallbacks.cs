using System;

#nullable enable
public record InputCallbacks(
    Action? OnClick = null,
    Action? OnDoubleClick = null
);