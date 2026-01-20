```

BenchmarkDotNet v0.15.8, macOS Tahoe 26.2 (25C56) [Darwin 25.2.0]
Apple M3 Pro, 1 CPU, 11 logical and 11 physical cores
.NET SDK 10.0.101
  [Host]    : .NET 10.0.1 (10.0.1, 10.0.125.57005), Arm64 RyuJIT armv8.0-a
  .NET 10.0 : .NET 10.0.1 (10.0.1, 10.0.125.57005), Arm64 RyuJIT armv8.0-a
  .NET 8.0  : .NET 8.0.17 (8.0.17, 8.0.1725.26602), Arm64 RyuJIT armv8.0-a
  .NET 9.0  : .NET 9.0.10 (9.0.10, 9.0.1025.47515), Arm64 RyuJIT armv8.0-a


```
| Method | Job       | Runtime   | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------- |---------- |---------- |---------:|---------:|---------:|-------:|-------:|----------:|
| Render | .NET 10.0 | .NET 10.0 | 12.45 μs | 0.130 μs | 0.101 μs | 5.4169 | 0.0916 |  44.34 KB |
| Render | .NET 8.0  | .NET 8.0  | 21.74 μs | 0.033 μs | 0.031 μs | 5.7678 | 0.0916 |   47.2 KB |
| Render | .NET 9.0  | .NET 9.0  | 14.37 μs | 0.070 μs | 0.065 μs | 5.7068 | 0.0916 |  46.64 KB |
