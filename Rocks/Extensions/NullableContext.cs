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

		internal NullableContext() => 
			(this.flags, this.index) = (Array.Empty<byte>(), 0);

		private static byte[] GetNullableFlags(ParameterInfo parameter)
		{
			static byte[]? GetNullableContextValue(IList<CustomAttributeData> data)
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

			if(parameter.ParameterType.IsValueType && !parameter.ParameterType.IsGenericType)
			{
				return Array.Empty<byte>();
			}

			foreach (var attribute in parameter.GetCustomAttributesData())
			{
				if (attribute.IsNullableAttribute())
				{
					var nullableCtor = attribute.ConstructorArguments[0];

					return nullableCtor.ArgumentType.IsArray switch
					{
						true => ((IList<CustomAttributeTypedArgument>)nullableCtor.Value).Select(_ => (byte)_.Value).ToArray(),
						_ => new byte[] { (byte)nullableCtor.Value }
					};
				}
			}

			return GetNullableContextValue(parameter.Member.GetCustomAttributesData()) ??
				GetNullableContextValue(parameter.Member.DeclaringType.GetCustomAttributesData()) ??
				GetNullableContextValue(parameter.Member.DeclaringType.Module.GetCustomAttributesData()) ??
				Array.Empty<byte>();
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