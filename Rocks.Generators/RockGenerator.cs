using Microsoft.CodeAnalysis;

namespace Rocks
{
	[Generator]
	public sealed class RockGenerator
		: ISourceGenerator
	{
		public void Execute(SourceGeneratorContext context) => throw new System.NotImplementedException();
		public void Initialize(InitializationContext context) => throw new System.NotImplementedException();
	}
}
