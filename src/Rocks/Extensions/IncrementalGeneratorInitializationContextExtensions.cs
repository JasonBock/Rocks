using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace Rocks.Extensions;

internal static class IncrementalGeneratorInitializationContextExtensions
{
   internal static ImmutableArray<(string fileName, string code)> GetOutputCode() => 
		[
		   (
				"Rocks.RefStructArgument.g.cs",
				/* lang=c#-test */
				"""
				#nullable enable

				namespace Rocks;

				[global::System.Serializable]
				public sealed class RefStructArgument<T>
					: global::Rocks.Argument
					where T : allows ref struct
				{
					private readonly global::System.Predicate<T>? evaluation;
					private readonly global::Rocks.ValidationState validation;

					internal RefStructArgument() => 
						this.validation = global::Rocks.ValidationState.None;

					internal RefStructArgument(global::System.Predicate<T> @evaluation) => 
						(this.evaluation, this.validation) =
							(@evaluation, global::Rocks.ValidationState.Evaluation);

					public bool IsValid(T @value) =>
							this.validation switch
							{
								global::Rocks.ValidationState.None => true,
								global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
								_ => throw new global::System.NotSupportedException("Invalid validation state."),
							};
				}
				"""
			)
	   ];

   internal static void RegisterTypes(this IncrementalGeneratorInitializationContext self) => 
		self.RegisterPostInitializationOutput(static postInitializationContext =>
		{
			foreach(var (fileName, code) in IncrementalGeneratorInitializationContextExtensions.GetOutputCode())
			{
				postInitializationContext.AddSource(fileName, SourceText.From(code, Encoding.UTF8));
			}
		});
}
