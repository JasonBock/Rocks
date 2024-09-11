using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Rocks.Extensions;

internal static class StringExtensions
{
	// This gets a consistent hash value for a string.
	// GetHashCode() isn't unique, and that's needed 
	// for methods like GetProjectedCallbackDelegateName(), where
	// a unique number is needed for a given "signature" of a method
	// in string form, but the number needs to be same.
	//
	// The absolute value of the BigInteger result is returned
	// because this value will be used as part of a type name,
	// and "-" isn't allowed in these names.
	internal static BigInteger GetHash(this string? self) => 
		BigInteger.Abs(new(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(self ?? ""))));

	internal static string GenerateFileName(this string self) =>
		self.Replace("global::", string.Empty)
			.Replace(":", string.Empty)
			.Replace("<", string.Empty)
			.Replace(">", string.Empty)
			.Replace("?", "_null_")
			.Replace("*", "Pointer");
}