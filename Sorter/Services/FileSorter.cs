using Sorter.Models;
using Sorter.Services.ChunkProcessing;
using Sorter.Services.FilePathProvider;

namespace Sorter.Services;

public class FileSorter(IChunkProcessor chunkProcessor, IFilePathProvider filePathProvider) : IFileSorter
{
    public async Task StartAsync(int memoryLimit)
    {
        Console.WriteLine(Environment.CurrentDirectory);
        
        Directory.CreateDirectory(filePathProvider.ChunksDirectory);
        long chunkSize = memoryLimit * 1024 * 1024; // Convert to MB

        var tempChunks = await chunkProcessor.ProcessChunksAsync(
            filePathProvider.InputFilePath, 
            chunkSize, 
            filePathProvider.ChunksDirectory);

        await MergeChunksAsync(tempChunks, filePathProvider.OutputFilePath);
    }

    /// <summary>
    /// Merge sorted chunks. Limited by number of files opened simultaneously
    /// </summary>
    /// <param name="tempFiles"></param>
    /// <param name="outputFilePath"></param>
    private static async Task MergeChunksAsync(List<string> tempFiles, string outputFilePath)
    {
        var readers = tempFiles.Select(file => new StreamReader(file)).ToArray();
        var minHeap = new PriorityQueue<Line, Line>();
        await using var outputWriter = new StreamWriter(outputFilePath);
        try
        {
            for (int i = 0; i < readers.Length; i++)
            {
                string line = await readers[i].ReadLineAsync();
                if (line != null)
                {
                    var lineStruct = new Line(i, line);
                    minHeap.Enqueue(lineStruct, lineStruct);
                }
            }

            while (minHeap.Count > 0)
            {
                var smallestLine = minHeap.Dequeue();
                await outputWriter.WriteLineAsync(smallestLine.OriginalLine);

                string nextLine = await readers[smallestLine.FileIndex].ReadLineAsync();
                if (nextLine != null)
                {
                    var lineStruct = new Line(smallestLine.FileIndex, nextLine);
                    minHeap.Enqueue(lineStruct, lineStruct);
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