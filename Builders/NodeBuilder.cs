using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Textures.TextureWraps;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.BaseTypes;
using KamiToolKit.Enums;
using KamiToolKit.Interfaces;
using KamiToolKit.Nodes;
using Lumina.Text.ReadOnly;


namespace KamiToolKit.Components.Builders;

/// <summary>
/// Aliased <see cref="NodeBuilder{T}"/> with a backing <see cref="VerticalListNode"/> as this is the most basic form of ui building.
/// </summary>
public class NodeBuilder : NodeBuilder<VerticalListNode> {

    /// <summary>
    /// Gets or sets the inner vertical list node's size.
    /// </summary>
    public Vector2 Size {
        get => Node.Size;
        set => Node.Size = value;
    }

    /// <summary>
    /// Gets or sets the inner vertical list node's position.
    /// </summary>
    public Vector2 Position {
        get => Node.Position;
        set => Node.Position = value;
    }

    /// <summary>
    /// Gets or sets the inner vertical list node's fit contents setting.
    /// </summary>
    public bool FitContents {
        get => Node.FitContents;
        set => Node.FitContents = value;
    }

    /// <summary>
    /// Gets or sets the inner vertical list node's first item spacing.
    /// </summary>
    public float FirstItemSpacing {
        get => Node.FirstItemSpacing;
        set => Node.FirstItemSpacing = value;
    }

    /// <summary>
    /// Gets or sets the inner vertical list node's item spacing.
    /// </summary>
    public float ItemSpacing {
        get => Node.ItemSpacing;
        set => Node.ItemSpacing = value;
    }

    /// <summary>
    /// Constructor for a NodeBuilder.
    /// </summary>
    public NodeBuilder() {
        Node.StretchWidth = true;
    }
}

