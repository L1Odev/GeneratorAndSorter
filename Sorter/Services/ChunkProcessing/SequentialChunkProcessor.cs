using Sorter.Models;

namespace Sorter.Services.ChunkProcessing;

public class SequentialChunkProcessor : IChunkProcessor
{
    public async Task<List<string>> ProcessChunksAsync(string inputFilePath, long chunkSize, string tempDirectory)
    {
        var tempFiles = new List<string>();

        using var reader = new StreamReader(inputFilePath);
        while (!reader.EndOfStream)
        {
            List<Line> lines = [];
            long currentChunkSize = 0;

            while (!reader.EndOfStream && currentChunkSize < chunkSize)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null) continue;

                lines.Add(new Line(tempFiles.Count, line));
                currentChunkSize += System.Text.Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;
            }

            lines.Sort();

            string tempFilePath = Path.Combine(tempDirectory, $"chunk_{tempFiles.Count}.txt");
            await File.WriteAllLinesAsync(tempFilePath, lines.Select(l => l.OriginalLine));
            tempFiles.Add(tempFilePath);
        }

        return tempFiles;
    }
}
