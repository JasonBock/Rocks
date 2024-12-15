using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Rocks.ReferenceTest")]

namespace Rocks.SourceTest;

internal sealed record DataStuff;

internal interface IDoStuff
{
	void Perform(DataStuff dataStuff);
}
