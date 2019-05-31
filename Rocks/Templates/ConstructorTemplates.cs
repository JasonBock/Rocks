namespace Rocks.Templates
{
	internal static class ConstructorTemplates
	{
		internal static string GetConstructor(string typeName, string argumentNames, string argumentNamesWithTypes) =>
$@"public {typeName}(SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> handlers{argumentNamesWithTypes})
	: base({argumentNames}) =>
	this.handlers = handlers;";

		internal static string GetConstructorWithNoArguments(string mockTypeName) =>
$@"public {mockTypeName}() =>
	this.handlers = new SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>>(
		new SCG.Dictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>>());";
	}
}