using System;

namespace Rocks
{
	[Serializable]
	public sealed class RaiseEventInformation
	{
		public RaiseEventInformation(string name, EventArgs args) =>
			(this.Name, this.Args) = (name, args);

		public EventArgs Args { get; }
		public string Name { get; }
	}
}