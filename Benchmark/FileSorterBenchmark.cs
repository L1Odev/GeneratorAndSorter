using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Sorter.Services;
using Sorter.Services.ChunkProcessing;

namespace Benchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class FileSorterBenchmark
{
    private FileSorter _parallelSorter = null!;
    private FileSorter _sequentialSorter = null!;
    
    [Params(1, 4, 8)]
    public int ChunkSizeMb { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _parallelSorter = new FileSorter(new ParallelChunkProcessor());
        _sequentialSorter = new FileSorter(new SequentialChunkProcessor());
        
        var chunkFolder = Path.Combine("../../../../", "Common", "Chunks");
        Directory.CreateDirectory(chunkFolder);
    }

    [IterationCleanup]
    public void Cleanup()
    {
        var chunkFolder = Path.Combine("../../../../", "Common", "Chunks");
        if (!Directory.Exists(chunkFolder)) return;
        
        foreach (var file in Directory.GetFiles(chunkFolder, "chunk_*.txt"))
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
