using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using Rocks.Extensions;
using System.Linq;
using BenchmarkDotNet.Code;

namespace Rocks.Sketchpad
{
	[MemoryDiagnoser]
	//[ClrJob, CoreJob]
	public class GenericArgumentsTests
	{
		private SortedSet<string> namespaces;

		[GlobalSetup]
		public void GlobalSetup() => this.namespaces = new SortedSet<string>();

		public IEnumerable<Type> Types()
		{
			yield return typeof(ComplexGenericType<,,,>);
		}

		[ParamsSource(nameof(Types))]
		public Type Type { get; set; }

		[Benchmark]
		public (string arguments, string constraints) GetGenericArgumentsOldWay()
		{
			var @this = this.Type;
			var arguments = string.Empty;
			var constraints = string.Empty;

			if (@this.IsGenericType)
			{
				var genericArguments = new List<string>();
				var genericConstraints = new List<string>();

				foreach (var argument in @this.GetGenericArguments())
				{
					genericArguments.Add($"{argument.GetFullName(this.namespaces)}");
					var constraint = argument.GetConstraints(this.namespaces);

					if (!string.IsNullOrWhiteSpace(constraint))
					{
						genericConstraints.Add(constraint);
					}
				}

				arguments = $"<{string.Join(", ", genericArguments)}>";
				// TODO: This should not add a space in front. The Maker class
				// should adjust the constraints to have a space in front.
				constraints = genericConstraints.Count == 0 ?
					string.Empty : $"{string.Join(" ", genericConstraints)}";
			}

			return (arguments, constraints);
		}

		[Benchmark]
		public (string arguments, string constraints) GetGenericArgumentsOldWayWithCheck()
		{
			var @this = this.Type;
			var arguments = string.Empty;
			var constraints = string.Empty;

			if (@this.IsGenericType)
			{
				var genericArguments = new List<string>();
				var genericConstraints = new List<string>();

				foreach (var argument in @this.GetGenericArguments())
				{
					genericArguments.Add($"{argument.GetFullName(this.namespaces)}");

					if(argument.IsGenericParameter && argument.GenericParameterAttributes != 0)
					{
						var constraint = argument.GetConstraints(this.namespaces);

						if (!string.IsNullOrWhiteSpace(constraint))
						{
							genericConstraints.Add(constraint);
						}
					}
				}

				arguments = $"<{string.Join(", ", genericArguments)}>";
				// TODO: This should not add a space in front. The Maker class
				// should adjust the constraints to have a space in front.
				constraints = genericConstraints.Count == 0 ?
					string.Empty : $"{string.Join(" ", genericConstraints)}";
			}

			return (arguments, constraints);
		}

		[Benchmark(Baseline = true)]
		public (string arguments, string constraints) GetGenericArgumentsNewWay()
		{
			var @this = this.Type;

			if (@this.IsGenericType)
			{
				var argumentTypes = @this.GetGenericArguments();
				var arguments = new string[argumentTypes.Length];
				var constraints = new string[argumentTypes.Count(_ => _.GenericParameterAttributes != 0)];
				var constraintCount = 0;

				for (var i = 0; i < arguments.Length; i++)
				{
					var argument = argumentTypes[i];
					arguments[i] = ($"{argument.GetFullName(this.namespaces)}");

					if(constraints.Length > 0 && argument.GenericParameterAttributes != 0)
					{
						constraints[constraintCount] = argument.GetConstraints(this.namespaces);
						constraintCount++;
					}
				}

				return ($"<{string.Join(", ", arguments)}>",
					constraints.Length == 0 ? string.Empty : $"{string.Join(" ", constraints)}");
			}

			return (string.Empty, string.Empty);
		}

		public class ComplexGenericType<T, U, V, X>
			where U : struct
			where X : new()
		{ }
	}
}
