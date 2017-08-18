using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class IEnumerableOfAssemblyExtensions
	{
		internal static IEnumerable<MetadataReference> Transform(this IEnumerable<Assembly> @this) =>
			@this
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.Cast<MetadataReference>();
	}
}
