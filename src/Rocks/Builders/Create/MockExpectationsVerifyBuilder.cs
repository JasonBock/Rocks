using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockExpectationsVerifyBuilder
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
			writer.WriteLine($"if (this.handlers{method.MemberIdentifier}?.Count > 0) {{ failures.AddRange(this.Verify(this.handlers{method.MemberIdentifier}, {method.MemberIdentifier})); }}");
		}

		foreach (var property in mockType.Properties)
		{
			if (property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.Init)
			{
				writer.WriteLine($"if (this.handlers{property.MemberIdentifier}?.Count > 0) {{ failures.AddRange(this.Verify(this.handlers{property.MemberIdentifier}, {property.MemberIdentifier})); }}");
			}
			else
			{
				var memberIdentifier = property.MemberIdentifier;

				if (property.GetCanBeSeenByContainingAssembly)
				{
					writer.WriteLine($"if (this.handlers{memberIdentifier}?.Count > 0) {{ failures.AddRange(this.Verify(this.handlers{memberIdentifier}, {memberIdentifier})); }}");
					memberIdentifier++;
				}

				if (property.SetCanBeSeenByContainingAssembly || property.InitCanBeSeenByContainingAssembly)
				{
					writer.WriteLine($"if (this.handlers{memberIdentifier}?.Count > 0) {{ failures.AddRange(this.Verify(this.handlers{memberIdentifier}, {memberIdentifier})); }}");
				}
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