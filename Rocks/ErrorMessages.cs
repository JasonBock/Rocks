namespace Rocks
{
	public static class ErrorMessages
	{
		public static string GetCannotMockSealedType(string typeName) => $"Cannot mock the sealed type {typeName}.";
		public static string GetVerificationFailed(string typeName, string methodDescription, string failure) => $"Type: {typeName}, method: {methodDescription}, message: {failure}";
	}
}
