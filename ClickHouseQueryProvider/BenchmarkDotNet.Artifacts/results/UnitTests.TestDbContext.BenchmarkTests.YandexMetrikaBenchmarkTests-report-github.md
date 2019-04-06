``` ini

BenchmarkDotNet=v0.11.5, OS=ubuntu 18.04
Intel Core i5-6200U CPU 2.30GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.2.103
  [Host]     : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT


```
|                             Method |  count |        Mean |     Error |    StdDev |
|----------------------------------- |------- |------------:|----------:|----------:|
| **SelectInAnonymouseClass5Properties** |   **1000** |    **63.34 ms** |  **1.247 ms** |  **1.748 ms** |
|             AdoNetSelectFiveColums |   1000 |    60.41 ms |  1.430 ms |  4.217 ms |
| **SelectInAnonymouseClass5Properties** |  **10000** |   **201.48 ms** |  **4.334 ms** |  **6.486 ms** |
|             AdoNetSelectFiveColums |  10000 |    94.31 ms |  1.847 ms |  3.732 ms |
| **SelectInAnonymouseClass5Properties** | **100000** | **1,546.16 ms** | **25.507 ms** | **23.859 ms** |
|             AdoNetSelectFiveColums | 100000 |   405.29 ms |  7.933 ms |  9.444 ms |
