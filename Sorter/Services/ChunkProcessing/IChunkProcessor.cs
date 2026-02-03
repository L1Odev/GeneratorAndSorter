namespace Sorter.Services.ChunkProcessing;

public interface IChunkProcessor
{
    /// <summary>
    /// Divides a file into sorted chunks, saves chunks
    /// </summary>
    /// <param name="inputFilePath">Path to the input file</param>
    /// <param name="chunkSize">Maximum size of each chunk in bytes</param>
    /// <param name="tempDirectory">Directory to store temporary chunk files</param>
    /// <returns>List of paths to the created chunk files</returns>
    Task<List<string>> ProcessChunksAsync(string inputFilePath, long chunkSize, string tempDirectory);
}
