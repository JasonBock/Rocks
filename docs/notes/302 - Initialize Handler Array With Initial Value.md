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

I honestly don't know why handling the enumeration the way `List<T>` does makes things better, but, it does. I'll figure out the "why" later. Maybe :). Ok, I think the addition of the `GetEnumerator()` method that returns the struct directly, I think that's the key.

* DONE - Implement `IEnumerable<>` on both `Handlers<>` explicitly
* Update generated code to use `Handlers<>`
    * DONE - Replace creating the list to a `Handlers<>`
    * DONE - Update generated `Verify()` to do a `null` check
    * DONE - In handled members in the mock, change the check for `?.Count > 0` to `is not null`
    * DONE - When adding an expectation, change that to do a `if-else`, using the new `Handlers<>`

Fixes:
* When the `Handlers<>` type is generated, they all need a `THandler` constrained to the right `Handler` type. 
* Could co/contravariance help with having one `Verify()` defined? Maybe not.

I think I overthought it:

| Method                   | count | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------ |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| AddDefault               | 1     |  40.33 ns | 0.797 ns | 0.886 ns |  1.00 |    0.00 | 0.0688 |     288 B |        1.00 |
| AddHandlers              | 1     |  32.67 ns | 0.674 ns | 0.662 ns |  0.81 |    0.02 | 0.0631 |     264 B |        0.92 |
| AddOneCapacity           | 1     |  33.62 ns | 0.526 ns | 0.492 ns |  0.83 |    0.02 | 0.0631 |     264 B |        0.92 |
| AddTwoCapacity           | 1     |  32.28 ns | 0.589 ns | 0.551 ns |  0.80 |    0.02 | 0.0650 |     272 B |        0.94 |
| AddWithOneInitialization | 1     |  55.92 ns | 0.811 ns | 0.677 ns |  1.38 |    0.04 | 0.0765 |     320 B |        1.11 |
|                          |       |           |          |          |       |         |        |           |             |
| AddDefault               | 2     |  63.45 ns | 1.166 ns | 1.091 ns |  1.00 |    0.00 | 0.1166 |     488 B |        1.00 |
| AddHandlers              | 2     |  60.00 ns | 1.060 ns | 0.885 ns |  0.94 |    0.02 | 0.1185 |     496 B |        1.02 |
| AddOneCapacity           | 2     |  78.53 ns | 1.566 ns | 2.484 ns |  1.26 |    0.04 | 0.1204 |     504 B |        1.03 |
| AddTwoCapacity           | 2     |  54.41 ns | 0.887 ns | 0.830 ns |  0.86 |    0.02 | 0.1128 |     472 B |        0.97 |
| AddWithOneInitialization | 2     |  98.04 ns | 1.681 ns | 1.490 ns |  1.55 |    0.04 | 0.1339 |     560 B |        1.15 |
|                          |       |           |          |          |       |         |        |           |             |
| AddDefault               | 5     | 162.44 ns | 3.074 ns | 2.567 ns |  1.00 |    0.00 | 0.2811 |    1176 B |        1.00 |
| AddHandlers              | 5     | 143.64 ns | 2.836 ns | 2.785 ns |  0.89 |    0.02 | 0.2849 |    1192 B |        1.01 |
| AddOneCapacity           | 5     | 187.47 ns | 1.945 ns | 1.819 ns |  1.16 |    0.02 | 0.2983 |    1248 B |        1.06 |
| AddTwoCapacity           | 5     | 170.56 ns | 2.465 ns | 2.059 ns |  1.05 |    0.02 | 0.2906 |    1216 B |        1.03 |
| AddWithOneInitialization | 5     | 215.84 ns | 2.891 ns | 2.563 ns |  1.33 |    0.02 | 0.3116 |    1304 B |        1.11 |
|                          |       |           |          |          |       |         |        |           |             |
| AddDefault               | 10    | 316.95 ns | 4.586 ns | 7.004 ns |  1.00 |    0.00 | 0.5565 |    2328 B |        1.00 |
| AddHandlers              | 10    | 291.17 ns | 5.073 ns | 4.745 ns |  0.91 |    0.03 | 0.5622 |    2352 B |        1.01 |
| AddOneCapacity           | 10    | 335.64 ns | 5.682 ns | 5.315 ns |  1.05 |    0.03 | 0.5736 |    2400 B |        1.03 |
| AddTwoCapacity           | 10    | 324.92 ns | 5.796 ns | 4.840 ns |  1.01 |    0.03 | 0.5660 |    2368 B |        1.02 |
| AddWithOneInitialization | 10    | 369.46 ns | 7.415 ns | 8.242 ns |  1.16 |    0.03 | 0.5870 |    2456 B |        1.05 |

