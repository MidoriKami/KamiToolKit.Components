using System.Linq;
using KamiToolKit.Components.ListItemNodes;
using KamiToolKit.Extensions;
using Lumina.Excel.Sheets;
using Lumina.Text.ReadOnly;

namespace KamiToolKit.Components.Search;

/// <summary>
/// Implementation of <see cref="AbstractSearchAddon{T,TU}"/> for use with TerritoryType lumina row entries.
/// </summary>
public class TerritoryTypeSearchAddon : AbstractSearchAddon<TerritoryType, TerritoryTypeListItemNode> {
    protected override void OnSearchInputReceived(ReadOnlySeString searchString) {
        var searchRegex = searchString.AsRegex();

        ResultsListNode?.OptionsList = OptionsList.Where(option => {
            var placeName = option.PlaceName.ValueNullable?.Name.ToString();
            if (placeName is not null && searchRegex.IsMatch(placeName)) return true;

            if (option.ContentFinderCondition.RowId is not 0) {
                var dutyName = option.ContentFinderCondition.ValueNullable?.Name.ToString();
                if (dutyName is not null && searchRegex.IsMatch(dutyName)) return true;
            }

            return false;
        }).ToList();

        base.OnSearchInputReceived(searchString);
    }
}
