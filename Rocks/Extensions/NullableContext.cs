﻿using System;
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
		private int index;

		internal NullableContext(ParameterInfo parameter)
		{
			if (parameter == null) throw new ArgumentNullException(nameof(parameter));
			(this.flags, this.index) = (NullableContext.GetNullableFlags(parameter), 0);
		}

		internal NullableContext(byte[] flags)
		{
			if (flags == null) throw new ArgumentNullException(nameof(flags));
			(this.flags, this.index) = (flags, 0);
		}

		private static byte[] GetNullableFlags(ParameterInfo parameter)
		{
			foreach (var attribute in parameter.GetCustomAttributesData())
			{
				if (attribute.IsNullableAttribute())
				{
					if (attribute.ConstructorArguments.Count > 0)
					{
						var nullableCtor = attribute.ConstructorArguments[0];

						// https://codeblog.jonskeet.uk/2019/02/10/nullableattribute-and-c-8/
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

		internal byte GetNextState()
		{
			var state = this.flags.Length == 0 ? NullableContext.NotNullable : this.flags[this.index];
			this.index = this.flags.Length <= 1 ? 0 : this.index + 1;
			return state;
		}

		public int Count => this.flags.Length;
	}
}