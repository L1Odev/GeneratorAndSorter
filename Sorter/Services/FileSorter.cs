using Sorter.Models;
using Sorter.Services.Sorting;
using Sorter.Utility;

namespace Sorter.Services;

public class FileSorter(ISortingStrategy sortingStrategy) : IFileSorter
{
    public async Task StartAsync(int memoryLimit)
    {
        var chunkFolder = Path.Combine("../../../../", "Common", "Chunks");
        Directory.CreateDirectory(chunkFolder);
        var inputFilePath = Path.Combine("../../../../", "Common", "Files", "input.txt");
        var tempDirectory = Path.Combine("../../../../", "Common", "Chunks");
        var outputFilePath = Path.Combine("../../../../", "Common", "Files", "sorted_output.txt");
        long chunkSize = memoryLimit * 1024 * 1024; // Convert to MB
        List<string> tempFiles = new List<string>();

        await SaveSortedChunks(inputFilePath, chunkSize, tempDirectory, tempFiles);

        await MergeChunksAsync(tempFiles, outputFilePath);
    }

    /// <summary>
    /// Reads and divides large file into sorted chunks
    /// </summary>
    /// <param name="inputFilePath"></param>
    /// <param name="chunkSize"></param>
    /// <param name="tempDirectory"></param>
    /// <param name="tempFiles"></param>
    private async Task SaveSortedChunks(string inputFilePath, long chunkSize, string tempDirectory, List<string> tempFiles)
    {
        using var reader = new StreamReader(inputFilePath);
        while (!reader.EndOfStream)
        {
            List<string> lines = [];
            long currentChunkSize = 0;

            while (!reader.EndOfStream && currentChunkSize < chunkSize)
            {
                string line = await reader.ReadLineAsync();
                if (line == null) continue;
                
                lines.Add(line);
                currentChunkSize += System.Text.Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;
            }

            var sortedLines = sortingStrategy.Sort(lines);

            string tempFilePath = Path.Combine(tempDirectory, $"chunk_{tempFiles.Count}.txt");
            await File.WriteAllLinesAsync(tempFilePath, sortedLines);
            tempFiles.Add(tempFilePath);
        }
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
                string line = readers[i].ReadLine();
                if (line != null)
                {
                    minHeap.Enqueue(new Line(i, line), line);
                }
            }

            while (minHeap.Count > 0)
            {
                var smallestLine = minHeap.Dequeue();
                outputWriter.WriteLine(smallestLine.Content);

                string nextLine = readers[smallestLine.FileIndex].ReadLine();
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