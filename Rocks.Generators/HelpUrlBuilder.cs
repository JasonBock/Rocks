namespace Rocks
{
	internal static class HelpUrlBuilder
	{
		internal static string Build(string identifier, string title) =>
		  $"https://github.com/JasonBock/Rocks/tree/master/Rocks.Documentation/{identifier}-{title}.md";
	}
}