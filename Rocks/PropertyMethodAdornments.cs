namespace Rocks
{
	public sealed class PropertyMethodAdornments
	{
		public PropertyMethodAdornments(MethodAdornments getter, MethodAdornments setter)
		{
			this.Getter = getter;
			this.Setter = setter;
		}

		public MethodAdornments Getter { get; }
		public MethodAdornments Setter { get; }
	}
}
