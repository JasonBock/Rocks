using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockExpectationsVerifyBuilderV4
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		writer.WriteLines(
			"""
			public override void Verify()
			{
				if (this.WasInstanceInvoked)
				{
					var failures = new global::System.Collections.Generic.List<string>();

			""");

		writer.Indent += 2;

		foreach (var method in mockType.Methods)
		{
			writer.WriteLine($"failures.AddRange(this.Verify(handlers{method.MemberIdentifier}));");
		}

		foreach (var property in mockType.Properties)
		{
			writer.WriteLine($"failures.AddRange(this.Verify(handlers{property.MemberIdentifier}));");

			if (property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit)
			{
				writer.WriteLine($"failures.AddRange(this.Verify(handlers{property.MemberIdentifier + 1}));");
			}
		}

		writer.Indent -= 2;

		writer.WriteLines(
			"""

					if (failures.Count > 0)
					{
						throw new global::Rocks.Exceptions.VerificationException(failures);
					}
				}
			}
			""");
	}
}