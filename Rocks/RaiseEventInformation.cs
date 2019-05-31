using System;

namespace Rocks
{
	[Serializable]
	internal sealed class RaiseEventInformation
	{
		internal RaiseEventInformation(string name, EventArgs args) =>
			(this.Name, this.Args) = (name, args);

		internal string Name { get; }
		internal EventArgs Args { get; }
	}
}