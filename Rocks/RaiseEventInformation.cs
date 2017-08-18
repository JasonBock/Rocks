using System;

namespace Rocks
{
	[Serializable]
	internal sealed class RaiseEventInformation
	{
		internal RaiseEventInformation(string name, EventArgs args)
		{
			this.Name = name;
			this.Args = args;
		}

		internal string Name { get; }
		internal EventArgs Args { get; }
	}
}
