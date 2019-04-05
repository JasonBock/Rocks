using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks
{
	internal sealed class TypeDissector
	{
		private static readonly ConcurrentDictionary<Type, TypeDissector> mapping =
			new ConcurrentDictionary<Type, TypeDissector>();

		private static readonly ImmutableDictionary<string, string> simplifiedPrimitiveNames =
			new Dictionary<string, string>
			{
				{ typeof(bool).Name, "bool" },
				{ typeof(byte).Name, "byte" },
				{ typeof(sbyte).Name, "sbyte" },
				{ typeof(short).Name, "short" },
				{ typeof(ushort).Name, "ushort" },
				{ typeof(int).Name, "int" },
				{ typeof(uint).Name, "uint" },
				{ typeof(long).Name, "long" },
				{ typeof(ulong).Name, "ulong" },
				{ typeof(char).Name, "char" },
				{ typeof(double).Name, "double" },
				{ typeof(float).Name, "float" },
				{ typeof(decimal).Name, "decimal" },
				{ typeof(string).Name, "string" },
				{ typeof(object).Name, "object" }
			}.ToImmutableDictionary();

		internal static TypeDissector Create(Type type) => 
			TypeDissector.mapping.GetOrAdd(type, t => new TypeDissector(type));

		private TypeDissector(Type type)
		{
			this.Type = type;
			this.IsArray = type.IsArray;
			this.IsPointer = type.IsPointer;
			this.IsByRef = type.IsByRef;
			this.RootType = type;

			while (this.RootType.HasElementType)
			{
				this.RootType = this.RootType.GetElementType();
				this.IsArray |= this.RootType.IsArray;
				this.IsPointer |= this.RootType.IsPointer;
				this.IsByRef |= this.RootType.IsByRef;
			}

			this.SafeName = TypeDissector.GetSafeName(this.RootType);
		}

		private static string GetSafeName(Type type)
		{
			var typeName = type.Name;
			var typeFullName = type.FullName;

			var isConflictingTypeName = typeof(TypeDissector)
				.Assembly.GetTypes().Any(_ => _.Name == typeName);

			string name;

			if (isConflictingTypeName)
			{
				name = typeFullName.Split('`')[0];
			}
			else
			{
				name = !string.IsNullOrWhiteSpace(typeFullName) ?
					typeFullName.Split('`')[0].Split('.').Last().Replace("+", ".") :
					typeName.Split('`')[0];

				if (TypeDissector.simplifiedPrimitiveNames.ContainsKey(name))
				{
					name = TypeDissector.simplifiedPrimitiveNames[name];
				}
			}

			return name;
		}

		internal Type Type { get; }
		internal Type RootType { get; }
		internal bool IsPointer { get; }
		internal bool IsByRef { get; }
		internal bool IsArray { get; }
		internal string SafeName { get; }
	}
}