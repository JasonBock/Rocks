| Method                                     | information         | Mean     | Error   | StdDev  | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------------------------------- |-------------------- |---------:|--------:|--------:|------:|-------:|----------:|------------:|
| GetDescriptionNoOptimizationAttempts       | Attr(...)on } [238] | 794.0 ns | 7.67 ns | 7.17 ns |  1.00 | 0.1364 |    2360 B |        1.00 |
| GetDescriptionOptimizationsConcat          | Attr(...)on } [238] | 764.7 ns | 1.79 ns | 1.59 ns |  0.96 | 0.1297 |    2240 B |        0.95 |
| GetDescriptionOptimizationsPreallocateList | Attr(...)on } [238] | 765.1 ns | 0.76 ns | 0.67 ns |  0.96 | 0.1173 |    2024 B |        0.86 |
|                                            |                     |          |         |         |       |        |           |             |
| GetDescriptionNoOptimizationAttempts       | Attr(...)on } [122] | 309.5 ns | 2.27 ns | 1.90 ns |  1.00 | 0.0458 |     792 B |        1.00 |
| GetDescriptionOptimizationsConcat          | Attr(...)on } [122] | 322.8 ns | 1.04 ns | 0.87 ns |  1.04 | 0.0477 |     824 B |        1.04 |
| GetDescriptionOptimizationsPreallocateList | Attr(...)on } [122] | 298.4 ns | 0.73 ns | 0.68 ns |  0.96 | 0.0429 |     744 B |        0.94 |
|                                            |                     |          |         |         |       |        |           |             |
| GetDescriptionNoOptimizationAttempts       | Attr(...)on } [182] | 498.9 ns | 7.15 ns | 6.69 ns |  1.00 | 0.0944 |    1640 B |        1.00 |
| GetDescriptionOptimizationsConcat          | Attr(...)on } [182] | 466.5 ns | 2.66 ns | 2.22 ns |  0.94 | 0.0877 |    1520 B |        0.93 |
| GetDescriptionOptimizationsPreallocateList | Attr(...)on } [182] | 459.7 ns | 1.57 ns | 1.47 ns |  0.92 | 0.0820 |    1416 B |        0.86 |
|                                            |                     |          |         |         |       |        |           |             |
| GetDescriptionNoOptimizationAttempts       | Attr(...)on } [128] | 313.5 ns | 1.04 ns | 0.87 ns |  1.00 | 0.0539 |     936 B |        1.00 |
| GetDescriptionOptimizationsConcat          | Attr(...)on } [128] | 311.6 ns | 0.81 ns | 0.72 ns |  0.99 | 0.0520 |     904 B |        0.97 |
| GetDescriptionOptimizationsPreallocateList | Attr(...)on } [128] | 306.8 ns | 1.41 ns | 1.32 ns |  0.98 | 0.0496 |     856 B |        0.91 |
|                                            |                     |          |         |         |       |        |           |             |
| GetDescriptionNoOptimizationAttempts       | Attr(...)on } [170] | 367.8 ns | 1.56 ns | 1.46 ns |  1.00 | 0.0734 |    1272 B |        1.00 |
| GetDescriptionOptimizationsConcat          | Attr(...)on } [170] | 382.4 ns | 1.99 ns | 1.86 ns |  1.04 | 0.0753 |    1304 B |        1.03 |
| GetDescriptionOptimizationsPreallocateList | Attr(...)on } [170] | 346.9 ns | 1.34 ns | 1.19 ns |  0.94 | 0.0653 |    1128 B |        0.89 |
|                                            |                     |          |         |         |       |        |           |             |
| GetDescriptionNoOptimizationAttempts       | Attr(...)on } [110] | 160.9 ns | 0.70 ns | 0.62 ns |  1.00 | 0.0193 |     336 B |        1.00 |
| GetDescriptionOptimizationsConcat          | Attr(...)on } [110] | 170.4 ns | 1.00 ns | 0.94 ns |  1.06 | 0.0174 |     304 B |        0.90 |
| GetDescriptionOptimizationsPreallocateList | Attr(...)on } [110] | 166.7 ns | 0.79 ns | 0.74 ns |  1.04 | 0.0174 |     304 B |        0.90 |