using Sorter.Utility;

namespace Sorter.Services.ChunkProcessing;

public class SequentialChunkProcessor : IChunkProcessor
{
    public async Task<List<string>> ProcessChunksAsync(string inputFilePath, long chunkSize, string tempDirectory)
    {
        var tempFiles = new List<string>();

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

            lines.Sort(new SortComparer());

            string tempFilePath = Path.Combine(tempDirectory, $"chunk_{tempFiles.Count}.txt");
            await File.WriteAllLinesAsync(tempFilePath, lines);
            tempFiles.Add(tempFilePath);
        }

        return tempFiles;
    }
}
