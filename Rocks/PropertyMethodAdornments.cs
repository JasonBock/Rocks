using System;

namespace Rocks
{
	public sealed class PropertyMethodAdornments
	{
		private readonly MethodAdornments getter;
		private readonly MethodAdornments setter;

		public PropertyMethodAdornments(MethodAdornments getter, MethodAdornments setter)
		{
			this.getter = getter;
			this.setter = setter;
		}

		public PropertyMethodAdornments RaisesOnGetter(string eventName, EventArgs args)
		{
			this.getter.Raises(eventName, args);
			return this;
		}

		public PropertyMethodAdornments RaisesOnSetter(string eventName, EventArgs args)
		{
			this.setter.Raises(eventName, args);
			return this;
		}
	}
}
