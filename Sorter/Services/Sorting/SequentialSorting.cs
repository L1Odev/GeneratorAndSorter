using Sorter.Utility;

namespace Sorter.Services.Sorting;

public class SequentialSorting : ISortingStrategy
{
    public IReadOnlyCollection<string> Sort(List<string> lines)
    {
        lines.Sort(new SortComparer());
        
        return lines;
    }
}