| Method            | handlers             | Mean      | Error     | StdDev    | Allocated |
|------------------ |--------------------- |----------:|----------:|----------:|----------:|
| EnumerateHandlers | Rocks(...)rInt] [46] | 1.3925 ns | 0.0366 ns | 0.0306 ns |         - |
| EnumerateHandlers | Rocks(...)rInt] [46] | 1.2055 ns | 0.0459 ns | 0.0430 ns |         - |
| EnumerateHandlers | Rocks(...)rInt] [46] | 3.3687 ns | 0.0680 ns | 0.0568 ns |         - |
| EnumerateHandlers | Rocks(...)rInt] [46] | 3.8827 ns | 0.1066 ns | 0.1047 ns |         - |
| EnumerateList     | Syste(...)rInt] [63] | 0.6215 ns | 0.0104 ns | 0.0092 ns |         - |
| EnumerateList     | Syste(...)rInt] [63] | 1.8759 ns | 0.0377 ns | 0.0352 ns |         - |
| EnumerateList     | Syste(...)rInt] [63] | 4.3624 ns | 0.0530 ns | 0.0496 ns |         - |
| EnumerateList     | Syste(...)rInt] [63] | 4.4588 ns | 0.1020 ns | 0.1048 ns |         - |

Before I keep going...maybe I should try a `LinkedList<>`, just to see how it performs:

| Method                   | count | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------ |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| AddDefault               | 1     |  40.75 ns | 0.832 ns | 0.959 ns |  1.00 |    0.00 | 0.0688 |     288 B |        1.00 |
| AddLinkedList            | 1     |  34.97 ns | 0.413 ns | 0.323 ns |  0.87 |    0.02 | 0.0688 |     288 B |        1.00 |
| AddHandlers              | 1     |  32.34 ns | 0.662 ns | 0.812 ns |  0.80 |    0.02 | 0.0631 |     264 B |        0.92 |
| AddOneCapacity           | 1     |  31.84 ns | 0.652 ns | 0.697 ns |  0.78 |    0.02 | 0.0631 |     264 B |        0.92 |
| AddTwoCapacity           | 1     |  32.08 ns | 0.657 ns | 0.674 ns |  0.79 |    0.03 | 0.0650 |     272 B |        0.94 |
| AddWithOneInitialization | 1     |  56.08 ns | 1.154 ns | 1.579 ns |  1.38 |    0.05 | 0.0764 |     320 B |        1.11 |
|                          |       |           |          |          |       |         |        |           |             |
| AddDefault               | 2     |  63.14 ns | 1.263 ns | 1.297 ns |  1.00 |    0.00 | 0.1166 |     488 B |        1.00 |
| AddLinkedList            | 2     |  68.01 ns | 1.383 ns | 1.748 ns |  1.08 |    0.04 | 0.1281 |     536 B |        1.10 |
| AddHandlers              | 2     |  60.41 ns | 1.245 ns | 1.662 ns |  0.95 |    0.02 | 0.1185 |     496 B |        1.02 |
| AddOneCapacity           | 2     |  75.77 ns | 1.361 ns | 1.336 ns |  1.20 |    0.02 | 0.1204 |     504 B |        1.03 |
| AddTwoCapacity           | 2     |  54.06 ns | 0.404 ns | 0.315 ns |  0.85 |    0.02 | 0.1128 |     472 B |        0.97 |
| AddWithOneInitialization | 2     | 102.93 ns | 1.087 ns | 1.017 ns |  1.63 |    0.05 | 0.1339 |     560 B |        1.15 |
|                          |       |           |          |          |       |         |        |           |             |
| AddDefault               | 5     | 162.84 ns | 3.264 ns | 4.008 ns |  1.00 |    0.00 | 0.2811 |    1176 B |        1.00 |
| AddLinkedList            | 5     | 165.34 ns | 2.446 ns | 2.043 ns |  1.01 |    0.03 | 0.3059 |    1280 B |        1.09 |
| AddHandlers              | 5     | 142.51 ns | 2.306 ns | 2.157 ns |  0.87 |    0.02 | 0.2849 |    1192 B |        1.01 |
| AddOneCapacity           | 5     | 188.56 ns | 3.480 ns | 3.085 ns |  1.15 |    0.04 | 0.2983 |    1248 B |        1.06 |
| AddTwoCapacity           | 5     | 171.24 ns | 2.155 ns | 1.799 ns |  1.04 |    0.03 | 0.2906 |    1216 B |        1.03 |
| AddWithOneInitialization | 5     | 216.67 ns | 2.194 ns | 1.945 ns |  1.32 |    0.04 | 0.3116 |    1304 B |        1.11 |
|                          |       |           |          |          |       |         |        |           |             |
| AddDefault               | 10    | 316.24 ns | 6.358 ns | 6.803 ns |  1.00 |    0.00 | 0.5565 |    2328 B |        1.00 |
| AddLinkedList            | 10    | 331.92 ns | 6.376 ns | 5.652 ns |  1.05 |    0.03 | 0.6022 |    2520 B |        1.08 |
| AddHandlers              | 10    | 289.58 ns | 5.180 ns | 4.845 ns |  0.91 |    0.02 | 0.5622 |    2352 B |        1.01 |
| AddOneCapacity           | 10    | 348.55 ns | 5.663 ns | 5.020 ns |  1.10 |    0.03 | 0.5736 |    2400 B |        1.03 |
| AddTwoCapacity           | 10    | 321.30 ns | 3.219 ns | 2.688 ns |  1.01 |    0.03 | 0.5660 |    2368 B |        1.02 |
| AddWithOneInitialization | 10    | 365.51 ns | 4.204 ns | 3.727 ns |  1.15 |    0.03 | 0.5870 |    2456 B |        1.05 |

