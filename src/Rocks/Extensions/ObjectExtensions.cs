namespace Rocks.Extensions;

internal static class ObjectExtensions
{
	/// <summary>
	/// This should only be used to get a stringified version of a default value
	/// that will be put into the call site of an emitted method.
	/// </summary>
	internal static string GetDefaultValue(this object? self, bool isValueType = false) =>
		self switch
		{
			// TODO: Try to get enum values as "EnumType.EnumValue" here...if possible.
			// Maybe an override of this method where the type is constrained to an enum?
			string s => $"\"{s}\"",
			bool b => $"{(b ? "true" : "false")}",
			byte b => b.GetByteDefaultValue(),
			sbyte sb => sb.GetSignedByteDefaultValue(),
			char c => c.GetCharDefaultValue(),
			decimal d => d.GetDecimalDefaultValue(),
			double d => d.GetDoubleDefaultValue(),
			float f => f.GetFloatDefaultValue(),
			int i => i.GetIntDefaultValue(),
			null => isValueType ? "default" : "null",
			_ => self.ToString() ?? string.Empty
		};

	private static string GetByteDefaultValue(this byte self) =>
		self switch
		{
			byte.MaxValue => "byte.MaxValue",
			byte.MinValue => "byte.MinValue",
			_ => self.ToString()
		};

	private static string GetSignedByteDefaultValue(this sbyte self) =>
		self switch
		{
			sbyte.MaxValue => "sbyte.MaxValue",
			sbyte.MinValue => "sbyte.MinValue",
			_ => self.ToString()
		};

	private static string GetCharDefaultValue(this char self) =>
		self switch
		{
			char.MaxValue => "char.MaxValue",
			char.MinValue => "char.MinValue",
			_ => self.ToString()
		};

	private static string GetDecimalDefaultValue(this decimal self) =>
		self switch
		{
			decimal.MaxValue => "decimal.MaxValue",
			decimal.MinusOne => "decimal.MinusOne",
			decimal.MinValue => "decimal.MinValue",
			decimal.One => "decimal.One",
			decimal.Zero => "decimal.Zero",
			_ => self.ToString()
		};

	private static string GetDoubleDefaultValue(this double self) =>
		self switch
		{
			double.Epsilon => "double.Epsilon",
			double.MaxValue => "double.MaxValue",
			double.MinValue => "double.MinValue",
			double.NaN => "double.NaN",
			double.NegativeInfinity => "double.NegativeInfinity",
			double.PositiveInfinity => "double.PositiveInfinity",
			_ => self.ToString()
		};

	private static string GetFloatDefaultValue(this float self) =>
		self switch
		{
			float.Epsilon => "float.Epsilon",
			float.MaxValue => "float.MaxValue",
			float.MinValue => "float.MinValue",
			float.NaN => "float.NaN",
			float.NegativeInfinity => "float.NegativeInfinity",
			float.PositiveInfinity => "float.PositiveInfinity",
			_ => self.ToString()
		};

	private static string GetIntDefaultValue(this int self) =>
		self switch
		{
			int.MaxValue => "int.MaxValue",
			int.MinValue => "int.MinValue",
			_ => self.ToString()
		};
}