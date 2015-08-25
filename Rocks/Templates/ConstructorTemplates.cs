namespace Rocks.Templates
{
	internal static class ConstructorTemplates
	{
		internal static string GetConstructor(string typeName, string argumentNames, string argumentNamesWithTypes) =>
$@"public {typeName}(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers{argumentNamesWithTypes})
	: base({argumentNames})
{{
	this.handlers = handlers;
}}";

		internal static string GetConstructorWithNoArguments(string mockTypeName) =>
$@"public {mockTypeName}() 
{{ 
	this.handlers = new ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>(
		new System.Collections.Generic.Dictionary<int, ReadOnlyCollection<HandlerInformation>>());
}}";
	}
}
