using Sorter.Models;
using Sorter.Services.ChunkProcessing;
using Sorter.Utility;

namespace Sorter.Services;

public class FileSorter(IChunkProcessor chunkProcessor) : IFileSorter
{
    public async Task StartAsync(int memoryLimit)
    {
        var chunkFolder = Path.Combine("../../../../", "Common", "Chunks");
        Directory.CreateDirectory(chunkFolder);
        var inputFilePath = Path.Combine("../../../../", "Common", "Files", "input.txt");
        var tempDirectory = Path.Combine("../../../../", "Common", "Chunks");
        var outputFilePath = Path.Combine("../../../../", "Common", "Files", "sorted_output.txt");
        long chunkSize = memoryLimit * 1024 * 1024; // Convert to MB

        var tempChunks = await chunkProcessor.ProcessChunksAsync(inputFilePath, chunkSize, tempDirectory);

        await MergeChunksAsync(tempChunks, outputFilePath);
    }

    /// <summary>
    /// Merge sorted chunks. Limited by number of files opened simultaneously
    /// </summary>
    /// <param name="tempFiles"></param>
    /// <param name="outputFilePath"></param>
    private static async Task MergeChunksAsync(List<string> tempFiles, string outputFilePath)
    {
        var readers = tempFiles.Select(file => new StreamReader(file)).ToArray();
        var minHeap = new PriorityQueue<Line, string>(new SortComparer());
        await using var outputWriter = new StreamWriter(outputFilePath);
        try
        {
            for (int i = 0; i < readers.Length; i++)
            {
                string line = await readers[i].ReadLineAsync();
                if (line != null)
                {
                    minHeap.Enqueue(new Line(i, line), line);
                }
            }

            while (minHeap.Count > 0)
            {
                var smallestLine = minHeap.Dequeue();
                await outputWriter.WriteLineAsync(smallestLine.Content);

                string nextLine = await readers[smallestLine.FileIndex].ReadLineAsync();
                if (nextLine != null)
                {
                    minHeap.Enqueue(smallestLine with { Content = nextLine }, nextLine);
                }
            }
        }
        finally
        {
            foreach (var tempReader in readers)
            {
                tempReader.Dispose();
            }
        }
    }
}