/// <summary>
/// Abstract base class for node builders, implementing commonly used nodes.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class NodeBuilder<T> where T : NodeBase, new() {

    /// <summary>
    /// Built node instance for inheritors to use.
    /// </summary>
    protected readonly T Node = new();

    /// <summary>
    /// List of child nodes to build with.
    /// </summary>
    protected readonly List<NodeBase> ChildNodes = [];

    /// <summary>
    /// Builds the resulting node.
    /// </summary>
    public T Build() {
        if (Node is ILayoutNode layoutNode) {
            layoutNode.InitialNodes = [.. ChildNodes];
        }

        return Node;
    }

    /// <summary>
    /// Adds a predefined node to this builder.
    /// </summary>
    public NodeBuilder<T> AddNode(NodeBase node) {
        ChildNodes.Add(node);

        return this;
    }

    /// <summary>
    /// Adds a spacer with the specified size.
    /// </summary>
    public NodeBuilder<T> Dummy(float size)
        => AddNode(new ResNode { Size = new Vector2(size, size) });

    /// <summary>
    /// Adds a spacer with the specified size.
    /// </summary>
    public NodeBuilder<T> Spacer(float size)
        => Dummy(size);

    /// <summary>
    /// Adds a HorizontalLineNode.
    /// </summary>
    public NodeBuilder<T> Separator()
        => AddNode(new HorizontalLineNode { Height = 4.0f });

    /// <summary>
    /// Adds a <see cref="CategoryTextNode"/> with label text.
    /// </summary>
    public NodeBuilder<T> CategoryLabel(ReadOnlySeString label, out CategoryTextNode node, Action<ICategoryTextNode>? configure = null) {
        node = new CategoryTextNode {
            Size = new Vector2(100.0f, 16.0f),
            String = label,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="CategoryTextNode"/> with label text.
    /// </summary>
    public NodeBuilder<T> CategoryLabel(ReadOnlySeString label, Action<ICategoryTextNode>? configure = null)
        => CategoryLabel(label, out _, configure);

    /// <summary>
    /// Adds a <see cref="TextNode"/> with label text.
    /// </summary>
    public NodeBuilder<T> Text(ReadOnlySeString label, out TextNode node, Action<ITextNode>? configure = null) {
        node = new TextNode {
            Size = new Vector2(100.0f, 24.0f),
            String = label,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="TextNode"/> with label text.
    /// </summary>
    public NodeBuilder<T> Text(ReadOnlySeString label, Action<ITextNode>? configure = null)
        => Text(label, out _, configure);

    /// <summary>
    /// Adds a centered <see cref="TextNode"/> with label text.
    /// </summary>
    public NodeBuilder<T> TextCentered(ReadOnlySeString label, out TextNode node, Action<ITextNode>? configure = null)
        => Text(label, out node, text => {
            text.AlignmentType = AlignmentType.Center;
            configure?.Invoke(text);
        });

    /// <summary>
    /// Adds a centered <see cref="TextNode"/> with label text.
    /// </summary>
    public NodeBuilder<T> TextCentered(ReadOnlySeString label, Action<ITextNode>? configure = null)
        => TextCentered(label, out _, configure);

    /// <summary>
    /// Adds a centered <see cref="TextNode"/> with label text and a specified font size.
    /// </summary>
    public NodeBuilder<T> TextCentered(ReadOnlySeString label, uint fontSize, out TextNode node, Action<ITextNode>? configure = null)
        => Text(label, out node, text => {
            text.AlignmentType = AlignmentType.Center;
            text.FontSize = fontSize;
            configure?.Invoke(text);
        });

    /// <summary>
    /// Adds a centered <see cref="TextNode"/> with label text and a specified font size.
    /// </summary>
    public NodeBuilder<T> TextCentered(ReadOnlySeString label, uint fontSize, Action<ITextNode>? configure = null)
        => TextCentered(label, fontSize, out _, configure);

    /// <summary>
    /// Adds a <see cref="TextNode"/> with label text.
    /// With params to wordwrap.
    /// </summary>
    public NodeBuilder<T> TextWrapped(ReadOnlySeString label, out TextNode node, Action<ITextNode>? configure = null) {
        node = new TextNode {
            Size = new Vector2(100.0f, 24.0f),
            TextFlags = TextFlags.WordWrap | TextFlags.MultiLine,
            LineSpacing = 14,
            String = label,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="TextNode"/> with label text.
    /// With params to wordwrap.
    /// </summary>
    public NodeBuilder<T> TextWrapped(ReadOnlySeString label, Action<ITextNode>? configure = null)
        => TextWrapped(label, out _, configure);

    /// <summary>
    /// Adds a <see cref="CheckboxNode"/>.
    /// </summary>
    public NodeBuilder<T> Checkbox(ReadOnlySeString label, bool value, Action<bool> onChecked, out CheckboxNode node, Action<ICheckboxNode>? configure = null) {
        node = new CheckboxNode {
            Size = new Vector2(100.0f, 24.0f),
            String = label,
            IsChecked = value,
            OnChecked = onChecked,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="CheckboxNode"/>.
    /// </summary>
    public NodeBuilder<T> Checkbox(ReadOnlySeString label, bool value, Action<bool> onChecked, Action<ICheckboxNode>? configure = null)
        => Checkbox(label, value, onChecked, out _, configure);

    /// <summary>
    /// Adds a <see cref="TextButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> TextButton(ReadOnlySeString label, Action onClicked, out TextButtonNode node, Action<ITextButtonNode>? configure = null) {
        node = new TextButtonNode {
            Size = new Vector2(100.0f, 24.0f),
            String = label,
            OnClick = onClicked,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="TextButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> TextButton(ReadOnlySeString label, Action onClicked, Action<ITextButtonNode>? configure = null)
        => TextButton(label, onClicked, out _, configure);

    /// <summary>
    /// Adds a <see cref="CircleButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> CircleButton(CircleButtonIcon icon, Action onClicked, out CircleButtonNode node, Action<ICircleButtonNode>? configure = null) {
        node = new CircleButtonNode {
            Size = new Vector2(32.0f, 32.0f),
            Icon = icon,
            OnClick = onClicked,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="CircleButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> CircleButton(CircleButtonIcon icon, Action onClicked, Action<ICircleButtonNode>? configure = null)
        => CircleButton(icon, onClicked, out _, configure);

    /// <summary>
    /// Adds a <see cref="IconButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> IconButton(uint iconId, Action onClicked, out IconButtonNode node, Action<IIconButtonNode>? configure = null) {
        node = new IconButtonNode {
            Size = new Vector2(32.0f, 32.0f),
            IconId = iconId,
            OnClick = onClicked,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="IconButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> IconButton(uint iconId, Action onClicked, Action<IIconButtonNode>? configure = null)
        => IconButton(iconId, onClicked, out _, configure);

    /// <summary>
    /// Adds a <see cref="ImGuiIconButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> ImGuiButton(string filePath, Action onClicked, out ImGuiIconButtonNode node, Action<IImGuiIconButtonNode>? configure = null) {
        node = new ImGuiIconButtonNode {
            Size = new Vector2(32.0f, 32.0f),
            TexturePath = filePath,
            OnClick = onClicked,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="ImGuiIconButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> ImGuiButton(string filePath, Action onClicked, Action<IImGuiIconButtonNode>? configure = null)
        => ImGuiButton(filePath, onClicked, out _, configure);

    /// <summary>
    /// Adds a <see cref="ImGuiIconButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> ImGuiButton(IDalamudTextureWrap texture, Action onClicked, out ImGuiIconButtonNode node, Action<IImGuiIconButtonNode>? configure = null) {
        node = new ImGuiIconButtonNode {
            Size = new Vector2(32.0f, 32.0f),
            OnClick = onClicked,
        };

        node.LoadTexture(texture);

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="ImGuiIconButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> ImGuiButton(IDalamudTextureWrap texture, Action onClicked, Action<IImGuiIconButtonNode>? configure = null)
        => ImGuiButton(texture, onClicked, out _, configure);

    /// <summary>
    /// Adds a <see cref="TextureButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> TextureButton(string texturePath, Vector2 textureCoordinates, Vector2 partSize, Action onClicked, out TextureButtonNode node, Action<ITextureButtonNode>? configure = null) {
        node = new TextureButtonNode {
            Size = new Vector2(32.0f, 32.0f),
            TexturePath = texturePath,
            TextureCoordinates = textureCoordinates,
            TextureSize = partSize,
            OnClick = onClicked,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="TextureButtonNode"/>.
    /// </summary>
    public NodeBuilder<T> TextureButton(string texturePath, Vector2 textureCoordinates, Vector2 partSize, Action onClicked, Action<ITextureButtonNode>? configure = null)
        => TextureButton(texturePath, textureCoordinates, partSize, onClicked, out _, configure);

    /// <summary>
    /// Adds a <see cref="DropDownNode{TV}"/>.
    /// </summary>
    public NodeBuilder<T> Combo<TU>(List<TU> options, TU initialValue, Action<TU> onOptionSelected, Func<TU, ReadOnlySeString> labelDefinition, out DropDownNode<TU> node, Action<IDropDownNode<TU>>? configure = null) {
        node = new DropDownNode<TU> {
            Size = new Vector2(100.0f, 24.0f),
            GetLabelFunction = labelDefinition,
            OnOptionSelected = onOptionSelected,
            MaxListOptions = options.Count > 15 ? 15 : 5,
            Options = options,
            SelectedOption = initialValue,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="DropDownNode{T}"/>.
    /// </summary>
    public NodeBuilder<T> Combo<TU>(List<TU> options, TU initialValue, Action<TU> onOptionSelected, Func<TU, ReadOnlySeString> labelDefinition, Action<IDropDownNode<TU>>? configure = null)
        => Combo(options, initialValue, onOptionSelected, labelDefinition, out _, configure);

    /// <summary>
    /// Adds a <see cref="DropDownNode{T}"/> predefined as an enum DropDownNode.
    /// </summary>
    public NodeBuilder<T> Combo<TU>(TU initialValue, Action<TU> onOptionSelected, out DropDownNode<TU> node, Action<IDropDownNode<TU>>? configure = null) where TU : struct, Enum
        => Combo([.. Enum.GetValues<TU>()], initialValue, onOptionSelected, value => value.ToString(), out node, configure);

    /// <summary>
    /// Adds a <see cref="DropDownNode{T}"/> predefined as an enum DropDownNode.
    /// </summary>
    public NodeBuilder<T> Combo<TU>(TU initialValue, Action<TU> onOptionSelected, Action<IDropDownNode<TU>>? configure = null) where TU : struct, Enum
        => Combo(initialValue, onOptionSelected, out _, configure);

    /// <summary>
    /// Adds a <see cref="DropDownNode{T}"/> predefined as a string DropDownNode.
    /// </summary>
    public NodeBuilder<T> Combo(List<ReadOnlySeString> options, ReadOnlySeString initialValue, Action<ReadOnlySeString> onOptionSelected, out DropDownNode<ReadOnlySeString> builtNode, Action<IDropDownNode<ReadOnlySeString>>? configure = null)
        => Combo(options, initialValue, onOptionSelected, value => value, out builtNode, configure);

    /// <summary>
    /// Adds a <see cref="DropDownNode{T}"/> predefined as a string DropDownNode.
    /// </summary>
    public NodeBuilder<T> Combo(List<ReadOnlySeString> options, ReadOnlySeString initialValue, Action<ReadOnlySeString> onOptionSelected, Func<ReadOnlySeString, ReadOnlySeString> labelDefinition, Action<IDropDownNode<ReadOnlySeString>>? configure = null)
        => Combo(options, initialValue, onOptionSelected, labelDefinition, out _, configure);

    /// <summary>
    /// Adds a <see cref="NumericInputNode"/>.
    /// </summary>
    public NodeBuilder<T> InputInt(int value, int min, int max, Action<int> onValueUpdate, out NumericInputNode node, Action<INumericInputNode>? configure = null) {
        node = new NumericInputNode {
            Size = new Vector2(100.0f, 24.0f),
            Range = min..max,
            Value = value,
            OnValueUpdate = onValueUpdate,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="NumericInputNode"/>.
    /// </summary>
    public NodeBuilder<T> InputInt(int value, int min, int max, Action<int> onValueUpdate, Action<INumericInputNode>? configure = null)
        =>  InputInt(value, min, max, onValueUpdate, out _, configure);

    /// <summary>
    /// Adds a <see cref="SliderNode"/>.
    /// </summary>
    public NodeBuilder<T> IntSlider(int value, int min, int max, Action<int> onValueUpdate, out SliderNode node, Action<ISliderNode<int>>? configure = null) {
        node = new SliderNode {
            Size = new Vector2(100.0f, 24.0f),
            Range = min..max,
            Value = value,
            OnValueUpdate = onValueUpdate,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="SliderNode"/>.
    /// </summary>
    public NodeBuilder<T> IntSlider(int value, int min, int max, Action<int> onValueUpdate, Action<ISliderNode<int>>? configure = null)
        =>  IntSlider(value, min, max, onValueUpdate, out _, configure);

    /// <summary>
    /// Adds a <see cref="FloatSliderNode"/>.
    /// </summary>
    public NodeBuilder<T> FloatSlider(float value, float min, float max, Action<float> onValueUpdate, out FloatSliderNode node, Action<ISliderNode<float>>? configure = null) {
        node = new FloatSliderNode {
            Size = new Vector2(100.0f, 24.0f),
            Min = min,
            Max = max,
            Value = value,
            OnValueUpdate = onValueUpdate,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="FloatSliderNode"/>.
    /// </summary>
    public NodeBuilder<T> FloatSlider(float value, float min, float max, Action<float> onValueUpdate, Action<ISliderNode<float>>? configure = null)
        =>  FloatSlider(value, min, max, onValueUpdate, out _, configure);

    /// <summary>
    /// Adds a <see cref="TextInputNode"/>.
    /// </summary>
    public NodeBuilder<T> InputText(Action<ReadOnlySeString> onInputReceived, out TextInputNode node, ReadOnlySeString? initialValue = null, Action<ITextInputNode>? configure = null) {
        node = new TextInputNode {
            Size = new Vector2(100.0f, 24.0f),
            String = initialValue ?? string.Empty,
            OnInputReceived = onInputReceived,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="TextInputNode"/>.
    /// </summary>
    public NodeBuilder<T> InputText(Action<ReadOnlySeString> onInputReceived, ReadOnlySeString? initialValue = null, Action<ITextInputNode>? configure = null)
        => InputText(onInputReceived, out _, initialValue, configure);

    /// <summary>
    /// Adds a <see cref="TextInputNode"/> with defaults for search.
    /// </summary>
    public NodeBuilder<T> Search(Action<ReadOnlySeString> onInputReceived, out SearchInputNode node, ReadOnlySeString? initialValue = null, Action<ITextInputNode>? configure = null) {
        node = new SearchInputNode {
            Size = new Vector2(100.0f, 24.0f),
            String = initialValue ?? string.Empty,
            OnInputReceived = onInputReceived,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="TextInputNode"/> with defaults for search.
    /// </summary>
    public NodeBuilder<T> Search(Action<ReadOnlySeString> onInputReceived, ReadOnlySeString? initialValue = null, Action<ITextInputNode>? configure = null)
        => Search(onInputReceived, out _, initialValue, configure);

    /// <summary>
    /// Adds a virtualized list via <see cref="ListNode{T,TU}"/>.
    /// </summary>
    public NodeBuilder<T> List<TU, TV>(List<TU> options, Action<TU> onOptionSelected, out ListNode<TU, TV> node, Action<IListNode<TU, TV>>? configure = null) where TV : ListItemNode<TU>, IListItemNode, new() {
        node = new ListNode<TU, TV> {
            LayoutFill = LayoutFill.Vertical,
            Options = options,
            OnOptionSelected = onOptionSelected,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a virtualized list via <see cref="ListNode{T,TU}"/>.
    /// </summary>
    public NodeBuilder<T> List<TU, TV>(List<TU> options, Action<TU?> onItemSelected, Action<IListNode<TU, TV>>? configure = null) where TV : ListItemNode<TU>, IListItemNode, new()
        => List(options, onItemSelected, out _, configure);

    /// <summary>
    /// Adds a virtualized list via <see cref="ListNode{T,TU}"/>.
    /// </summary>
    /// <remarks>
    /// This version is without a <see cref="ListNode{T,TU}.OnOptionSelected"/> callback, so it assumes you want the options to not be selectable.
    /// </remarks>
    public NodeBuilder<T> List<TU, TV>(List<TU> options, out ListNode<TU, TV> node, Action<IListNode<TU, TV>>? configure = null) where TV : ListItemNode<TU>, IListItemNode, new() {
        node = new ListNode<TU, TV> {
            LayoutFill = LayoutFill.Vertical,
            Options = options,
            EnableSelection = false,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a virtualized list via <see cref="ListNode{T,TU}"/>.
    /// </summary>
    /// <remarks>
    /// This version is without a <see cref="ListNode{T,TU}.OnOptionSelected"/> callback, so it assumes you want the options to not be selectable.
    /// </remarks>
    public NodeBuilder<T> List<TU, TV>(List<TU> options, Action<IListNode<TU, TV>>? configure = null) where TV : ListItemNode<TU>, IListItemNode, new()
        => List(options, out _, configure);

    /// <summary>
    /// Adds a virtualized list via <see cref="ListNode{T,TU}"/>.
    /// </summary>
    /// <remarks>
    /// This overload provides a callback for when the node itself reports that its state has changed vis ItemStateChanged(...);
    /// </remarks>
    public NodeBuilder<T> List<TU, TV>(List<TU> options, Action<TV, TU>? onItemStateChanged, out ListNode<TU, TV> node, Action<IListNode<TU, TV>>? configure = null) where TV : ListItemNode<TU>, IListItemNode, new() {
        node = new ListNode<TU, TV> {
            LayoutFill = LayoutFill.Vertical,
            Options = options,
            OnItemStateChanged = onItemStateChanged,
        };

        configure?.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a virtualized list via <see cref="ListNode{T,TU}"/>.
    /// </summary>
    /// <remarks>
    /// This overload provides a callback for when the node itself reports that its state has changed vis ItemStateChanged(...);
    /// </remarks>
    public NodeBuilder<T> List<TU, TV>(List<TU> options, Action<TV, TU>? onItemStateChanged, Action<IListNode<TU, TV>>? configure = null) where TV : ListItemNode<TU>, IListItemNode, new()
        => List(options, onItemStateChanged, out _, configure);

    /// <summary>
    /// Adds a <see cref="ScrollingNode{T}"/> with a backed VerticalListNode.
    /// </summary>
    /// <remarks>
    /// Warning, if you need to display a lot of elements use a <see cref="ListNode{T,TU}"/> instead.
    /// </remarks>
    public NodeBuilder<T> Scrolling(Action<NodeBuilder> buildInner, out ScrollingNode<VerticalListNode> node, Action<IScrollingNode<VerticalListNode>>? configure = null) {
        var innerBuilder = new NodeBuilder();
        buildInner(innerBuilder);

        var listNode = innerBuilder.Node;

        listNode.FitContents = true;
        listNode.StretchWidth = true;
        listNode.InitialNodes = [.. innerBuilder.ChildNodes];

        node = new ScrollingNode<VerticalListNode>(listNode) {
            AutoHideScrollBar = true,
        };

        configure?.Invoke(node);
        node.RecalculateLayout();

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="ScrollingNode{T}"/> with a backed VerticalListNode.
    /// </summary>
    /// <remarks>
    /// Warning, if you need to display a lot of elements use a <see cref="ListNode{T,TU}"/> instead.
    /// </remarks>
    public NodeBuilder<T> Scrolling(Action<NodeBuilder> buildInner, Action<IScrollingNode<VerticalListNode>>? configure = null)
        => Scrolling(buildInner, out _, configure);

    /// <summary>
    /// Adds a <see cref="RatioSplitNode"/> for creating segmented areas.
    /// </summary>
    public NodeBuilder<T> RatioSplit(float[] ratios, out RatioSplitNode node, Action<IRatioSplitNode>? configure = null, params Action<NodeBuilder>[] sectionBuildActions) {
        node = new RatioSplitNode {
            Size = new Vector2(100.0f, 24.0f),
            Ratios = ratios,
            ItemSpacing = 2.0f,
        };

        foreach (var buildAction in sectionBuildActions) {
            var innerBuilder = new NodeBuilder();
            buildAction(innerBuilder);

            // If we only added one node to this builder, add it directly.
            if (innerBuilder.ChildNodes.Count is 1) {
                innerBuilder.Node.Dispose();
                node.AddNode(innerBuilder.ChildNodes.First());
            }

            // else attach them all inside a vertical list.
            else if (innerBuilder.ChildNodes.Count > 1) {
                var layoutNode = innerBuilder.Node;

                layoutNode.FitContents = true;
                layoutNode.StretchWidth = true;
                layoutNode.InitialNodes = [.. innerBuilder.ChildNodes];
                node.AddNode(layoutNode);
            }

            // Something went wrong.
            else {
                innerBuilder.Node.Dispose();
            }
        }

        configure?.Invoke(node);
        node.RecalculateLayout();

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="RatioSplitNode"/> for creating segmented areas.
    /// </summary>
    public NodeBuilder<T> RatioSplit(float[] ratios, Action<IRatioSplitNode>? configure = null, params Action<NodeBuilder>[] sectionBuildActions)
        => RatioSplit(ratios, out _, configure, sectionBuildActions);

    /// <summary>
    /// Adds a <see cref="RatioSplitNode"/> for creating segmented areas.
    /// </summary>
    public NodeBuilder<T> RatioSplit(float[] ratios, params Action<NodeBuilder>[] sectionBuildActions)
        => RatioSplit(ratios, out _, null, sectionBuildActions);

    /// <summary>
    /// Adds a two-string display node for displaying a label and a updatable value.
    /// </summary>
    public NodeBuilder<T> DataRow(ReadOnlySeString label, ReadOnlySeString initialValue, out TextNode valueNode, float[]? ratios = null, Action<IRatioSplitNode>? configureSplit = null) {
        TextNode localValueNode = null!;

        RatioSplit(ratios ?? [0.5f, 0.5f],
            configureRatio => {
                configureRatio.ShowSeparators = false;
                configureSplit?.Invoke(configureRatio);
            },
            leftSplit => leftSplit.Text(label, textNode => {
                textNode.AddTextFlags(TextFlags.Ellipsis);
            }),
            rightSplit => rightSplit.Text(initialValue, out localValueNode, textNode => {
                textNode.AddTextFlags(TextFlags.Ellipsis);
            }));

        valueNode = localValueNode;
        return this;
    }

    /// <summary>
    /// Adds a standardized data row with a left label and a right value node.
    /// </summary>
    /// <remarks>
    /// This overload does not include an out TextNode for updating the value text, this probably isn't what you wanted, unless the data doesn't change.
    /// </remarks>
    public NodeBuilder<T> DataRow(ReadOnlySeString label, ReadOnlySeString initialValue, float[]? ratios = null, Action<IRatioSplitNode>? configureSplit = null)
        => DataRow(label, initialValue, out _, ratios, configureSplit);

    /// <summary>
    /// Adds a standardized split row with a left text label and a custom right-hand side component that is built with NodeBuilder.
    /// </summary>
    public NodeBuilder<T> Row(ReadOnlySeString label, Action<NodeBuilder> buildRight, float[]? ratios = null, Action<IRatioSplitNode>? configureSplit = null) {
        RatioSplit(ratios ?? [0.5f, 0.5f],
            configureRatio => {
                configureRatio.ShowSeparators = false;
                configureSplit?.Invoke(configureRatio);
            },
            leftSplit => leftSplit.Text(label, configureLabel => {
                configureLabel.AddTextFlags(TextFlags.Ellipsis);
            }),
            buildRight);

        return this;
    }

    /// <summary>
    /// Adds a <see cref="TabBarNode"/>.
    /// </summary>
    /// <remarks>
    /// Builds a standard tab bar, this does not include content nodes, if you need content areas as well use TabbedContent.
    /// </remarks>
    public NodeBuilder<T> TabBar(Action<ITabBarNode> configure, out TabBarNode node) {
        node = new TabBarNode {
            Size = new Vector2(100.0f, 24.0f),
        };

        configure.Invoke(node);

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="TabBarNode"/> with specified tab initialization actions.
    /// </summary>
    /// <remarks>
    /// Builds a standard tab bar, this does not include content nodes, if you need content areas as well use TabbedContent.
    /// </remarks>
    public NodeBuilder<T> TabBar(Action<ITabBarNode> configure)
        => TabBar(configure, out _);

    /// <summary>
    /// Adds a <see cref="TabbedContentNode"/>, a tab bar with associated content areas.
    /// </summary>
    public NodeBuilder<T> TabbedContent(Action<TabbedContentBuilder> buildTabs, out TabbedContentNode node, Action<ITabbedContentNode>? configure = null, Action? tabChanged = null) {
        node = new TabbedContentNode {
            Size = new Vector2(100.0f, 200.0f),
            OnTabChanged = tabChanged,
        };

        var contentBuilder = new TabbedContentBuilder(node);
        buildTabs(contentBuilder);

        configure?.Invoke(node);
        node.RecalculateLayout();

        return AddNode(node);
    }

    /// <summary>
    /// Adds a <see cref="TabbedContentNode"/>, a tab bar with associated content areas.
    /// </summary>
    public NodeBuilder<T> TabbedContent(Action<TabbedContentBuilder> buildTabs, Action<ITabbedContentNode>? configure = null, Action? tabChanged = null)
        => TabbedContent(buildTabs, out _, configure, tabChanged);

    /// <summary>
    /// Adds a nested <see cref="VerticalListNode"/> container.
    /// </summary>
    public NodeBuilder<T> VerticalList(Action<NodeBuilder> buildInner, out VerticalListNode node, Action<VerticalListNode>? configure = null) {
        var innerBuilder = new NodeBuilder {
            Node = {
                StretchWidth = true,
            },
        };
        buildInner(innerBuilder);

        configure?.Invoke(innerBuilder.Node);
        node = innerBuilder.Build();

        return AddNode(node);
    }

    /// <summary>
    /// Adds a nested <see cref="VerticalListNode"/> container.
    /// </summary>
    public NodeBuilder<T> VerticalList(Action<NodeBuilder> buildInner, Action<VerticalListNode>? configure = null)
        => VerticalList(buildInner, out _, configure);

    /// <summary>
    /// Adds a nested <see cref="HorizontalListNode"/> container.
    /// </summary>
    public NodeBuilder<T> HorizontalList(Action<NodeBuilder> buildInner, out HorizontalListNode node, Action<HorizontalListNode>? configure = null) {
        var innerBuilder = new NodeBuilder();
        buildInner(innerBuilder);

        node = new HorizontalListNode {
            FitHeight = true,
            InitialNodes = [.. innerBuilder.ChildNodes],
        };

        configure?.Invoke(node);
        node.RecalculateLayout();

        return AddNode(node);
    }

    /// <summary>
    /// Adds a nested <see cref="HorizontalListNode"/> container.
    /// </summary>
    public NodeBuilder<T> HorizontalList(Action<NodeBuilder> buildInner, Action<HorizontalListNode>? configure = null)
        => HorizontalList(buildInner, out _, configure);
}
