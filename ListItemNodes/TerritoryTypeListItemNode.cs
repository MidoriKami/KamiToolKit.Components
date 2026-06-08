using System.Numerics;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Classes;
using KamiToolKit.Interfaces;
using KamiToolKit.Nodes;
using KamiToolKit.Nodes.Simplified;
using Lumina.Excel.Sheets;

namespace KamiToolKit.Components.ListItemNodes;

/// <summary>
/// Implementation of <see cref="ListItemNode{T}"/> for use with <see cref="Search.TerritoryTypeSearchAddon"/>.
/// </summary>
public class TerritoryTypeListItemNode : ListItemWithFocusNav<TerritoryType>, IListItemNode {

    /// <inheritdoc/>
    public static float ItemHeight => 64.0f;

    /// <summary>
    /// Gets the image node used to represent the territory.
    /// </summary>
    protected SimpleImageNode TerritoryImageNode { get; }

    /// <summary>
    /// Gets the text node used to display the territory name.
    /// </summary>
    protected TextNode TerritoryNameTextNode { get; }

    /// <summary>
    /// Gets the text node used to display the territory intended use.
    /// </summary>
    protected TextNode TerritoryDescriptionTextNode { get; }

    /// <summary>
    /// Gets the text node used to display the territory's row id.
    /// </summary>
    protected TextNode TerritoryRowIdTextNode { get; }

    /// <inheritdoc/>
    protected override void SetNodeData(TerritoryType itemData) {
        if (itemData.RowId is 0) {
            TerritoryNameTextNode.String = "Invalid TerritoryType Row Id";
            return;
        }

        if (itemData.LoadingImage.ValueNullable?.FileName is { IsEmpty: false } filePath) {
            TerritoryImageNode.LoadTexture($"ui/loadingimage/{filePath}_hr1.tex");
            TerritoryImageNode.IsVisible = true;
        }

        TerritoryRowIdTextNode.String = itemData.RowId.ToString();
        TerritoryNameTextNode.String = itemData.PlaceName.ValueNullable?.Name.ToString() ?? string.Empty;

        if (itemData.ContentFinderCondition.RowId is 0) {
            TerritoryDescriptionTextNode.String = string.Empty;
        }
        else {
            var cfcRow = itemData.ContentFinderCondition.Value.RowId;
            TerritoryDescriptionTextNode.String = SeStringEvaluator.EvaluateFromAddon(9781, [cfcRow]);
        }
    }

    public TerritoryTypeListItemNode() {
        TerritoryImageNode = new SimpleImageNode {
            FitTexture = true,
            IsVisible = false,
        };
        TerritoryImageNode.AttachNode(this);

        TerritoryNameTextNode = new TextNode {
            TextFlags = TextFlags.Ellipsis,
            AlignmentType = AlignmentType.BottomLeft,
            String = "None Selected",
        };
        TerritoryNameTextNode.AttachNode(this);

        TerritoryDescriptionTextNode = new TextNode {
            TextFlags = TextFlags.Ellipsis,
            AlignmentType = AlignmentType.TopLeft,
            TextColor = ColorHelper.GetColor(2),
        };
        TerritoryDescriptionTextNode.AttachNode(this);

        TerritoryRowIdTextNode = new TextNode {
            AlignmentType = AlignmentType.Right,
            TextColor = ColorHelper.GetColor(3),
        };
        TerritoryRowIdTextNode.AttachNode(this);

        KamiToolKitLibrary.PluginInterface.Inject(this);
    }

    protected override void OnSizeChanged() {
        base.OnSizeChanged();

        TerritoryImageNode.Size = new Vector2((Height - 4.0f) * 1.777f, Height - 4.0f);
        TerritoryImageNode.Position = new Vector2(2.0f, 2.0f);

        TerritoryRowIdTextNode.Size = new Vector2(30.0f, 30.0f);
        TerritoryRowIdTextNode.Position = new Vector2(Width - TerritoryRowIdTextNode.Width, 0.0f);

        TerritoryNameTextNode.Size = new Vector2(Width - TerritoryImageNode.Width - 10.0f - TerritoryRowIdTextNode.Width - 4.0f, Height / 2.0f);
        TerritoryNameTextNode.Position = new Vector2(TerritoryImageNode.Bounds.Right + 8.0f, 0.0f);

        TerritoryDescriptionTextNode.Size = TerritoryNameTextNode.Size;
        TerritoryDescriptionTextNode.Position = new Vector2(TerritoryNameTextNode.Bounds.Left, Height / 2.0f);
    }

    [PluginService] private ISeStringEvaluator SeStringEvaluator { get; set; } = null!;
}
