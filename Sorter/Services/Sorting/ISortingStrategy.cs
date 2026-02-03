namespace Sorter.Services.Sorting;

public interface ISortingStrategy
{
    IReadOnlyCollection<string> Sort(List<string> lines);
}