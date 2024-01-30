| Method                   | count | Mean      | Error    | StdDev    | Median    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------ |----------:|---------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| AddDefault               | 1     |  40.92 ns | 1.396 ns |  3.961 ns |  42.05 ns |  1.00 |    0.00 | 0.0516 |     216 B |        1.00 |
| AddHandlers              | 1     |  26.69 ns | 1.058 ns |  3.000 ns |  27.47 ns |  0.66 |    0.11 | 0.0382 |     160 B |        0.74 |
| AddOneCapacity           | 1     |  28.91 ns | 0.910 ns |  2.596 ns |  29.32 ns |  0.71 |    0.08 | 0.0459 |     192 B |        0.89 |
| AddTwoCapacity           | 1     |  33.10 ns | 0.674 ns |  1.392 ns |  32.93 ns |  0.85 |    0.13 | 0.0478 |     200 B |        0.93 |
| AddWithOneInitialization | 1     |  61.75 ns | 0.812 ns |  0.678 ns |  61.74 ns |  1.68 |    0.24 | 0.0592 |     248 B |        1.15 |
|                          |       |           |          |           |           |       |         |        |           |             |
| AddDefault               | 2     |  56.71 ns | 0.881 ns |  0.824 ns |  56.67 ns |  1.00 |    0.00 | 0.0823 |     344 B |        1.00 |
| AddHandlers              | 2     |  54.56 ns | 1.083 ns |  1.204 ns |  54.53 ns |  0.96 |    0.02 | 0.0688 |     288 B |        0.84 |
| AddOneCapacity           | 2     |  86.30 ns | 4.339 ns | 12.792 ns |  84.85 ns |  1.71 |    0.10 | 0.0861 |     360 B |        1.05 |
| AddTwoCapacity           | 2     |  44.36 ns | 0.933 ns |  1.753 ns |  43.93 ns |  0.80 |    0.04 | 0.0783 |     328 B |        0.95 |
| AddWithOneInitialization | 2     |  90.20 ns | 1.695 ns |  1.814 ns |  90.34 ns |  1.60 |    0.03 | 0.0994 |     416 B |        1.21 |
|                          |       |           |          |           |           |       |         |        |           |             |
| AddDefault               | 5     | 131.93 ns | 2.641 ns |  4.413 ns | 131.86 ns |  1.00 |    0.00 | 0.1950 |     816 B |        1.00 |
| AddHandlers              | 5     |  85.75 ns | 1.728 ns |  3.203 ns |  85.14 ns |  0.65 |    0.03 | 0.1606 |     672 B |        0.82 |
| AddOneCapacity           | 5     | 152.95 ns | 2.635 ns |  2.465 ns | 152.97 ns |  1.14 |    0.04 | 0.2122 |     888 B |        1.09 |
| AddTwoCapacity           | 5     | 137.95 ns | 1.791 ns |  1.495 ns | 137.99 ns |  1.03 |    0.04 | 0.2046 |     856 B |        1.05 |
| AddWithOneInitialization | 5     | 172.90 ns | 3.412 ns |  4.894 ns | 172.35 ns |  1.30 |    0.06 | 0.2255 |     944 B |        1.16 |
|                          |       |           |          |           |           |       |         |        |           |             |
| AddDefault               | 10    | 244.35 ns | 4.715 ns |  4.842 ns | 243.15 ns |  1.00 |    0.00 | 0.3843 |    1608 B |        1.00 |
| AddHandlers              | 10    | 169.12 ns | 3.393 ns |  3.174 ns | 168.87 ns |  0.69 |    0.02 | 0.3135 |    1312 B |        0.82 |
| AddOneCapacity           | 10    | 265.92 ns | 5.246 ns |  7.524 ns | 265.73 ns |  1.09 |    0.04 | 0.4015 |    1680 B |        1.04 |
| AddTwoCapacity           | 10    | 259.37 ns | 4.219 ns |  3.740 ns | 259.35 ns |  1.06 |    0.02 | 0.3939 |    1648 B |        1.02 |
| AddWithOneInitialization | 10    | 287.93 ns | 5.717 ns |  8.200 ns | 285.29 ns |  1.18 |    0.04 | 0.4148 |    1736 B |        1.08 |

Next steps:
* Benchmark foreach/enumeration

Sealed class

