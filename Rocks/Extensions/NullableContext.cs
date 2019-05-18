using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal sealed class NullableContext
	{
		internal const byte NotNullable = 1;
		internal const byte Nullable = 2;

		private readonly byte[] flags;
		private readonly int index;

		internal NullableContext(ParameterInfo parameter)
			: this(NullableContext.GetNullableFlags(parameter), 0) { }

		internal NullableContext(byte[] flags)
			: this(flags, 0) { }

		private NullableContext(byte[] flags, int index) =>
			(this.flags, this.index) = (flags, index);

		private static byte[] GetNullableFlags(ParameterInfo parameter)
		{
			if (!parameter.ParameterType.IsValueType)
			{
				foreach (var attribute in parameter.GetCustomAttributesData())
				{
					if (attribute.IsNullableAttribute())
					{
						if (attribute.ConstructorArguments.Count > 0)
						{
							var nullableCtor = attribute.ConstructorArguments[0];

							//if(nullableCtor.ArgumentType.IsArray)
							//{
							//	return ((IList<byte>)nullableCtor.Value).ToArray();
							//}
							//else
							//{
							//	return new byte[] { (byte)nullableCtor.Value };
							//}

							// https://codeblog.jonskeet.uk/2019/02/10/nullableattribute-and-c-8/
							return nullableCtor.ArgumentType.IsArray switch
							{
								true => ((IList<CustomAttributeTypedArgument>)nullableCtor.Value).Select(_ => (byte)_.Value).ToArray(),
								_ => new byte[] { (byte)nullableCtor.Value }
							};
						}
					}
				}
			}

			return Array.Empty<byte>();
		}

		internal (byte, NullableContext) GetNextState() =>
			(this.flags.Length == 0 ? NullableContext.NotNullable : this.flags[this.index],
				new NullableContext(this.flags, this.flags.Length <= 1 ? 0 : this.index + 1));
	}
}