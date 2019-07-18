using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	// https://github.com/dotnet/roslyn/blob/master/docs/features/nullable-metadata.md
	internal sealed class NullableContext
	{
		internal const byte Oblivious = 0;
		internal const byte NotAnnotated = 1;
		internal const byte Annotated = 2;

		private readonly byte[] flags;
		private int index;

		internal NullableContext(ParameterInfo parameter)
		{
			if (parameter == null) throw new ArgumentNullException(nameof(parameter));
			(this.flags, this.index) = (NullableContext.GetNullableFlags(parameter), 0);
		}

		internal NullableContext() => 
			(this.flags, this.index) = (Array.Empty<byte>(), 0);

		private static byte[] GetNullableFlags(ParameterInfo parameter)
		{
			static byte? GetContextValue(IList<CustomAttributeData> data)
			{
				foreach (var attribute in data)
				{
					if (attribute.IsNullableContextAttribute())
					{
						return (byte)attribute.ConstructorArguments[0].Value;
					}
				}

				return null;
			}

			// TODO: If the parameter type is a value type that has no generic values, I think
			// I can immediately return with an empty array.

			var found = false;

			foreach (var attribute in parameter.GetCustomAttributesData())
			{
				if (attribute.IsNullableAttribute())
				{
					found = true;
					var nullableCtor = attribute.ConstructorArguments[0];

					// https://codeblog.jonskeet.uk/2019/02/10/nullableattribute-and-c-8/ and
					// https://blog.rsuter.com/the-output-of-nullable-reference-types-and-how-to-reflect-it/
					return nullableCtor.ArgumentType.IsArray switch
					{
						true => ((IList<CustomAttributeTypedArgument>)nullableCtor.Value).Select(_ => (byte)_.Value).ToArray(),
						_ => new byte[] { (byte)nullableCtor.Value }
					};
				}
			}

			if(!found)
			{
				var methodContextValue = GetContextValue(parameter.Member.GetCustomAttributesData());

				if(methodContextValue != null)
				{
					return new byte[] { methodContextValue.Value };
				}
				else
				{
					var typeContextValue = GetContextValue(parameter.Member.DeclaringType.GetCustomAttributesData());

					if (typeContextValue != null)
					{
						return new byte[] { typeContextValue.Value };
					}
					else
					{
						var moduleContextValue = GetContextValue(parameter.Member.DeclaringType.Module.GetCustomAttributesData());

						if (moduleContextValue != null)
						{
							return new byte[] { moduleContextValue.Value };
						}
					}
				}
			}

			return Array.Empty<byte>();
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