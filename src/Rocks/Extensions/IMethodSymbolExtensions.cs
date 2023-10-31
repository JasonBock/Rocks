using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Diagnostics;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class IMethodSymbolExtensions
{
	private const string DoesNotReturnAttributeName =
		"global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute";

	internal static ImmutableArray<Diagnostic> GetObsoleteDiagnostics(
		this IMethodSymbol self, InvocationExpressionSyntax invocation, INamedTypeSymbol obsoleteAttribute)
	{
		var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
		
		if (self.Parameters.Any(_ => _.Type.IsObsolete(obsoleteAttribute)) ||
			self.TypeParameters.Any(_ => _.IsObsolete(obsoleteAttribute)) ||
			!self.ReturnsVoid && self.ReturnType.IsObsolete(obsoleteAttribute))
		{
			diagnostics.Add(MemberUsesObsoleteTypeDiagnostic.Create(invocation, self));
		}

		return diagnostics.ToImmutable();
	}

	internal static bool CanBeSeenByContainingAssembly(this IMethodSymbol self, IAssemblySymbol assembly) =>
		((ISymbol)self).CanBeSeenByContainingAssembly(assembly) &&
			self.Parameters.All(_ => _.Type.CanBeSeenByContainingAssembly(assembly)) &&
			self.TypeParameters.All(_ => _.CanBeSeenByContainingAssembly(assembly)) &&
			(self.ReturnsVoid || self.ReturnType.CanBeSeenByContainingAssembly(assembly));

   internal static bool IsMarkedWithDoesNotReturn(this IMethodSymbol self) => 
		self.GetAttributes().Any(
		   _ => _.AttributeClass?.GetFullyQualifiedName() == IMethodSymbolExtensions.DoesNotReturnAttributeName);

   internal static bool RequiresProjectedDelegate(this IMethodSymbol self) =>
		self.Parameters.Length > 16 ||
		self.Parameters.Any(_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out || _.Type.IsEsoteric()) ||
			!self.ReturnsVoid && self.ReturnType.IsEsoteric();

	/// <summary>
	/// This is needed because if a method has a generic parameter that is used 
	/// either with a method parameter or the return value and declares the type to be nullable,
	/// the override must create a <c>where T : default</c> constraint.
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
				// TODO: This is starting to get convoluted.
				// Arguably, it would be good to have one method that gets
				// all constraints for a method, rather than doing it via
				// GetConstraints() and GetDefaultConstraints().
				if (typeParameter.HasReferenceTypeConstraint)
				{
					builder.Add($"where {typeParameter.GetName()} : class");
				}
				else if (typeParameter.HasValueTypeConstraint)
				{
					builder.Add($"where {typeParameter.GetName()} : struct");
				}
				else if (self.Parameters.Any(_ => _.Type.Equals(typeParameter) && _.NullableAnnotation == NullableAnnotation.Annotated) ||
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
			$"<{string.Join(", ", self.TypeArguments.Select(_ => _.GetFullyQualifiedName()))}>" : string.Empty;
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
			if (self.TypeParameters.Length != other.TypeParameters.Length)
			{
				return MethodMatch.None;
			}

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

				if (selfParameter.Type.WithNullableAnnotation(NullableAnnotation.NotAnnotated).GetFullyQualifiedName() !=
					otherParameter.Type.WithNullableAnnotation(NullableAnnotation.NotAnnotated).GetFullyQualifiedName() ||
					!(selfParameter.RefKind == otherParameter.RefKind ||
						(selfParameter.RefKind == RefKind.Ref && otherParameter.RefKind == RefKind.Out) ||
						(selfParameter.RefKind == RefKind.Out && otherParameter.RefKind == RefKind.Ref)) ||
					selfParameter.IsParams != otherParameter.IsParams)
				{
					return MethodMatch.None;
				}
			}

			return self.ReturnType.WithNullableAnnotation(NullableAnnotation.NotAnnotated).GetFullyQualifiedName() ==
				other.ReturnType.WithNullableAnnotation(NullableAnnotation.NotAnnotated).GetFullyQualifiedName() ?
				MethodMatch.Exact : MethodMatch.DifferByReturnTypeOnly;
		}
	}
}