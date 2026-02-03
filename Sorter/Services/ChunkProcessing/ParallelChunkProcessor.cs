using System.Collections.Concurrent;
using System.Threading.Channels;
using Sorter.Utility;

namespace Sorter.Services.ChunkProcessing;

public class ParallelChunkProcessor : IChunkProcessor
{
    public async Task<List<string>> ProcessChunksAsync(string inputFilePath, long chunkSize, string tempDirectory)
    {
        var channel = Channel.CreateBounded<(List<string> Lines, int ChunkIndex)>(
            new BoundedChannelOptions(Environment.ProcessorCount)
            {
                FullMode = BoundedChannelFullMode.Wait
            });
        
        var tempFiles = new ConcurrentBag<string>();
        await ProduceChunksAsync(inputFilePath, chunkSize, channel.Writer);

        var consumerTasks = Enumerable
            .Range(0, Environment.ProcessorCount)
            .Select(_ => ConsumeChunksAsync(channel.Reader, tempDirectory, tempFiles))
            .ToArray();
        
        await Task.WhenAll(consumerTasks);

        return tempFiles.ToList();
    }

    private static async Task ProduceChunksAsync(
        string inputFilePath,
        long chunkSize,
        ChannelWriter<(List<string> Lines, int ChunkIndex)> writer)
    {
        try
        {
            int chunkIndex = 0;
            using var reader = new StreamReader(inputFilePath);

            while (!reader.EndOfStream)
            {
                List<string> lines = [];
                long currentChunkSize = 0;

                while (!reader.EndOfStream && currentChunkSize < chunkSize)
                {
                    string? line = await reader.ReadLineAsync();
                    if (line == null) continue;

                    lines.Add(line);
                    currentChunkSize += System.Text.Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;
                }

                await writer.WriteAsync((lines, chunkIndex++));
            }
        }
        finally
        {
            writer.Complete();
        }
    }

    private async Task ConsumeChunksAsync(
        ChannelReader<(List<string> Lines, int ChunkIndex)> reader,
        string tempDirectory,
        ConcurrentBag<string> tempFiles)
    {
        await foreach (var (lines, index) in reader.ReadAllAsync())
        {
            var sortedLines = lines
                .AsParallel()
                .OrderBy(line => line, new SortComparer())
                .ToList();
            string tempFilePath = Path.Combine(tempDirectory, $"chunk_{index}.txt");
            await File.WriteAllLinesAsync(tempFilePath, sortedLines);
            tempFiles.Add(tempFilePath);
        }
    }
}
