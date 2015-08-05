namespace Rocks.Templates
{
	public static class ConstructorTemplates
	{
		public static string GetConstructor(string typeName, string argumentNames, string argumentNamesWithTypes) =>
$@"public {typeName}(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers{argumentNamesWithTypes})
	: base({argumentNames})
{{
	this.handlers = handlers;
}}";

		public static string GetConstructorWithNoArguments(string mockTypeName) =>
$@"public {mockTypeName}() 
{{ 
	this.handlers = new ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>(
		new System.Collections.Generic.Dictionary<int, ReadOnlyCollection<HandlerInformation>>());
}}";
	}
}