| Method              | handlers             | Mean      | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|-------------------- |--------------------- |----------:|----------:|----------:|------:|--------:|----------:|------------:|
| EnumerateHandlers   | Rocks(...)rInt] [46] | 1.0575 ns | 0.0285 ns | 0.0253 ns |     ? |       ? |         - |           ? |
| EnumerateHandlers   | Rocks(...)rInt] [46] | 1.2259 ns | 0.0531 ns | 0.0521 ns |     ? |       ? |         - |           ? |
| EnumerateHandlers   | Rocks(...)rInt] [46] | 3.4842 ns | 0.0965 ns | 0.0902 ns |     ? |       ? |         - |           ? |
| EnumerateHandlers   | Rocks(...)rInt] [46] | 3.8933 ns | 0.0760 ns | 0.0711 ns |     ? |       ? |         - |           ? |
|                     |                      |           |           |           |       |         |           |             |
| EnumerateLinkedList | Syste(...)rInt] [69] | 1.0713 ns | 0.0323 ns | 0.0302 ns |     ? |       ? |         - |           ? |
| EnumerateLinkedList | Syste(...)rInt] [69] | 2.6119 ns | 0.0244 ns | 0.0216 ns |     ? |       ? |         - |           ? |
| EnumerateLinkedList | Syste(...)rInt] [69] | 3.8765 ns | 0.0789 ns | 0.0699 ns |     ? |       ? |         - |           ? |
| EnumerateLinkedList | Syste(...)rInt] [69] | 4.4030 ns | 0.0735 ns | 0.0652 ns |     ? |       ? |         - |           ? |
|                     |                      |           |           |           |       |         |           |             |
| EnumerateList       | Syste(...)rInt] [63] | 0.9396 ns | 0.0401 ns | 0.0313 ns |  1.00 |    0.00 |         - |          NA |
| EnumerateList       | Syste(...)rInt] [63] | 1.9398 ns | 0.0551 ns | 0.0589 ns |  2.08 |    0.08 |         - |          NA |
| EnumerateList       | Syste(...)rInt] [63] | 4.5234 ns | 0.1128 ns | 0.1000 ns |  4.82 |    0.22 |         - |          NA |
| EnumerateList       | Syste(...)rInt] [63] | 4.5265 ns | 0.1204 ns | 0.1182 ns |  4.84 |    0.23 |         - |          NA |

* DONE - Run all integration and code generation tests
* DONE - Fix all unit tests
* DONE - For any types like Handler, Handlers, etc. that are public but not intended to be used by external code, add that to comments.
* DONE - Delete perf projects other than Rocks.Performance
* DONE - Add XML comments for `Handlers<>`
* DONE - Write unit tests for `Handlers<>`
* DONE - Celebrate