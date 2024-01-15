using BenchmarkDotNet.Running;
using Rocks.Performance;

//var ideas = new WriteLinesIdeas();

//var oldContent = ideas.OldWay();
//Console.WriteLine(oldContent.Length);
//Console.WriteLine(oldContent);

//Console.WriteLine();
//Console.WriteLine();

//var newContent = ideas.NewWay();
//Console.WriteLine(newContent.Length);
//Console.WriteLine(newContent);

#pragma warning disable CA1852 // Seal internal types
BenchmarkRunner.Run<WriteLinesIdeas>();
#pragma warning restore CA1852 // Seal internal types