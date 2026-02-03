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
Job-JLJALU : .NET 9.0.11 (9.0.1125.51716), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1

| Method                   | ChunkSizeMb | Mean       | Error    | StdDev   | Ratio | RatioSD | Rank | Gen0        | Gen1       | Gen2      | Allocated | Alloc Ratio |
|------------------------- |------------ |-----------:|---------:|---------:|------:|--------:|-----:|------------:|-----------:|----------:|----------:|------------:|
| ParallelChunkProcessor   | 1           |   694.9 ms | 14.49 ms | 42.26 ms |  0.62 |    0.05 |    1 | 218000.0000 |  9000.0000 | 2000.0000 |   2.55 GB |        1.11 |
| SequentialChunkProcessor | 1           | 1,122.3 ms | 22.41 ms | 58.25 ms |  1.00 |    0.07 |    2 | 200000.0000 | 13000.0000 | 7000.0000 |    2.3 GB |        1.00 |
