using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Rocks.Sketchpad
{
	[MemoryDiagnoser]
	public class MetadataReferenceBenchmark
	{
		private readonly Assembly assembly;
		private readonly ConcurrentDictionary<Assembly, PortableExecutableReference> concurrentMap;
		private readonly Dictionary<Assembly, PortableExecutableReference> map;

		public MetadataReferenceBenchmark()
		{
			this.assembly = typeof(MetadataReferenceBenchmark).Assembly;
			var metadata = MetadataReference.CreateFromFile(this.assembly.Location);
			this.map = new Dictionary<Assembly, PortableExecutableReference> { { this.assembly, metadata } };
			this.concurrentMap = new ConcurrentDictionary<Assembly, PortableExecutableReference>();
			this.concurrentMap.TryAdd(this.assembly, metadata);
		}

		[Benchmark]
		public PortableExecutableReference GetReferenceFromAssembly() => 
			MetadataReference.CreateFromFile(this.assembly.Location);

		[Benchmark]
		public PortableExecutableReference GetReferenceFromMap() =>
			this.map[this.assembly];

		[Benchmark]
		public PortableExecutableReference GetReferenceFromConcurrentMap() =>
			this.concurrentMap[this.assembly];

		[Benchmark]
		public PortableExecutableReference GetReferenceFromConcurrentMapViaGetOrAdd() =>
			this.concurrentMap.GetOrAdd(this.assembly, asm => MetadataReference.CreateFromFile(asm.Location));
	}
}
