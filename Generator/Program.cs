using System.Text;

const long maxFileSizeBytes = 8L * 1024 * 1024;
const int maxNumber = 1_000_000;
var fileFolder = Path.Combine("../../../../", "Common", "Files");
var outputPath = Path.Combine(fileFolder, "input.txt");
Directory.CreateDirectory(fileFolder);
Console.Write("File size in MB (max 8 MB): ");
var input = Console.ReadLine();

if (!int.TryParse(input, out var fileSizeMb) || fileSizeMb <= 0)
{
    Console.WriteLine("Invalid input. Please enter a positive number.");
    return;
}

var targetSizeBytes = Math.Min((long)fileSizeMb * 1024 * 1024, maxFileSizeBytes);
Console.WriteLine($"Generating file with target size: {targetSizeBytes / (1024 * 1024)} MB...");

var random = new Random();
var words = new[]
{
    "Apple", "Orange", "Banana", "Cherry", "Grape", "Lemon", "Mango", "Peach", "Plum", "Melon",
    "Something", "Nothing", "Everything", "Anything", "Text", "Line", "Word", "String", "Data", "Value",
    "Hello", "World", "Test", "Sample", "Example", "Random", "Generated", "Content", "File", "Output"
};

long bytesWritten = 0;
var lineCount = 0;

using (var writer = new StreamWriter(File.Open(outputPath, FileMode.OpenOrCreate), Encoding.UTF8))
{
    while (bytesWritten < targetSizeBytes)
    {
        var number = random.Next(1, maxNumber + 1);
        var text = GenerateRandomText(random, words);
        var line = $"{number}. {text}";
        
        writer.WriteLine(line);
        bytesWritten += Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;
        lineCount++;
        
        if (lineCount % 100_000 == 0)
        {
            Console.WriteLine($"Progress: {bytesWritten / (1024 * 1024)} MB written, {lineCount:N0} lines...");
        }
    }
}

Console.WriteLine($"Generated {lineCount:N0} lines ({bytesWritten / (1024 * 1024)} MB)");
Console.WriteLine($"File saved to: {Path.GetFullPath(outputPath)}");

static string GenerateRandomText(Random random, string[] words)
{
    var wordCount = random.Next(1, 6);
    var sb = new StringBuilder();
    
    for (var i = 0; i < wordCount; i++)
    {
        if (i > 0) sb.Append(' ');
        sb.Append(words[random.Next(words.Length)]);
    }
    
    return sb.ToString();
}
