using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class IEnumerableOfAssemblyExtensions
	{
		private static readonly ConcurrentDictionary<Assembly, MetadataReference> transformedAssemblies =
			new ConcurrentDictionary<Assembly, MetadataReference>();

		internal static IEnumerable<MetadataReference> Transform(this IEnumerable<Assembly> @this) =>
			@this.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => IEnumerableOfAssemblyExtensions.transformedAssemblies.GetOrAdd(
					_, asm => MetadataReference.CreateFromFile(asm.Location)))
				.Cast<MetadataReference>();
	}
}
