using Microsoft.CodeAnalysis;
using NUnit.Framework;
using System.Collections.Immutable;

[assembly: Parallelizable(ParallelScope.Children)]

internal static class Shared
{
	internal static Lazy<ImmutableArray<PortableExecutableReference>> References { get; } =
		new Lazy<ImmutableArray<PortableExecutableReference>>(() =>
			AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.ToImmutableArray());
}