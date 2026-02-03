namespace Sorter.Services.FilePathProvider;

public interface IFilePathProvider
{
    string InputFilePath { get; init; }
    string OutputFilePath { get; init; }
    string ChunksDirectory { get; init; }
}
