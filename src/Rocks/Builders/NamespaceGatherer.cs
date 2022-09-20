using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Builders;

internal sealed class NamespaceGatherer
{
	private readonly ImmutableHashSet<string>.Builder builder =
		ImmutableHashSet.CreateBuilder<string>();

	public void Add(INamespaceSymbol? @namespace)
	{
		if(!@namespace?.IsGlobalNamespace ?? false)
		{
			this.Add(@namespace.GetName());
		}
	}

	public void Add(Type type) => 
		this.Add(type.Namespace);

	public void Add(string? @namespace)
	{
		if (!string.IsNullOrWhiteSpace(@namespace) &&
			@namespace != "<global namespace>")
		{
			this.builder.Add(@namespace!);
		}
	}

	public void AddRange(IEnumerable<INamespaceSymbol> namespaces)
	{
		foreach(var @namespace in namespaces)
		{
			this.Add(@namespace);
		}
	}

	public ImmutableHashSet<string> Values => this.builder.ToImmutable();
}