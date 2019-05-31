using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal sealed class NullableContext
	{
		internal const byte NeverNullable = 0;
		internal const byte NotNullable = 1;
		internal const byte Nullable = 2;

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
			foreach (var attribute in parameter.GetCustomAttributesData())
			{
				if (attribute.IsNullableAttribute())
				{
					if (attribute.ConstructorArguments.Count > 0)
					{
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
			}

			return Array.Empty<byte>();
		}

		internal byte GetNextFlag()
		{
			var flag = this.flags.Length == 0 ? NullableContext.NotNullable : this.flags[this.index];
			this.index = this.flags.Length <= 1 ? 0 : this.index + 1;
			return flag;
		}

		public int Count => this.flags.Length;
	}
}