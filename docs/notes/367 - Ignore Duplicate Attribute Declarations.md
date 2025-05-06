Current:

| Method       | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------- |---------:|---------:|---------:|-------:|-------:|----------:|
| RunGenerator | 69.67 us | 0.428 us | 0.358 us | 7.8125 | 0.9766 | 138.23 KB |

Branch:

| Method       | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------- |---------:|---------:|---------:|-------:|-------:|----------:|
| RunGenerator | 62.60 us | 0.614 us | 0.574 us | 7.3242 | 0.9766 |  129.6 KB |

The other tests showed a slight favoritism to the new approach, but it wasn't 100% consistent. However, when there's a duplication, even just one, there is a clear winner, so I think I'll go with the new approach.