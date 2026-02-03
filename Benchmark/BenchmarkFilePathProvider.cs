using Sorter.Services.FilePathProvider;

namespace Benchmark;

public class BenchmarkFilePathProvider : IFilePathProvider
{
    public BenchmarkFilePathProvider()
    {
        var basePath = Path.Combine("../../../../../../../../", "Common");
        InputFilePath = Path.Combine(basePath, "Files", "input.txt");
        OutputFilePath = Path.Combine(basePath, "Files", "sorted_output.txt");
        ChunksDirectory = Path.Combine(basePath, "Chunks");
    }

    public string InputFilePath { get; init; }
    
    public string OutputFilePath { get; init; }

    public string ChunksDirectory { get; init; }
}
