using BenchmarkDotNet.Running;
using Rocks.Performance;

BenchmarkRunner.Run<CastingSpeed>();

//var test = new SimpleGeneration();

//for (var i = 0; i < 100_000; i++)
//{
//	_ = test.RunGenerator();
//}