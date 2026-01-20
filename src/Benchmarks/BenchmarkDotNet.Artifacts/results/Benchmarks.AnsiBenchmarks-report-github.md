```

BenchmarkDotNet v0.15.8, macOS Tahoe 26.2 (25C56) [Darwin 25.2.0]
Apple M3 Pro, 1 CPU, 11 logical and 11 physical cores
.NET SDK 10.0.101
  [Host]    : .NET 10.0.1 (10.0.1, 10.0.125.57005), Arm64 RyuJIT armv8.0-a
  .NET 10.0 : .NET 10.0.1 (10.0.1, 10.0.125.57005), Arm64 RyuJIT armv8.0-a
  .NET 8.0  : .NET 8.0.17 (8.0.17, 8.0.1725.26602), Arm64 RyuJIT armv8.0-a
  .NET 9.0  : .NET 9.0.10 (9.0.10, 9.0.1025.47515), Arm64 RyuJIT armv8.0-a


```
| Method           | Job       | Runtime   | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|----------------- |---------- |---------- |---------:|---------:|---------:|-------:|-------:|----------:|
| RenderableToAnsi | .NET 10.0 | .NET 10.0 | 12.33 μs | 0.054 μs | 0.045 μs | 5.3558 | 0.1831 |  43.87 KB |
| RenderableToAnsi | .NET 8.0  | .NET 8.0  | 14.12 μs | 0.100 μs | 0.094 μs | 5.7068 | 0.1831 |  46.73 KB |
| RenderableToAnsi | .NET 9.0  | .NET 9.0  | 14.12 μs | 0.052 μs | 0.049 μs | 5.6458 | 0.1984 |  46.17 KB |
