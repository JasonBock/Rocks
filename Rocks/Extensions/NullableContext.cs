using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	// https://github.com/dotnet/roslyn/blob/master/docs/features/nullable-metadata.md
	// https://codeblog.jonskeet.uk/2019/02/10/nullableattribute-and-c-8/
	// https://blog.rsuter.com/the-output-of-nullable-reference-types-and-how-to-reflect-it/
	internal sealed class NullableContext
	{
		internal const byte Oblivious = 0;
		internal const byte NotAnnotated = 1;
		internal const byte Annotated = 2;

		private readonly byte[] flags;
		private int index;

		internal NullableContext(ParameterInfo parameter)
		{
			if (parameter == null) { throw new ArgumentNullException(nameof(parameter)); }
			(this.flags, this.index) = (NullableContext.GetNullableFlags(parameter), 0);
		}

		internal NullableContext(Type type)
		{
			if (type == null) { throw new ArgumentNullException(nameof(type)); }
			if (!type.IsGenericParameter) { throw new NotSupportedException("Only generic parameter types are accepted."); };

			(this.flags, this.index) = (NullableContext.GetNullableFlags(type), 0);
		}

		internal NullableContext() =>
			(this.flags, this.index) = (Array.Empty<byte>(), 0);

		private static byte[]? GetNullableContextValue(IList<CustomAttributeData> data)
		{
			foreach (var attribute in data)
			{
				if (attribute.IsNullableContextAttribute())
				{
					return new[] { (byte)attribute.ConstructorArguments[0].Value };
				}
			}

			return null;
		}

		private static (bool, byte[]?) GetNullableFlags(IList<CustomAttributeData> attributes)
		{
			foreach (var attribute in attributes)
			{
				if (attribute.IsNullableAttribute())
				{
					var nullableCtor = attribute.ConstructorArguments[0];

					return nullableCtor.ArgumentType.IsArray switch
					{
						true => (true, ((IList<CustomAttributeTypedArgument>)nullableCtor.Value).Select(_ => (byte)_.Value).ToArray()),
						_ => (true, new byte[] { (byte)nullableCtor.Value })
					};
				}
			}

			return (false, null);
		}

		private static byte[] GetNullableFlags(Type type) => 
			GetNullableFlags(type.GetCustomAttributesData()) switch
			{
				(true, var flags) => flags!,
				_ => NullableContext.GetNullableContextValue(type.DeclaringMethod?.GetCustomAttributesData() ?? Array.Empty<CustomAttributeData>()) ??
					NullableContext.GetNullableContextValue(type.DeclaringType.GetCustomAttributesData()) ??
					Array.Empty<byte>()
			};

		private static byte[] GetNullableFlags(ParameterInfo parameter)
		{
			if (parameter.ParameterType.IsValueType && !parameter.ParameterType.IsGenericType)
			{
				return Array.Empty<byte>();
			}

			return GetNullableFlags(parameter.GetCustomAttributesData()) switch
			{
				// TODO: May need to recursively descend on DeclaringType
				(true, var flags) => flags!,
				_ => NullableContext.GetNullableContextValue(parameter.Member.GetCustomAttributesData()) ??
					NullableContext.GetNullableContextValue(parameter.Member.DeclaringType.GetCustomAttributesData()) ??
					Array.Empty<byte>()
			};
		}

		internal byte GetNextFlag()
		{
			var flag = this.flags.Length == 0 ? NullableContext.NotAnnotated : this.flags[this.index];
			this.index = this.flags.Length <= 1 ? 0 : this.index + 1;
			return flag;
		}

		public int Count => this.flags.Length;
	}
}