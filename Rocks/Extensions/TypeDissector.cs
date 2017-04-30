using System;
using System.Collections.Generic;

namespace Rocks.Extensions
{
	internal sealed class TypeDissector
	{
		internal TypeDissector(Type type)
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

			this.SafeName = this.RootType.GetSafeName();
		}

		internal Type Type { get; }
		internal Type RootType { get; }
		internal bool IsPointer { get; }
		internal bool IsByRef { get; }
		internal bool IsArray { get; }
		internal string SafeName { get; }
	}
}
