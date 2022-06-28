using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class IMethodSymbolExtensions
{
	internal static bool IsMarkedWithDoesNotReturn(this IMethodSymbol self, Compilation compilation)
	{
		// I would LOVE to be able to use typeof(...) for DoesNotReturnAttribute
		// but...it shows up as internal in a NS 2.0 project. Ugh.
		var doesNotReturnAttributeType = Type.GetType("System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute", false);

		if (doesNotReturnAttributeType is not null)
		{
			var doesNotReturnAttribute = compilation.GetTypeByMetadataName(doesNotReturnAttributeType.FullName);

			return self.GetAttributes().Any(
				_ => _.AttributeClass?.Equals(doesNotReturnAttribute, SymbolEqualityComparer.Default) ?? false);
		}
		else
		{
			return false;
		}
	}

	internal static bool RequiresProjectedDelegate(this IMethodSymbol self) =>
		self.Parameters.Any(_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out || _.Type.IsEsoteric()) ||
			!self.ReturnsVoid && self.ReturnType.IsEsoteric();

	/// <summary>
	/// This is needed because if a method has a generic parameter that is used 
	/// either with a method parameter or the return value and declares the type to be nullable,
	/// the override must create a <code>where T : default</code> constraint.
	/// Otherwise, CS0508 and CS0453 will occur.
	/// </summary>
	/// <param name="self">The target method.</param>
	/// <returns>A list of default constraints.</returns>
	internal static ImmutableArray<string> GetDefaultConstraints(this IMethodSymbol self)
	{
		var builder = ImmutableArray.CreateBuilder<string>();

		if (self.TypeParameters.Length > 0)
		{
			foreach (var typeParameter in self.TypeParameters)
			{
				if (self.Parameters.Any(_ => _.Type.Equals(typeParameter) && _.NullableAnnotation == NullableAnnotation.Annotated) ||
					(!self.ReturnsVoid && self.ReturnType.Equals(typeParameter) && self.ReturnType.NullableAnnotation == NullableAnnotation.Annotated))
				{
					builder.Add($"where {typeParameter.GetName()} : default");
				}
			}
		}

		return builder.ToImmutable();
	}

	internal static bool IsUnsafe(this IMethodSymbol self) =>
		self.Parameters.Any(_ => _.Type.IsPointer()) || (!self.ReturnsVoid && self.ReturnType.IsPointer());

	internal static string GetName(this IMethodSymbol self, MethodNameOption option = MethodNameOption.IncludeGenerics, string extendedName = "")
	{
		var generics = option == MethodNameOption.IncludeGenerics && self.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", self.TypeArguments.Select(_ => _.GetName()))}>" : string.Empty;
		return $"{self.Name}{extendedName}{generics}";
	}

	internal static ImmutableArray<string> GetConstraints(this IMethodSymbol self)
	{
		var constraints = new List<string>();

		if (self.TypeParameters.Length > 0)
		{
			for (var i = 0; i < self.TypeParameters.Length; i++)
			{
				var typeParameter = self.TypeParameters[i];

				if (typeParameter.Equals(self.TypeArguments[i]))
				{
					constraints.Add(typeParameter.GetConstraints());
				}
			}
		}

		return constraints.Where(_ => !string.IsNullOrWhiteSpace(_)).ToImmutableArray();
	}

	internal static ImmutableHashSet<INamespaceSymbol> GetNamespaces(this IMethodSymbol self)
	{
		static IEnumerable<INamespaceSymbol> GetParameterNamespaces(IParameterSymbol parameter)
		{
			foreach (var parameterTypeNamespace in parameter.Type.GetNamespaces())
			{
				yield return parameterTypeNamespace;
			}

			foreach (var attributeNamespace in parameter.GetAttributes().SelectMany(_ => _.GetNamespaces()))
			{
				yield return attributeNamespace;
			}
		}

		var namespaces = ImmutableHashSet.CreateBuilder<INamespaceSymbol>();

		if (!self.ReturnsVoid)
		{
			namespaces.AddRange(self.ReturnType.GetNamespaces());
			namespaces.AddRange(self.GetReturnTypeAttributes().SelectMany(_ => _.GetNamespaces()));
		}

		namespaces.AddRange(self.GetAttributes().SelectMany(_ => _.GetNamespaces()));
		namespaces.AddRange(self.Parameters.SelectMany(_ => GetParameterNamespaces(_)));

		return namespaces.ToImmutable();
	}

	internal static MethodMatch Match(this IMethodSymbol self, IMethodSymbol other)
	{
		if (self.Name != other.Name)
		{
			return MethodMatch.None;
		}
		else
		{
			var selfParameters = self.Parameters;
			var otherParameters = other.Parameters;

			if (selfParameters.Length != otherParameters.Length)
			{
				return MethodMatch.None;
			}

			for (var i = 0; i < selfParameters.Length; i++)
			{
				var selfParameter = selfParameters[i];
				var otherParameter = otherParameters[i];

				if (!selfParameter.Type.Equals(otherParameter.Type, SymbolEqualityComparer.Default) ||
					!(selfParameter.RefKind == otherParameter.RefKind ||
						(selfParameter.RefKind == RefKind.Ref && otherParameter.RefKind == RefKind.Out) ||
						(selfParameter.RefKind == RefKind.Out && otherParameter.RefKind == RefKind.Ref)) ||
					selfParameter.IsParams != otherParameter.IsParams)
				{
					return MethodMatch.None;
				}
			}

			return self.ReturnType.Equals(other.ReturnType, SymbolEqualityComparer.Default) ?
				MethodMatch.Exact : MethodMatch.DifferByReturnTypeOnly;
		}
	}
}