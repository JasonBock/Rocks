using Microsoft.CodeAnalysis;

namespace Rocks.Analysis.Extensions;

/// <summary>
/// Provides extensions for <see cref="object"/> values.
/// </summary>
public static class ObjectExtensions
{
	/// <summary>
	/// This should only be used to get a stringified version of a default value
	/// that will be put into the call site of an emitted method.
	/// </summary>
	internal static string GetDefaultValue(this object? self, ITypeSymbol selfType, Compilation compilation)
	{
		if (selfType.TypeKind == TypeKind.Enum)
		{
			return $"({selfType.GetFullyQualifiedName(compilation)})({self})";
		}
		else
		{
			return self switch
			{
				string s => $"\"{s}\"",
				bool b => $"{(b ? "true" : "false")}",
				byte b => b.GetByteValue(),
				sbyte sb => sb.GetSignedByteValue(),
				char c => c.GetCharValue(),
				decimal d => d.GetDecimalValue(),
				double d => d.GetDoubleValue(),
				float f => f.GetFloatValue(),
				int i => i.GetIntValue(),
				uint ui => ui.GetUnsignedIntValue(),
				long l => l.GetLongValue(),
				ulong ul => ul.GetUnsignedLongValue(),
				short s => s.GetShortValue(),
				ushort us => us.GetUnsignedShortValue(),
				null => selfType.IsValueType ? "default" :
					selfType.TypeKind == TypeKind.TypeParameter ? "default!" : "null",
				_ => self.ToString() ?? string.Empty
			};
		}
	}

	private static string GetByteValue(this byte self) =>
		self switch
		{
			byte.MaxValue => "byte.MaxValue",
			byte.MinValue => "byte.MinValue",
			_ => self.ToString()
		};

	private static string GetSignedByteValue(this sbyte self) =>
		self switch
		{
			sbyte.MaxValue => "sbyte.MaxValue",
			sbyte.MinValue => "sbyte.MinValue",
			_ => self.ToString()
		};

	private static string GetCharValue(this char self) =>
		self switch
		{
			char.MaxValue => "char.MaxValue",
			char.MinValue => "char.MinValue",
			_ => $"'{self}'"
		};

	private static string GetDecimalValue(this decimal self) =>
		self switch
		{
			decimal.MaxValue => "decimal.MaxValue",
			decimal.MinusOne => "decimal.MinusOne",
			decimal.MinValue => "decimal.MinValue",
			decimal.One => "decimal.One",
			decimal.Zero => "decimal.Zero",
			_ => self.ToString()
		};

	private static string GetDoubleValue(this double self) =>
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

	private static string GetFloatValue(this float self) =>
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

	private static string GetIntValue(this int self) =>
		self switch
		{
			int.MaxValue => "int.MaxValue",
			int.MinValue => "int.MinValue",
			_ => self.ToString()
		};

	private static string GetUnsignedIntValue(this uint self) =>
		self switch
		{
			uint.MaxValue => "uint.MaxValue",
			uint.MinValue => "uint.MinValue",
			_ => self.ToString()
		};

	private static string GetLongValue(this long self) =>
		self switch
		{
			long.MaxValue => "long.MaxValue",
			long.MinValue => "long.MinValue",
			_ => self.ToString()
		};

	private static string GetUnsignedLongValue(this ulong self) =>
		self switch
		{
			ulong.MaxValue => "ulong.MaxValue",
			ulong.MinValue => "ulong.MinValue",
			_ => self.ToString()
		};

	private static string GetShortValue(this short self) =>
		self switch
		{
			short.MaxValue => "short.MaxValue",
			short.MinValue => "short.MinValue",
			_ => self.ToString()
		};

	private static string GetUnsignedShortValue(this ushort self) =>
		self switch
		{
			ushort.MaxValue => "ushort.MaxValue",
			ushort.MinValue => "ushort.MinValue",
			_ => self.ToString()
		};
}