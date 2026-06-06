using System.Numerics;
using KamiToolKit.Nodes;

namespace KamiToolKit.Components.Search.Nodes;

/// <summary>
/// Abstract list item node for use with implementations of <see cref="ListItemNode{T}"/>
/// that don't use any component nodes to use with navigation.
/// </summary>
public abstract class ListItemWithFocusNav<T> : ListItemNode<T> {

    /// <summary>
    /// The focus node the cursor will use for interaction.
    /// </summary>
    protected NavFocusNode NavFocusNode { get; }

    protected ListItemWithFocusNav() {
        NavFocusNode = new NavFocusNode {
            OnSelected = OnNavSelected,
        };
        NavFocusNode.AttachNode(this);
    }

    protected override void OnSizeChanged() {
        base.OnSizeChanged();

        NavFocusNode.Size = new Vector2(0.0f, 0.0f);
        NavFocusNode.Position = new Vector2(2.0f, Height / 2.0f);
    }

    public override void ProcessNav(int index, int up, int down) {
        base.ProcessNav(index, up, down);

        NavFocusNode.NavIndex = index;
        NavFocusNode.NavUp = up;
        NavFocusNode.NavDown = down;
    }

    private void OnNavSelected() {
        IsSelected = !IsSelected;
        OnClick?.Invoke(this);
    }
}
