```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.7623)
Unknown processor
.NET SDK 9.0.308
  [Host]     : .NET 9.0.11 (9.0.1125.51716), X64 RyuJIT AVX2
  Job-MHGIWO : .NET 9.0.11 (9.0.1125.51716), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
| Method                   | ChunkSizeMb | Mean | Error | Ratio | RatioSD | Rank | Alloc Ratio |
|------------------------- |------------ |-----:|------:|------:|--------:|-----:|------------:|
| SequentialChunkProcessor | 1           |   NA |    NA |     ? |       ? |    ? |           ? |
| ParallelChunkProcessor   | 1           |   NA |    NA |     ? |       ? |    ? |           ? |
|                          |             |      |       |       |         |      |             |
| SequentialChunkProcessor | 4           |   NA |    NA |     ? |       ? |    ? |           ? |
| ParallelChunkProcessor   | 4           |   NA |    NA |     ? |       ? |    ? |           ? |
|                          |             |      |       |       |         |      |             |
| SequentialChunkProcessor | 8           |   NA |    NA |     ? |       ? |    ? |           ? |
| ParallelChunkProcessor   | 8           |   NA |    NA |     ? |       ? |    ? |           ? |

Benchmarks with issues:
  FileSorterBenchmark.SequentialChunkProcessor: Job-MHGIWO(InvocationCount=1, UnrollFactor=1) [ChunkSizeMb=1]
  FileSorterBenchmark.ParallelChunkProcessor: Job-MHGIWO(InvocationCount=1, UnrollFactor=1) [ChunkSizeMb=1]
  FileSorterBenchmark.SequentialChunkProcessor: Job-MHGIWO(InvocationCount=1, UnrollFactor=1) [ChunkSizeMb=4]
  FileSorterBenchmark.ParallelChunkProcessor: Job-MHGIWO(InvocationCount=1, UnrollFactor=1) [ChunkSizeMb=4]
  FileSorterBenchmark.SequentialChunkProcessor: Job-MHGIWO(InvocationCount=1, UnrollFactor=1) [ChunkSizeMb=8]
  FileSorterBenchmark.ParallelChunkProcessor: Job-MHGIWO(InvocationCount=1, UnrollFactor=1) [ChunkSizeMb=8]
