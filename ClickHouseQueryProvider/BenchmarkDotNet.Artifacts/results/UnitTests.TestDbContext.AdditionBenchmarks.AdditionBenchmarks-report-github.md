``` ini

BenchmarkDotNet=v0.11.5, OS=ubuntu 18.04
Intel Core i5-6200U CPU 2.30GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.2.103
  [Host]     : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT


```
|         Method |         Mean |      Error |     StdDev |
|--------------- |-------------:|-----------:|-----------:|
|     Add100Rows |     89.91 ms |   1.944 ms |   1.909 ms |
|    Add1000Rows |     89.25 ms |   1.770 ms |   1.656 ms |
|   Add10000Rows |    166.58 ms |   3.272 ms |   6.610 ms |
|  Add100000Rows |    675.07 ms |  13.590 ms |  13.956 ms |
| Add1000000Rows | 13,762.25 ms | 259.112 ms | 242.374 ms |
