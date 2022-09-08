using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rocks.Configuration;

internal sealed class ConfigurationValues
{
	private const string IndentSizeKey = "indent_size";
	private const uint IndentSizeDefaultValue = 3u;
	private const string IndentStyleKey = "indent_style";
	private const IndentStyle IndentStyleDefaultValue = IndentStyle.Tab;
	private const string TreatWarningsAsErrorsKey = "build_property.TreatWarningsAsErrors";

	public ConfigurationValues(AnalyzerConfigOptionsProvider optionsProvider, SyntaxTree tree)
	{
		var options = optionsProvider.GetOptions(tree);

		this.IndentStyle = options.TryGetValue(ConfigurationValues.IndentStyleKey, out var indentStyle) ?
			(Enum.TryParse<IndentStyle>(indentStyle, out var indentStyleValue) ? indentStyleValue : ConfigurationValues.IndentStyleDefaultValue) :
			ConfigurationValues.IndentStyleDefaultValue;
		this.IndentSize = options.TryGetValue(ConfigurationValues.IndentSizeKey, out var indentSize) ?
			(uint.TryParse(indentSize, out var indentSizeValue) ? indentSizeValue : ConfigurationValues.IndentSizeDefaultValue) :
			ConfigurationValues.IndentSizeDefaultValue;
		this.TreatWarningsAsErrors = options.TryGetValue(ConfigurationValues.TreatWarningsAsErrorsKey, out var treatWarningsAsErrors) && 
			bool.TryParse(treatWarningsAsErrors, out var treatWarningsAsErrorsValue) && treatWarningsAsErrorsValue;
	}

	internal ConfigurationValues(IndentStyle indentStyle, uint indentSize, bool treatWarningsAsErrors) =>
		(this.IndentStyle, this.IndentSize, this.TreatWarningsAsErrors) =
			(indentStyle, indentSize, treatWarningsAsErrors);

	internal IndentStyle IndentStyle { get; }
	internal uint IndentSize { get; }
	internal bool TreatWarningsAsErrors { get; }
}