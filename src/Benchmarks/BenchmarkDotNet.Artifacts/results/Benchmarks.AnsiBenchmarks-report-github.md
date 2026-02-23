```

BenchmarkDotNet v0.15.8, macOS Tahoe 26.2 (25C56) [Darwin 25.2.0]
Apple M3 Pro, 1 CPU, 11 logical and 11 physical cores
.NET SDK 10.0.101
  [Host]    : .NET 10.0.1 (10.0.1, 10.0.125.57005), Arm64 RyuJIT armv8.0-a
  .NET 10.0 : .NET 10.0.1 (10.0.1, 10.0.125.57005), Arm64 RyuJIT armv8.0-a
  .NET 8.0  : .NET 8.0.17 (8.0.17, 8.0.1725.26602), Arm64 RyuJIT armv8.0-a
  .NET 9.0  : .NET 9.0.10 (9.0.10, 9.0.1025.47515), Arm64 RyuJIT armv8.0-a


```
| Method           | Job       | Runtime   | Mean     | Error   | StdDev  | Gen0   | Gen1   | Allocated |
|----------------- |---------- |---------- |---------:|--------:|--------:|-------:|-------:|----------:|
| RenderableToAnsi | .NET 10.0 | .NET 10.0 | 786.6 ns | 1.87 ns | 1.75 ns | 0.3662 | 0.0010 |   2.99 KB |
| RenderableToAnsi | .NET 8.0  | .NET 8.0  | 897.3 ns | 2.21 ns | 1.96 ns | 0.3815 | 0.0010 |   3.12 KB |
| RenderableToAnsi | .NET 9.0  | .NET 9.0  | 935.2 ns | 3.13 ns | 2.77 ns | 0.3700 | 0.0010 |   3.02 KB |
