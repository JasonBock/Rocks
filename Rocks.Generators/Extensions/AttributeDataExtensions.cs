using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Extensions
{
	internal static class AttributeDataExtensions
	{
		internal static string GetDescription(this AttributeData self)
		{
			static string GetTypedConstantValue(TypedConstant value) =>
				value.Kind switch
				{
					TypedConstantKind.Primitive => GetValue(value.Value),
					TypedConstantKind.Type => $"typeof({((INamedTypeSymbol)value.Value!).ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)})",
					TypedConstantKind.Array => $"new[] {{ {string.Join(", ", value.Values.Select(v => GetValue(v)))} }}",
					TypedConstantKind.Enum => $"({value.Type!.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}){value.Value}",
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

			var name = self.AttributeClass!.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
				.Replace("Attribute", string.Empty);
			var constructorArguments = string.Empty;

			if (self.ConstructorArguments.Length > 0)
			{
				var arguments = self.ConstructorArguments.Select(_ => GetTypedConstantValue(_));
				constructorArguments = $"({string.Join(", ", arguments)})";
			}

			return $"{name}{constructorArguments}";
		}

		internal static string GetDescription(this ImmutableArray<AttributeData> self, AttributeTargets? target = null) => 
			$"[{(!string.IsNullOrWhiteSpace(target.GetTarget()) ? $"{target.GetTarget()}: " : string.Empty)}{string.Join(", ", self.Select(_ => _.GetDescription()))}]";

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
}