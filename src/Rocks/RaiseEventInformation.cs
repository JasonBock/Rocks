using System;

namespace Rocks
{
	[Serializable]
	internal sealed class RaiseEventInformation
	{
		internal RaiseEventInformation(string name, EventArgs args) =>
			(this.Name, this.Args) = (name, args);

		internal EventArgs Args { get; }
		internal string Name { get; }
	}
}