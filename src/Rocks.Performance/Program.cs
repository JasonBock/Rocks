using BenchmarkDotNet.Running;
using Rocks.Performance;

#pragma warning disable CA1852 // Seal internal types
BenchmarkRunner.Run<CastingSpeed>();

//var test = new SimpleGeneration();

//for (var i = 0; i < 100_000; i++)
//{
//	_ = test.RunGenerator();
//}
#pragma warning restore CA1852 // Seal internal types