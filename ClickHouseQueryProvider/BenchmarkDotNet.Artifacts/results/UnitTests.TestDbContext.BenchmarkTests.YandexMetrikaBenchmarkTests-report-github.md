``` ini

BenchmarkDotNet=v0.11.5, OS=ubuntu 18.04
Intel Core i5-6200U CPU 2.30GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.2.103
  [Host]     : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT


```
|                             Method |   count |         Mean |       Error |      StdDev |
|----------------------------------- |-------- |-------------:|------------:|------------:|
|                          **SelectAll** |      **10** |     **86.11 ms** |   **3.7074 ms** |  **10.9313 ms** |
|                       AdoNetGetAll |      10 |     93.09 ms |   1.8532 ms |   4.0287 ms |
|              SelectInPrimitiveType |      10 |     50.01 ms |   0.6496 ms |   0.6076 ms |
|    AdoNetTestSelectInPrimitiveType |      10 |     52.10 ms |   0.4320 ms |   0.4041 ms |
|                          **SelectAll** |     **100** |    **102.23 ms** |   **3.7603 ms** |  **11.0872 ms** |
|                       AdoNetGetAll |     100 |     94.90 ms |   1.8649 ms |   3.0641 ms |
|              SelectInPrimitiveType |     100 |     51.46 ms |   0.7534 ms |   0.7047 ms |
|    AdoNetTestSelectInPrimitiveType |     100 |     53.86 ms |   0.8965 ms |   0.8386 ms |
|                          **SelectAll** |    **1000** |    **290.33 ms** |   **5.7560 ms** |  **13.1093 ms** |
|                       AdoNetGetAll |    1000 |    136.38 ms |   2.7109 ms |   4.8186 ms |
| SelectInAnonymouseClass5Properties |    1000 |     82.76 ms |   1.6469 ms |   2.9274 ms |
|             AdoNetSelectFiveColums |    1000 |     71.14 ms |   1.3701 ms |   1.5779 ms |
|              SelectInPrimitiveType |    1000 |     63.08 ms |   1.2167 ms |   1.4484 ms |
|    AdoNetTestSelectInPrimitiveType |    1000 |     67.15 ms |   1.2888 ms |   1.4325 ms |
|                          **SelectAll** |   **10000** |  **2,124.68 ms** |  **18.4618 ms** |  **17.2691 ms** |
|                       AdoNetGetAll |   10000 |    572.70 ms |  10.6841 ms |  10.4932 ms |
|         SelectInDtoWith5Properties |   10000 |    207.63 ms |   4.1065 ms |   9.5989 ms |
| SelectInAnonymouseClass5Properties |   10000 |    202.62 ms |   4.0354 ms |   9.8227 ms |
|             AdoNetSelectFiveColums |   10000 |    107.10 ms |   2.1133 ms |   4.5035 ms |
|              SelectInPrimitiveType |   10000 |    107.39 ms |   2.1177 ms |   5.0328 ms |
|    AdoNetTestSelectInPrimitiveType |   10000 |    103.42 ms |   2.0651 ms |   5.1045 ms |
|                          **SelectAll** |  **100000** | **20,035.78 ms** | **145.6007 ms** | **136.1950 ms** |
|                       AdoNetGetAll |  100000 |  4,730.44 ms |  37.9116 ms |  35.4626 ms |
|         SelectInDtoWith5Properties |  100000 |  1,464.93 ms |  14.4586 ms |  13.5245 ms |
| SelectInAnonymouseClass5Properties |  100000 |  1,477.50 ms |  10.5184 ms |   9.8389 ms |
|             AdoNetSelectFiveColums |  100000 |    403.00 ms |   8.0049 ms |  12.4627 ms |
|              SelectInPrimitiveType |  100000 |    460.72 ms |   9.1018 ms |  11.5109 ms |
|    AdoNetTestSelectInPrimitiveType |  100000 |    404.48 ms |   8.0009 ms |   7.8579 ms |
|         **SelectInDtoWith5Properties** | **1000000** | **14,160.45 ms** | **176.7752 ms** | **156.7066 ms** |
