using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Extensions;
using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;

namespace Rocks.Performance;

[MemoryDiagnoser]
public class AttributeGetDescription
{
	[Benchmark(Baseline = true)]
	[ArgumentsSource(nameof(GetAttributeInformation))]
	public string GetDescriptionNoOptimizationAttempts(AttributeInformation information)
	{
		static string GetTypedConstantValue(TypedConstant value, Compilation compilation) =>
		  value.Kind switch
		  {
			  TypedConstantKind.Primitive => GetValue(value.Value, compilation),
			  TypedConstantKind.Type => $"typeof({((INamedTypeSymbol)value.Value!).GetFullyQualifiedName(compilation)})",
			  TypedConstantKind.Array => $"new[] {{ {string.Join(", ", value.Values.Select(v => GetValue(v, compilation)))} }}",
			  TypedConstantKind.Enum => $"({value.Type!.GetFullyQualifiedName(compilation)})({value.Value})",
			  _ => value.Value?.ToString() ?? string.Empty
		  };

#pragma warning disable CA1307 // Specify StringComparison for clarity
		static string GetValue(object? value, Compilation compilation) =>
			value switch
			{
				TypedConstant tc => GetTypedConstantValue(tc, compilation),
				string s =>
				$"""
				"{s.Replace("\'", "\\\'").Replace("\"", "\\\"").Replace("\a", "\\a")
						.Replace("\b", "\\b").Replace("\f", "\\f").Replace("\n", "\\n")
						.Replace("\r", "\\r").Replace("\t", "\\t").Replace("\v", "\\v")}"
				""",
				bool b => $"{(b ? "true" : "false")}",
				_ => value?.ToString() ?? string.Empty
			};
#pragma warning restore CA1307 // Specify StringComparison for clarity

		(var data, var compilation) = information;
		var name = data.AttributeClass!.GetFullyQualifiedName(compilation);
		var argumentParts = new List<string>();

		if (data.ConstructorArguments.Length > 0)
		{
			argumentParts.AddRange(data.ConstructorArguments.Select(_ => GetTypedConstantValue(_, compilation)));
		}

		if (data.NamedArguments.Length > 0)
		{
			argumentParts.AddRange(data.NamedArguments.Select(_ => $"{_.Key} = {GetTypedConstantValue(_.Value, compilation)}"));
		}

		var arguments = argumentParts.Count > 0 ? $"({string.Join(", ", argumentParts)})" : string.Empty;
		return $"{name}{arguments}";
	}

	[Benchmark]
	[ArgumentsSource(nameof(GetAttributeInformation))]
	public string GetDescriptionOptimizationsConcat(AttributeInformation information)
	{
		static string GetTypedConstantValue(TypedConstant value, Compilation compilation) =>
		  value.Kind switch
		  {
			  TypedConstantKind.Primitive => GetValue(value.Value, compilation),
			  TypedConstantKind.Type => $"typeof({((INamedTypeSymbol)value.Value!).GetFullyQualifiedName(compilation)})",
			  TypedConstantKind.Array => $"new[] {{ {string.Join(", ", value.Values.Select(v => GetValue(v, compilation)))} }}",
			  TypedConstantKind.Enum => $"({value.Type!.GetFullyQualifiedName(compilation)})({value.Value})",
			  _ => value.Value?.ToString() ?? string.Empty
		  };

#pragma warning disable CA1307 // Specify StringComparison for clarity
		static string GetValue(object? value, Compilation compilation) =>
			value switch
			{
				TypedConstant tc => GetTypedConstantValue(tc, compilation),
				string s =>
				$"""
				"{s.Replace("\'", "\\\'").Replace("\"", "\\\"").Replace("\a", "\\a")
						.Replace("\b", "\\b").Replace("\f", "\\f").Replace("\n", "\\n")
						.Replace("\r", "\\r").Replace("\t", "\\t").Replace("\v", "\\v")}"
				""",
				bool b => $"{(b ? "true" : "false")}",
				_ => value?.ToString() ?? string.Empty
			};
#pragma warning restore CA1307 // Specify StringComparison for clarity

		(var data, var compilation) = information;
		var name = data.AttributeClass!.GetFullyQualifiedName(compilation);

		if (data.ConstructorArguments.Length == 0 && data.NamedArguments.Length == 0)
		{
			return name;
		}
		else
		{
			var arguments = $"({string.Join(", ", data.ConstructorArguments.Select(_ => GetTypedConstantValue(_, compilation)).Concat(data.NamedArguments.Select(_ => $"{_.Key} = {GetTypedConstantValue(_.Value, compilation)}")))})";
			return $"{name}{arguments}";
		}
	}

