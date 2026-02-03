using Microsoft.Extensions.DependencyInjection;
using Sorter.Services;

var services = new ServiceCollection();
services.AddScoped<IFileSorter, FileSorter>();
var app = services.BuildServiceProvider().GetRequiredService<IFileSorter>();


Console.WriteLine("Specify file chunk size (in MB, 8 MB max): ");
string memoryLimitInput = Console.ReadLine() ?? "8";
if (!int.TryParse(memoryLimitInput, out int memoryLimit))
{
    Console.WriteLine("Invalid memory limit input. Using default of 8 MB.");
    memoryLimit = 8;
}

Console.WriteLine("Sorting file from path: Common/Files/input.txt");
await app.StartAsync(memoryLimit);
Console.WriteLine("Finish");