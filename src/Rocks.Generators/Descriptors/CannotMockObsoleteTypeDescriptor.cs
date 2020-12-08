using Microsoft.CodeAnalysis;
using System;

namespace Rocks.Descriptors
{
	public static class UnexpectedExceptionDescriptor
	{
		internal static Diagnostic Create(Exception e) =>
			Diagnostic.Create(new(UnexpectedExceptionDescriptor.Id, UnexpectedExceptionDescriptor.Title,
				e.ToString().Replace(Environment.NewLine, " : "),
				DescriptorConstants.Usage, DiagnosticSeverity.Error, true,
				helpLinkUri: HelpUrlBuilder.Build(
					UnexpectedExceptionDescriptor.Id, UnexpectedExceptionDescriptor.Title)), null);

		public const string Id = "ROCK6";
		public const string Message = "An unexpected exception has occurred";
		public const string Title = "Unexpected Exception";
	}
}