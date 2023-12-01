Construct

| Method          | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| ConstructOldWay | 18.39 ns | 0.313 ns | 0.293 ns |  1.00 |    0.00 | 0.0344 |     144 B |        1.00 |
| ConstructNewWay | 22.65 ns | 0.392 ns | 0.367 ns |  1.23 |    0.03 | 0.0306 |     128 B |        0.89 |

Verify

| Method       | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------- |---------:|--------:|--------:|------:|--------:|-------:|----------:|------------:|
| VerifyOldWay | 263.0 ns | 5.36 ns | 7.51 ns |  1.00 |    0.00 | 0.2346 |     984 B |        1.00 |
| VerifyNewWay | 167.4 ns | 2.26 ns | 2.00 ns |  0.64 |    0.02 | 0.1624 |     680 B |        0.69 |

Callback

| Method         | Mean     | Error   | StdDev  | Ratio | Gen0   | Allocated | Alloc Ratio |
|--------------- |---------:|--------:|--------:|------:|-------:|----------:|------------:|
| CallbackOldWay | 265.0 ns | 5.39 ns | 4.78 ns |  1.00 | 0.2561 |    1072 B |        1.00 |
| CallbackNewWay | 178.4 ns | 2.39 ns | 1.99 ns |  0.67 | 0.1836 |     768 B |        0.72 |