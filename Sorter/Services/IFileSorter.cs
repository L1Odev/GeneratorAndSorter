namespace Sorter.Services;

public interface IFileSorter
{
    Task StartAsync(int chunkSizeLimit);
}