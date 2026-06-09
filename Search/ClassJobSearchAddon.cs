using System.Linq;
using KamiToolKit.Components.ListItemNodes;
using KamiToolKit.Extensions;
using Lumina.Excel.Sheets;
using Lumina.Text.ReadOnly;

namespace KamiToolKit.Components.Search;

/// <summary>
/// Implementation of <see cref="AbstractSearchAddon{T,TU}"/> to implement a ClassJob search.
/// </summary>
public class ClassJobSearchAddon : AbstractSearchAddon<ClassJob, ClassJobListItemNode> {
    protected override void OnSearchInputReceived(ReadOnlySeString searchString) {
        var searchRegex = searchString.AsRegex();

        ResultsListNode?.OptionsList = OptionsList.Where(option => {
            if (searchRegex.IsMatch(option.Name.ToString())) return true;
            if (searchRegex.IsMatch(option.Abbreviation.ToString())) return true;

            return false;
        }).ToList();

        base.OnSearchInputReceived(searchString);
    }
}
