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
			var name = self.AttributeClass!.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).Replace("Attribute", string.Empty);
			var constructorArguments = string.Empty;

			if (self.ConstructorArguments.Length > 0)
			{
				var arguments = self.ConstructorArguments.Select(_ => _.Value switch
				{
					string => $"\"{_.Value}\"",
					INamedTypeSymbol named => $"typeof({named.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)})",
					_ => _.Value
				});

				constructorArguments = $"({string.Join(", ", arguments)})";
			}

			return $"{name}{constructorArguments}";
		}

		internal static string GetDescription(this ImmutableArray<AttributeData> self, AttributeTargets? target = null) => 
			$"[{(!string.IsNullOrWhiteSpace(target.GetTarget()) ? $"{target}: " : string.Empty)}{string.Join(", ", self.Select(_ => _.GetDescription()))}]";

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