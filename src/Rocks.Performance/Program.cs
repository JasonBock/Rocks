﻿using BenchmarkDotNet.Running;
using Rocks;
using Rocks.Performance;

#pragma warning disable CA1852 // Seal internal types
BenchmarkRunner.Run<HandlerLists>();
#pragma warning restore CA1852 // Seal internal types