	[Benchmark]
	[ArgumentsSource(nameof(GetAttributeInformation))]
	public string GetDescriptionOptimizationsPreallocateList(AttributeInformation information)
	{
		static string GetTypedConstantValue(TypedConstant value, Compilation compilation) =>
		  value.Kind switch
		  {
			  TypedConstantKind.Primitive => GetValue(value.Value, compilation),
			  TypedConstantKind.Type => $"typeof({((INamedTypeSymbol)value.Value!).GetFullyQualifiedName(compilation)})",
			  TypedConstantKind.Array => $"new[] {{ {string.Join(", ", value.Values.Select(v => GetValue(v, compilation)))} }}",
			  TypedConstantKind.Enum => $"({value.Type!.GetFullyQualifiedName(compilation)})({value.Value})",
			  _ => value.Value?.ToString() ?? string.Empty
		  };

#pragma warning disable CA1307 // Specify StringComparison for clarity
		static string GetValue(object? value, Compilation compilation) =>
			value switch
			{
				TypedConstant tc => GetTypedConstantValue(tc, compilation),
				string s =>
				$"""
				"{s.Replace("\'", "\\\'").Replace("\"", "\\\"").Replace("\a", "\\a")
						.Replace("\b", "\\b").Replace("\f", "\\f").Replace("\n", "\\n")
						.Replace("\r", "\\r").Replace("\t", "\\t").Replace("\v", "\\v")}"
				""",
				bool b => $"{(b ? "true" : "false")}",
				_ => value?.ToString() ?? string.Empty
			};
#pragma warning restore CA1307 // Specify StringComparison for clarity

		(var data, var compilation) = information;
		var name = data.AttributeClass!.GetFullyQualifiedName(compilation);

		if (data.ConstructorArguments.Length == 0 && data.NamedArguments.Length == 0)
		{
			return name;
		}
		else
		{
			var argumentParts = new List<string>(data.ConstructorArguments.Length + data.NamedArguments.Length);

			if (data.ConstructorArguments.Length > 0)
			{
				argumentParts.AddRange(data.ConstructorArguments.Select(_ => GetTypedConstantValue(_, compilation)));
			}

			if (data.NamedArguments.Length > 0)
			{
				argumentParts.AddRange(data.NamedArguments.Select(_ => $"{_.Key} = {GetTypedConstantValue(_.Value, compilation)}"));
			}

			return $"{name}{$"({string.Join(", ", argumentParts)})"}";
		}
	}

	public static IEnumerable<object> GetAttributeInformation()
	{
		static AttributeInformation Create(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.ToImmutableArray();
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<MethodDeclarationSyntax>().Single();
			var methodSymbol = model.GetDeclaredSymbol(methodSyntax)!;

			return new(methodSymbol.GetAttributes()[0], compilation);
		}

		yield return Create(
			"""
			using System;
			
			public sealed class TargetAttribute : Attribute { }
			
			public static class Test
			{
				[Target]
				public static void Run() { }
			}
			""");
		yield return Create(
			"""
			using System;
			
			public sealed class TargetAttribute : Attribute
			{
				public TargetAttribute(int constructor0) { }

				public int Property0 { get; init; }
			}
			
			public static class Test
			{
				[Target(0, Property0 = 1)]
				public static void Run() { }
			}
			""");
		yield return Create(
			"""
			using System;
			
			public sealed class TargetAttribute : Attribute
			{
				public TargetAttribute(int constructor0, int constructor1, int constructor2, int constructor3) { }

				public int Property0 { get; init; }
				public int Property1 { get; init; }
				public int Property2 { get; init; }
				public int Property3 { get; init; }
			}
			
			public static class Test
			{
				[Target(0, 1, 2, 3, Property0 = 4, Property1 = 5, Property2 = 6, Property3 = 7)]
				public static void Run() { }
			}
			""");
		yield return Create(
			"""
			using System;
			
			public sealed class TargetAttribute : Attribute
			{
				public TargetAttribute(int constructor0, int constructor1, int constructor2, int constructor3) { }
			}
			
			public static class Test
			{
				[Target(0, 1, 2, 3)]
				public static void Run() { }
			}
			""");
		yield return Create(
			"""
			using System;
			
			public sealed class TargetAttribute : Attribute
			{
				public int Property0 { get; init; }
				public int Property1 { get; init; }
				public int Property2 { get; init; }
				public int Property3 { get; init; }
			}
			
			public static class Test
			{
				[Target(Property0 = 4, Property1 = 5, Property2 = 6, Property3 = 7)]
				public static void Run() { }
			}
			""");
		yield return Create(
			"""
			using System;
			
			public sealed class TargetAttribute : Attribute
			{
				public TargetAttribute(string constructor0, string constructor1, string constructor2, string constructor3) { }

				public string Property0 { get; init; }
				public string Property1 { get; init; }
				public string Property2 { get; init; }
				public string Property3 { get; init; }
			}
			
			public static class Test
			{
				[Target("data 0", "data 1", "data 2", "data 3", Property0 = "data 4", Property1 = "data 5", Property2 = "data 6", Property3 = "data 7")]
				public static void Run() { }
			}
			""");
	}
}

public sealed record AttributeInformation(AttributeData Data, Compilation Compilation);