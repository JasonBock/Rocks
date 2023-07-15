﻿using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Rocks.Extensions;

internal static class AttributeDataExtensions
{
	internal static ImmutableHashSet<INamespaceSymbol> GetNamespaces(this AttributeData self)
	{
		static IEnumerable<INamespaceSymbol> GetNamespacesForValue(TypedConstant value)
		{
			if (value.Kind == TypedConstantKind.Primitive || value.Kind == TypedConstantKind.Enum)
			{
				yield return value.Type!.ContainingNamespace;
			}
			else if (value.Kind == TypedConstantKind.Type)
			{
				yield return ((INamedTypeSymbol)value.Value!).ContainingNamespace;
			}
			else if (value.Kind == TypedConstantKind.Array)
			{
				yield return value.Type!.ContainingNamespace;

				foreach (var arrayValue in value.Values)
				{
					foreach (var arrayValueNamespace in GetNamespacesForValue(arrayValue))
					{
						yield return arrayValueNamespace;
					}
				}
			}
		}

		var namespaces = ImmutableHashSet.CreateBuilder<INamespaceSymbol>();

		namespaces.Add(self.AttributeClass!.ContainingNamespace);
		namespaces.AddRange(self.ConstructorArguments.SelectMany(_ => GetNamespacesForValue(_)));
		namespaces.AddRange(self.NamedArguments.SelectMany(_ => GetNamespacesForValue(_.Value)));

		return namespaces.ToImmutable();
	}

	internal static string GetDescription(this AttributeData self)
	{
		static string GetTypedConstantValue(TypedConstant value) =>
			value.Kind switch
			{
				TypedConstantKind.Primitive => GetValue(value.Value),
				TypedConstantKind.Type => $"typeof({((INamedTypeSymbol)value.Value!).GetFullyQualifiedName()})",
				TypedConstantKind.Array => $"new[] {{ {string.Join(", ", value.Values.Select(v => GetValue(v)))} }}",
				TypedConstantKind.Enum => $"({value.Type!.GetFullyQualifiedName()})({value.Value})",
				_ => value.Value?.ToString() ?? string.Empty
			};

		static string GetValue(object? value) =>
			value switch
			{
				TypedConstant tc => GetTypedConstantValue(tc),
				string s => $"\"{s}\"",
				bool b => $"{(b ? "true" : "false")}",
				_ => value?.ToString() ?? string.Empty
			};

		var name = self.AttributeClass!.GetFullyQualifiedName();
		var argumentParts = new List<string>();

		if (self.ConstructorArguments.Length > 0)
		{
			argumentParts.AddRange(self.ConstructorArguments.Select(_ => GetTypedConstantValue(_)));
		}

		if (self.NamedArguments.Length > 0)
		{
			argumentParts.AddRange(self.NamedArguments.Select(_ => $"{_.Key} = {GetTypedConstantValue(_.Value)}"));
		}

		var arguments = argumentParts.Count > 0 ? $"({string.Join(", ", argumentParts)})" : string.Empty;
		return $"{name}{arguments}";
	}

	internal static string GetDescription(this ImmutableArray<AttributeData> self, Compilation compilation, AttributeTargets? target = null)
	{
		if (self.Length == 0)
		{
			return string.Empty;
		}

		// We can't emit attributes that are:
		// * Compiler-generated
		// * Not visible to the current compilation (e.g. IntrinsicAttribute).
		// * IteratorStateMachineAttribute (see https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.iteratorstatemachineattribute#remarks)
		// * AsyncStateMachineAttribute (see https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.asyncstatemachineattribute#remarks)
		// * DynamicAttribute (CS1970 - the error is "Do not use 'System.Runtime.CompilerServices.DynamicAttribute'. Use the 'dynamic' keyword instead." - https://sourceroslyn.io/#Microsoft.CodeAnalysis.CSharp/Symbols/Source/SourcePropertySymbolBase.cs,1276)
		// * EnumeratorCancellationAttribute - I can't reference the type because it's not in .NET Standard 2.0 :(, but I have to filter it out.
		// * TupleElementNamesAttribute - If a base member has this, it's because the compiler emitted it. Code can't do that - CS8138
		var compilerGeneratedAttribute = compilation.GetTypeByMetadataName(typeof(CompilerGeneratedAttribute).FullName);
		var iteratorStateMachineAttribute = compilation.GetTypeByMetadataName(typeof(IteratorStateMachineAttribute).FullName);
		var asyncStateMachineAttribute = compilation.GetTypeByMetadataName(typeof(AsyncStateMachineAttribute).FullName);
		var dynamicAttribute = compilation.GetTypeByMetadataName(typeof(DynamicAttribute).FullName);
		var tupleElementNamesAttribute = compilation.GetTypeByMetadataName(typeof(TupleElementNamesAttribute).FullName);
		var conditionalAttribute = compilation.GetTypeByMetadataName(typeof(ConditionalAttribute).FullName);
		const string enumeratorCancellationAttribute = "global::System.Runtime.CompilerServices.EnumeratorCancellationAttribute";
		const string asyncIteratorStateMachineAttribute = "global::System.Runtime.CompilerServices.AsyncIteratorStateMachineAttribute";

		var attributes = self.Where(
			_ => _.AttributeClass is not null &&
				_.AttributeClass.CanBeSeenByContainingAssembly(compilation.Assembly) &&
				!_.AttributeClass.Equals(compilerGeneratedAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(iteratorStateMachineAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(asyncStateMachineAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(dynamicAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(tupleElementNamesAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(conditionalAttribute, SymbolEqualityComparer.Default) &&
				_.AttributeClass.GetFullyQualifiedName() != enumeratorCancellationAttribute &&
				_.AttributeClass.GetFullyQualifiedName() != asyncIteratorStateMachineAttribute).ToImmutableArray();

		if (attributes.Length == 0)
		{
			return string.Empty;
		}
		else
		{
			return $"[{(!string.IsNullOrWhiteSpace(target.GetTarget()) ? $"{target.GetTarget()}: " : string.Empty)}{string.Join(", ", attributes.Select(_ => _.GetDescription()))}]";
		}
	}

	private static string GetTarget(this AttributeTargets? target) =>
		target switch
		{
			AttributeTargets.Assembly => "assembly",
			AttributeTargets.Class or AttributeTargets.Delegate or AttributeTargets.Enum or
				AttributeTargets.Interface or AttributeTargets.Struct => "type",
			AttributeTargets.Constructor or AttributeTargets.Method => "method",
			AttributeTargets.Event => "event",
			AttributeTargets.Field => "field",
			AttributeTargets.GenericParameter or AttributeTargets.Parameter => "param",
			AttributeTargets.Module => "module",
			AttributeTargets.Property => "property",
			AttributeTargets.ReturnValue => "return",
			_ => string.Empty
		};
}