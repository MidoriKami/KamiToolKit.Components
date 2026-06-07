using System.Numerics;
using KamiToolKit.Interfaces;
using KamiToolKit.Nodes;

namespace KamiToolKit.Components.ListNodes;

/// <summary>
/// Implementation of a <see cref="ListItemNode{T}"/> that represents a string.
/// </summary>
public class StringListItemNode : ListItemWithFocusNav<string>, IListItemNode {

    /// <inheritdoc/>
    public static float ItemHeight => 24.0f;

    /// <summary>
    /// The text node used to display this string.
    /// </summary>
    protected TextNode LabelTextNode { get; }

    /// <inheritdoc/>
    protected override void SetNodeData(string itemData)
        => LabelTextNode.String = itemData;

    public StringListItemNode() {
        LabelTextNode = new TextNode();
        LabelTextNode.AttachNode(this);
    }

    protected override void OnSizeChanged() {
        base.OnSizeChanged();

        LabelTextNode.Size = new Vector2(Width - 8.0f, Height);
        LabelTextNode.Position = new Vector2(8.0f, 0.0f);
    }
}
