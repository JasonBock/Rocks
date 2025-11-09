using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Rocks.Analysis.Extensions;

internal static class AttributeDataExtensions
{
	internal static string GetDescription(this AttributeData self, Compilation compilation)
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

		var name = self.AttributeClass!.GetFullyQualifiedName(compilation);

		if (self.ConstructorArguments.Length == 0 && self.NamedArguments.Length == 0)
		{
			return name;
		}
		else
		{
			var argumentParts = new List<string>(self.ConstructorArguments.Length + self.NamedArguments.Length);

			if (self.ConstructorArguments.Length > 0)
			{
				argumentParts.AddRange(self.ConstructorArguments.Select(_ => GetTypedConstantValue(_, compilation)));
			}

			if (self.NamedArguments.Length > 0)
			{
				argumentParts.AddRange(self.NamedArguments.Select(_ => $"{_.Key} = {GetTypedConstantValue(_.Value, compilation)}"));
			}

			return $"{name}{$"({string.Join(", ", argumentParts)})"}";
		}
	}

	internal static string GetDescription(this ImmutableArray<AttributeData> self, Compilation compilation,
		AttributeTargets? target = null,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation = RequiresExplicitInterfaceImplementation.No,
		bool isParameterWithOptionalValue = false)
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
		// * DefaultMemberAttribute - This would only be on a type, but it can't be included if the type has indexers. It's just easier to not include it - CS0646.
		// * UnscopedRefAttribute - Essentially, this can't be included on implemented mock members because they're non-virtual (I think) - CS9101.
		//
		// There are a handful of attributes we can't "see" when targeting NS 2.0 
		// that we can't emit. Therefore, we do a string comparison against the FQN
		// for these attributes:
		// * EnumeratorCancellationAttribute
		// * AsyncIteratorStateMachineAttribute
		// * NullableAttribute
		// * NullableContextAttribute
		//
		// For types (i.e. target.Value.HasFlag(AttributeTargets.Class) == true), we will only "leak"
		// ObsoleteAttribute.
		//
		// If MemberNotNullWhenAttribute exists, we don't need to include on the overridden member - CS8776

		var compilerGeneratedAttribute = compilation.GetTypeByMetadataName(typeof(CompilerGeneratedAttribute).FullName);
		var iteratorStateMachineAttribute = compilation.GetTypeByMetadataName(typeof(IteratorStateMachineAttribute).FullName);
		var asyncStateMachineAttribute = compilation.GetTypeByMetadataName(typeof(AsyncStateMachineAttribute).FullName);
		var dynamicAttribute = compilation.GetTypeByMetadataName(typeof(DynamicAttribute).FullName);
		var tupleElementNamesAttribute = compilation.GetTypeByMetadataName(typeof(TupleElementNamesAttribute).FullName);
		var conditionalAttribute = compilation.GetTypeByMetadataName(typeof(ConditionalAttribute).FullName);
		var defaultMemberAttribute = compilation.GetTypeByMetadataName(typeof(DefaultMemberAttribute).FullName);
		var attributeUsageAttribute = compilation.GetTypeByMetadataName(typeof(AttributeUsageAttribute).FullName);
		var obsoleteAttribute = compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName);

		// If a member will be implemented explicitly, and the target type is a parameter,
		// and the parameter is optional, we can't emit these 4 "caller" attributes.
		var callerFilePathAttribute = compilation.GetTypeByMetadataName(typeof(CallerFilePathAttribute).FullName);
		var callerLineNumberAttribute = compilation.GetTypeByMetadataName(typeof(CallerLineNumberAttribute).FullName);
		var callerMemberNameAttribute = compilation.GetTypeByMetadataName(typeof(CallerMemberNameAttribute).FullName);
		var callerArgumentExpressionAttribute = compilation.GetTypeByMetadataName(typeof(CallerArgumentExpressionAttribute).FullName);

		const string enumeratorCancellationAttribute = "global::System.Runtime.CompilerServices.EnumeratorCancellationAttribute";
		const string asyncIteratorStateMachineAttribute = "global::System.Runtime.CompilerServices.AsyncIteratorStateMachineAttribute";
		const string nullableAttribute = "global::System.Runtime.CompilerServices.NullableAttribute";
		const string nullableContextAttribute = "global::System.Runtime.CompilerServices.NullableContextAttribute";
		const string unscopedRefAttribute = "global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute";
		const string memberNotNullWhenAttribute = "global::System.Diagnostics.CodeAnalysis.MemberNotNullWhenAttribute";

		var attributes = self.Where(
			_ => _.AttributeClass is not null &&
				(!target.HasValue || !target.Value.HasFlag(AttributeTargets.Class) ||
					_.AttributeClass.Equals(obsoleteAttribute, SymbolEqualityComparer.Default)) &&
				_.AttributeClass.CanBeSeenByContainingAssembly(compilation.Assembly, compilation) &&
				!_.AttributeClass.Equals(compilerGeneratedAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(iteratorStateMachineAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(asyncStateMachineAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(dynamicAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(tupleElementNamesAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(conditionalAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(defaultMemberAttribute, SymbolEqualityComparer.Default) &&
				_.AttributeClass.GetFullyQualifiedName(compilation) != enumeratorCancellationAttribute &&
				_.AttributeClass.GetFullyQualifiedName(compilation) != asyncIteratorStateMachineAttribute &&
				_.AttributeClass.GetFullyQualifiedName(compilation) != nullableAttribute &&
				_.AttributeClass.GetFullyQualifiedName(compilation) != nullableContextAttribute &&
				_.AttributeClass.GetFullyQualifiedName(compilation) != unscopedRefAttribute &&
				_.AttributeClass.GetFullyQualifiedName(compilation) != memberNotNullWhenAttribute).ToImmutableArray();

		// Do another pass where we eliminate "caller" attribute if necessary.
		if (requiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
			isParameterWithOptionalValue)
		{
			attributes = [.. attributes.Where(_ =>
				_.AttributeClass is not null &&
				!_.AttributeClass.Equals(callerFilePathAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(callerLineNumberAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(callerMemberNameAttribute, SymbolEqualityComparer.Default) &&
				!_.AttributeClass.Equals(callerArgumentExpressionAttribute, SymbolEqualityComparer.Default))];
		}

		if (attributes.Length == 0)
		{
			return string.Empty;
		}
		else
		{
			return $"[{(!string.IsNullOrWhiteSpace(target.GetTarget()) ? $"{target.GetTarget()}: " : string.Empty)}{string.Join(", ", attributes.Select(_ => _.GetDescription(compilation)))}]";
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