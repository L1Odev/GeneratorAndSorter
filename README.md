# GeneratorAndSorter

The idea is to use External Sorting:

1. Divide file into sorted chunks
2. Merge sorted chunks into one sorted file using a Min-Heap (Priority Queue).


Benchmark results of sorting 8MB file divided by 8x 1MB file chunks


// * Summary *

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.7623)
Unknown processor
.NET SDK 9.0.308
[Host]     : .NET 9.0.11 (9.0.1125.51716), X64 RyuJIT AVX2
Job-TKTKJJ : .NET 9.0.11 (9.0.1125.51716), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1

| Method                   | ChunkSizeMb | Mean     | Error   | StdDev   | Ratio | RatioSD | Rank | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|------------------------- |------------ |---------:|--------:|---------:|------:|--------:|-----:|-----------:|-----------:|----------:|----------:|------------:|
| ParallelChunkProcessor   | 1           | 443.3 ms | 9.33 ms | 27.50 ms |  0.89 |    0.07 |    1 | 15000.0000 |  9000.0000 | 3000.0000 | 217.73 MB |        1.50 |
| SequentialChunkProcessor | 1           | 498.5 ms | 9.93 ms | 23.98 ms |  1.00 |    0.07 |    2 | 13000.0000 | 10000.0000 | 6000.0000 | 145.22 MB |        1.00 |
