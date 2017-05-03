namespace Rocks
{
	public static class ErrorMessages
	{
		public static string GetCannotMockTypeWithSerializationRequestedAndNoPublicNoArgumentConstructor(string typeName) =>
			$"Cannot mock the sealed type {typeName} when serialization is requested and no public constructor with no arguments exists.";

		public static string GetCannotMockSealedType(string typeName) => $"Cannot mock the sealed type {typeName}.";

		public static string GetCannotMockTypeWithInternalAbstractMembers(string typeName) =>
			$"The type {typeName} has internal abstract member(s) without an appropriate InternalsVisibleToAttribute to allow the mock to override the member(s).";

		public static string GetCannotMockTypeWithNoAccessibleConstructors(string typeName) =>
			$"The type {typeName} has no accessible constructors.";

		public static string GetCannotUseIsDefaultWithNonOptionalArguments(string typeName, string methodDescription, string parameterName) =>
			$"Cannot use Arg.IsDefault with non-optional parameters. Type: {typeName}, method: {methodDescription}, parameter name: {parameterName}.";

		public static string GetVerificationFailed(string typeName, string methodDescription, string failure) =>
			$"Type: {typeName}, method: {methodDescription}, message: {failure}";
	}
}
