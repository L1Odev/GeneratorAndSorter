using Sorter.Utility;

namespace Sorter.Services.Sorting;

public class ParallelSorting : ISortingStrategy
{
    public IReadOnlyCollection<string> Sort(List<string> lines)
    {
        return lines
            .AsParallel()
            .OrderBy(line => line, new SortComparer())
            .ToList();
    }
}