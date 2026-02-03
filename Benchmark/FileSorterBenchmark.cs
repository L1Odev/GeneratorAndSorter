using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Sorter.Services;
using Sorter.Services.ChunkProcessing;
using Sorter.Services.FilePathProvider;

namespace Benchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class FileSorterBenchmark
{
    private FileSorter _parallelSorter = null!;
    private FileSorter _sequentialSorter = null!;
    private IFilePathProvider _filePathProvider = null!;
    
    [Params(1, 4, 8)]
    public int ChunkSizeMb { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _filePathProvider = new BenchmarkFilePathProvider();
        _parallelSorter = new FileSorter(new ParallelChunkProcessor(), _filePathProvider);
        _sequentialSorter = new FileSorter(new SequentialChunkProcessor(), _filePathProvider);
        
        Directory.CreateDirectory(_filePathProvider.ChunksDirectory);
    }

    [IterationCleanup]
    public void Cleanup()
    {
        if (!Directory.Exists(_filePathProvider.ChunksDirectory)) return;
        
        foreach (var file in Directory.GetFiles(_filePathProvider.ChunksDirectory, "chunk_*.txt"))
        {
            try { File.Delete(file); }
            catch
            {
                // ignored
            }
        }
    }

    [Benchmark(Baseline = true)]
    public async Task SequentialChunkProcessor()
    {
        await _sequentialSorter.StartAsync(ChunkSizeMb);
    }

    [Benchmark]
    public async Task ParallelChunkProcessor()
    {
        await _parallelSorter.StartAsync(ChunkSizeMb);
    }
}
