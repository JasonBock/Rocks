namespace Rocks.Options
{
	public sealed class RockOptions
	{
		public RockOptions(SerializationOption serialization = SerializationOption.NotSupported,
			AllowWarning allowWarning = AllowWarning.No) =>
			(this.Serialization, this.AllowWarning) = (serialization, allowWarning);

		public override int GetHashCode() => (this.Serialization, this.AllowWarning).GetHashCode();

		public AllowWarning AllowWarning { get; }
		public SerializationOption Serialization { get; }
	}
}