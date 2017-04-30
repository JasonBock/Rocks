using System;

namespace Rocks
{
#if !NETCOREAPP1_1
	[Serializable]
#endif
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
