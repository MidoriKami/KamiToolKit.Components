using System.Linq;
using System.Text.RegularExpressions;
using KamiToolKit.Components.ListNodes;
using Lumina.Text.ReadOnly;

namespace KamiToolKit.Components.Search;

/// <summary>
/// Specialization of <see cref="AbstractSearchAddon{T,TU}"/> that implements the results as a simple string.
/// </summary>
public class StringSearchAddon : AbstractSearchAddon<string, StringListItemNode> {
    protected override void OnSearchInputReceived(ReadOnlySeString searchString) {
        var searchRegex = new Regex(searchString.ToString(),  RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        ResultsListNode?.OptionsList = OptionsList.Where(option => searchRegex.IsMatch(option)).ToList();

        base.OnSearchInputReceived(searchString);
    }
}
