using BenchmarkDotNet.Running;
using Rocks.Performance.Integration;

#pragma warning disable CA1852 // Seal internal types
BenchmarkRunner.Run<LargeInterfaceGeneration>();
#pragma warning restore CA1852 // Seal internal types