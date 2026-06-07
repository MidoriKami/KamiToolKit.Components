using System.Linq;
using System.Text.RegularExpressions;
using KamiToolKit.Components.Search.Nodes;
using Lumina.Excel.Sheets;
using Lumina.Text.ReadOnly;

namespace KamiToolKit.Components.Search;

/// <summary>
/// Specialization of <see cref="AbstractSearchAddon{T,TU}"/> that implements the results
/// with icons and item information.
/// </summary>
public class ItemSearchAddon : AbstractSearchAddon<Item, ItemListItemNode> {
    protected override void OnSearchInputReceived(ReadOnlySeString searchString) {
        var searchRegex = new Regex(searchString.ToString(),  RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        ResultsListNode?.OptionsList = OptionsList.Where(option => {
            if (searchRegex.IsMatch(option.Name.ToString())) return true;

            var uiCategory = option.ItemUICategory.ValueNullable;
            if (uiCategory is not null && searchRegex.IsMatch(uiCategory.Value.Name.ToString())) return true;

            return false;
        }).ToList();

        base.OnSearchInputReceived(searchString);
    }
}
