```

BenchmarkDotNet v0.15.8, macOS Tahoe 26.2 (25C56) [Darwin 25.2.0]
Apple M3 Pro, 1 CPU, 11 logical and 11 physical cores
.NET SDK 10.0.101
  [Host]    : .NET 10.0.1 (10.0.1, 10.0.125.57005), Arm64 RyuJIT armv8.0-a
  .NET 10.0 : .NET 10.0.1 (10.0.1, 10.0.125.57005), Arm64 RyuJIT armv8.0-a
  .NET 8.0  : .NET 8.0.17 (8.0.17, 8.0.1725.26602), Arm64 RyuJIT armv8.0-a
  .NET 9.0  : .NET 9.0.10 (9.0.10, 9.0.1025.47515), Arm64 RyuJIT armv8.0-a


```
| Method | Job       | Runtime   | Mean     | Error   | StdDev  | Gen0   | Gen1   | Allocated |
|------- |---------- |---------- |---------:|--------:|--------:|-------:|-------:|----------:|
| Render | .NET 10.0 | .NET 10.0 | 723.8 ns | 4.00 ns | 3.74 ns | 0.2956 | 0.0124 |   2.42 KB |
| Render | .NET 8.0  | .NET 8.0  | 842.7 ns | 9.63 ns | 8.54 ns | 0.3119 | 0.0124 |   2.55 KB |
| Render | .NET 9.0  | .NET 9.0  | 859.7 ns | 6.40 ns | 4.99 ns | 0.3004 | 0.0124 |   2.46 KB |
