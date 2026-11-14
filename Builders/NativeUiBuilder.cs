using System;
using System.Numerics;
using KamiToolKit.Nodes;

namespace KamiToolKit.Components.Builders;

/// <summary>
/// Static class containing various node builders for building UI layouts easier.
/// </summary>
public static class NativeUiBuilder {
    /// <summary>
    /// Creates a new root vertical list node.
    /// </summary>
    public static VerticalListNode Create(Action<NodeBuilder> buildAction)
        => Create(default, default, buildAction);

    /// <summary>
    /// Creates a new root vertical list node with the specified size.
    /// </summary>
    public static VerticalListNode Create(Vector2 size, Action<NodeBuilder> buildAction)
        => Create(size, default, buildAction);

    /// <summary>
    /// Creates a new root vertical list node with the specified size and position.
    /// </summary>
    public static VerticalListNode Create(Vector2 size, Vector2 position, Action<NodeBuilder> buildAction) {
        var builder = new NodeBuilder {
            Size = size,
            Position = position,
        };

        buildAction.Invoke(builder);
        return builder.Build();
    }
}
