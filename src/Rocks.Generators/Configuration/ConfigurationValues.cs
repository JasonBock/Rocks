using Microsoft.CodeAnalysis;
using System;

namespace Rocks.Configuration
{
	internal sealed class ConfigurationValues
	{
		private const string IndentSizeKey = "indent_size";
		private const uint IndentSizeDefaultValue = 3u;
		private const string IndentStyleKey = "indent_style";
		private const IndentStyle IndentStyleDefaultValue = IndentStyle.Tab;
		private const string TreatWarningsAsErrorsKey = "build_property.TreatWarningsAsErrors";

		public ConfigurationValues(GeneratorExecutionContext context, SyntaxTree tree)
		{
			var options = context.AnalyzerConfigOptions.GetOptions(tree);

			this.IndentStyle = options.TryGetValue(ConfigurationValues.IndentStyleKey, out var indentStyle) ?
				(Enum.TryParse<IndentStyle>(indentStyle, out var indentStyleValue) ? indentStyleValue : ConfigurationValues.IndentStyleDefaultValue) :
				ConfigurationValues.IndentStyleDefaultValue;
			this.IndentSize = options.TryGetValue(ConfigurationValues.IndentSizeKey, out var indentSize) ?
				(uint.TryParse(indentSize, out var indentSizeValue) ? indentSizeValue : ConfigurationValues.IndentSizeDefaultValue) : 
				ConfigurationValues.IndentSizeDefaultValue;
			this.TreatWarningsAsErrors = options.TryGetValue(ConfigurationValues.TreatWarningsAsErrorsKey, out var treatWarningsAsErrors) ?
				(bool.TryParse(treatWarningsAsErrors, out var treatWarningsAsErrorsValue) ? treatWarningsAsErrorsValue : false) :
				false;
		}

		internal ConfigurationValues(IndentStyle indentStyle, uint indentSize, bool treatWarningsAsErrors) =>
			(this.IndentStyle, this.IndentSize, this.TreatWarningsAsErrors) =
				(indentStyle, indentSize, treatWarningsAsErrors);

		internal IndentStyle IndentStyle { get; }
		internal uint IndentSize { get; }
		internal bool TreatWarningsAsErrors { get; }
	}
}