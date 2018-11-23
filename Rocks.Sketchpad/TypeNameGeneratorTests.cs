using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Code;
using System;
using System.Collections.Generic;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Sketchpad
{
	[MemoryDiagnoser]
	[ClrJob, CoreJob]
	public class TypeNameGeneratorTests
	{
		private SortedSet<string> namespaces;

		[GlobalSetup]
		public void GlobalSetup() => this.namespaces = new SortedSet<string>();

		public IEnumerable<IParam> Types()
		{
			yield return new TypeParams(typeof(NonGenericType), "NonGenericType");
			yield return new TypeParams(typeof(GenericType<>), "GenericType<>");
		}

		[Benchmark]
		public string GetWithGuid()
		{
			var name = this.Type.IsGenericTypeDefinition ?
				$"{Guid.NewGuid().ToString("N")}{this.Type.GetGenericArguments(this.namespaces).arguments}" : Guid.NewGuid().ToString("N");
			return $"Rock{name}";
		}

		[Benchmark]
		public string GetWithToken() => this.Type.IsGenericTypeDefinition ?
			$"Rock{this.Type.MetadataToken}{this.Type.GetGenericArguments(this.namespaces).arguments}" : $"Rock{this.Type.MetadataToken}";

		[ParamsSource(nameof(Types))]
		public Type Type { get; set; }
	}

	public class TypeParams
		: IParam
	{
		public TypeParams(Type target, string typeName)
		{
			this.Target = target;
			this.TypeName = typeName;
		}

		public object Value => this.Target;

		public string DisplayText => this.Target.Name;

		public string ToSourceCode() => $"typeof({this.TypeName})";

		private Type Target { get; }
		private string TypeName { get; }
	}

	public class NonGenericType { }
	public class GenericType<T> { }
}