| Method            | handlers             | Mean      | Error     | StdDev    | Gen0   | Allocated |
|------------------ |--------------------- |----------:|----------:|----------:|-------:|----------:|
| EnumerateHandlers | Rocks(...)nt32] [ 1] |  5.937 ns | 0.0849 ns | 0.0752 ns | 0.0076 |      32 B |
| EnumerateHandlers | Rocks(...)nt32] [ 2] |  7.839 ns | 0.1862 ns | 0.2217 ns | 0.0076 |      32 B |
| EnumerateHandlers | Rocks(...)nt32] [ 5] | 11.618 ns | 0.1900 ns | 0.1777 ns | 0.0076 |      32 B |
| EnumerateHandlers | Rocks(...)nt32] [10] | 17.498 ns | 0.3538 ns | 0.2955 ns | 0.0076 |      32 B |
| EnumerateList     | Syste(...)rInt] [ 1] |  1.144 ns | 0.0383 ns | 0.0358 ns |      - |         - |
| EnumerateList     | Syste(...)rInt] [ 2] |  1.415 ns | 0.0561 ns | 0.0551 ns |      - |         - |
| EnumerateList     | Syste(...)rInt] [ 5] |  3.829 ns | 0.1041 ns | 0.1114 ns |      - |         - |
| EnumerateList     | Syste(...)rInt] [10] |  6.658 ns | 0.1500 ns | 0.1403 ns |      - |         - |

Struct

| Method            | handlers             | Mean      | Error     | StdDev    | Gen0   | Allocated |
|------------------ |--------------------- |----------:|----------:|----------:|-------:|----------:|
| EnumerateHandlers | Rocks(...)nt32] [ 1] |  6.369 ns | 0.1734 ns | 0.3542 ns | 0.0076 |      32 B |
| EnumerateHandlers | Rocks(...)nt32] [ 2] |  8.618 ns | 0.1806 ns | 0.1601 ns | 0.0076 |      32 B |
| EnumerateHandlers | Rocks(...)nt32] [ 5] | 14.020 ns | 0.2310 ns | 0.1929 ns | 0.0076 |      32 B |
| EnumerateHandlers | Rocks(...)nt32] [10] | 23.288 ns | 0.3702 ns | 0.3281 ns | 0.0076 |      32 B |
| EnumerateList     | Syste(...)rInt] [ 1] |  1.115 ns | 0.0180 ns | 0.0160 ns |      - |         - |
| EnumerateList     | Syste(...)rInt] [ 2] |  1.403 ns | 0.0339 ns | 0.0317 ns |      - |         - |
| EnumerateList     | Syste(...)rInt] [ 5] |  3.855 ns | 0.0929 ns | 0.0725 ns |      - |         - |
| EnumerateList     | Syste(...)rInt] [10] |  6.612 ns | 0.1075 ns | 0.1005 ns |      - |         - |

So, when handlers are added, the "handlers" approach is always faster and consumes less memory. With the custom enumerator, it allocates 32 B and it's a touch slower, but for most mocks, a member will probably just have 1 expectation set, and no more than 2. In those cases, all we'd all is approximately 7 ns and 32 bytes. That should be offset by the gains obtained by using a forward-only linked list-like approach.

It's interesting that using a `sealed class` seems **slightly** better than a `struct`. I did that based on the nested enumerator that `List<>` has. I'm not sure why a list doesn't make another allocation when you enumerate it.

Note that I removed the `Count` property, as it's really not needed - Rocks can just do `null` checks rather than seeing if `Count` is greater than 0.

Wait, I just literally copy/pasted what `List<T>` does (more or less), and...yay!

| Method            | handlers             | Mean      | Error     | StdDev    | Median    | Allocated |
|------------------ |--------------------- |----------:|----------:|----------:|----------:|----------:|
| EnumerateHandlers | Rocks(...)nt32] [ 1] |  3.008 ns | 0.1313 ns | 0.3481 ns |  2.949 ns |         - |
| EnumerateHandlers | Rocks(...)nt32] [ 2] |  4.693 ns | 0.1323 ns | 0.2282 ns |  4.653 ns |         - |
| EnumerateHandlers | Rocks(...)nt32] [ 5] |  5.618 ns | 0.1670 ns | 0.4790 ns |  5.535 ns |         - |
| EnumerateHandlers | Rocks(...)nt32] [10] |  6.809 ns | 0.1878 ns | 0.4980 ns |  6.620 ns |         - |
| EnumerateList     | Syste(...)rInt] [ 1] |  1.908 ns | 0.1416 ns | 0.3993 ns |  1.875 ns |         - |
| EnumerateList     | Syste(...)rInt] [ 2] |  2.345 ns | 0.0999 ns | 0.2685 ns |  2.305 ns |         - |
| EnumerateList     | Syste(...)rInt] [ 5] |  6.452 ns | 0.1807 ns | 0.3437 ns |  6.333 ns |         - |
| EnumerateList     | Syste(...)rInt] [10] | 11.568 ns | 0.2921 ns | 0.8429 ns | 11.540 ns |         - |

I honestly don't know why handling the enumeration the way `List<T>` does makes things better, but, it does. I'll figure out the "why" later. Maybe :).

* Create other handler and add XML comments
* Write unit tests
* Update generated code to use Handlers<>
* Run all tests
* For any types like Handler, Handlers, etc. that are public but not intended to be used by external code, add that to comments.
* Celebrate

