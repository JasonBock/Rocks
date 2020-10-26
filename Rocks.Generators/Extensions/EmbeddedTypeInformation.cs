using System.Collections.Immutable;

namespace Rocks.Extensions
{
	internal sealed class EmbeddedTypeInformation
	{
		internal EmbeddedTypeInformation(string typeResourceName, string typeName) =>
			(this.TypeResourceName, this.TypeName) = 
				(typeResourceName, typeName);

		internal string TypeName { get; }
		internal string TypeResourceName { get; }
	}
}