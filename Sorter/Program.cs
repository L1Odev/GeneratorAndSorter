using Sorter.Models;
using Sorter.Utility;

Console.WriteLine("Specify memory limit for sorting (in MB): ");
string memoryLimitInput = Console.ReadLine() ?? "512";
if (!int.TryParse(memoryLimitInput, out int memoryLimit))
{
    Console.WriteLine("Invalid memory limit input. Using default of 512 MB.");
    memoryLimit = 512;
}

Console.WriteLine("Sorting file from path: Common/Files/input.txt");


// divide large file
string inputFilePath = Path.Combine("../../../../", "Common", "Files", "input.txt");
string tempDirectory = Path.Combine("../../../../", "Common", "Chunks");
string outputFilePath = Path.Combine("../../../../", "Common", "Files", "sorted_output.txt");
long chunkSize = memoryLimit * 1024 * 1024; // Convert to MB
List<string> tempFiles = new List<string>();

using var reader = new StreamReader(inputFilePath);
while (!reader.EndOfStream)
{
    List<string> lines = [];
    long currentChunkSize = 0;

    while (!reader.EndOfStream && currentChunkSize < chunkSize)
    {
        string line = reader.ReadLine();
        if (line != null)
        {
            lines.Add(line);
            currentChunkSize += System.Text.Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;
        }
    }
    
    lines.Sort(new SortComparer());

    string tempFilePath = Path.Combine(tempDirectory, $"chunk_{tempFiles.Count}.txt");
    await File.WriteAllLinesAsync(tempFilePath, lines);
    tempFiles.Add(tempFilePath);
}

var readers = tempFiles.Select(file => new StreamReader(file)).ToArray();
var outputWriter = new StreamWriter(outputFilePath);
var minHeap = new PriorityQueue<Line, string>(new SortComparer());

for (int i = 0; i < readers.Length; i++)
{
    string line = readers[i].ReadLine();
    if (line != null)
    {
        minHeap.Enqueue(new Line(i, line), line);
    }
}

// merge sorted lines
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
outputWriter.Flush();

Console.WriteLine("Finish");