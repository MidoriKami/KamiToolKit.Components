using System.Linq;
using System.Text.RegularExpressions;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using KamiToolKit.Components.ListNodes;
using Lumina.Text.ReadOnly;

namespace KamiToolKit.Components.Search;

/// <summary>
/// Implementation of <see cref="AbstractSearchAddon{T,TU}"/> for use with Gearsets.
/// </summary>
public class GearsetSearchAddon : AbstractSearchAddon<RaptureGearsetModule.GearsetEntry, GearsetListItemNode> {
    protected override void OnSearchInputReceived(ReadOnlySeString searchString) {
        var searchRegex = new Regex(searchString.ToString(),  RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        ResultsListNode?.OptionsList = OptionsList.Where(option => searchRegex.IsMatch(option.NameString)).ToList();

        base.OnSearchInputReceived(searchString);
    }
}
