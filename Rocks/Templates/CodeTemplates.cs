namespace Rocks.Templates
{
	internal static class CodeTemplates
	{
		internal const string Internal = "internal";
		internal const string Public = "public";
 		internal const string Protected = "protected";
		internal const string ProtectedAndInternal = "internal protected";

		internal static string GetVisibility(bool isMemberFamily, bool isMemberFamilyOrAssembly) =>
			isMemberFamily || isMemberFamilyOrAssembly ? CodeTemplates.Protected : CodeTemplates.Internal;
		
		internal static string GetExpectation(string parameterName, string parameterTypeName) => 
			$"(methodHandler.Expectations[\"{parameterName}\"] as R.ArgumentExpectation<{parameterTypeName}>).IsValid({parameterName})";

		internal static string GetIsUnsafe(bool isUnsafe) => isUnsafe ? "unsafe" : string.Empty;
	}
}
