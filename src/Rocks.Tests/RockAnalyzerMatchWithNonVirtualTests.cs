using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;

namespace Rocks.Tests;

public static class RockAnalyzerMatchWithNonVirtualTests
{
	[Test]
	public static async Task AnalyzeWhenAnExactMatchWithANonVirtualMemberExistsAsync()
	{
		var code =
			"""
			using Rocks;
			
			[assembly: RockCreate<TypeConverter<TypeNode>>]
			
			public class TypeNode { }
			
			public interface IIRTypeContext { }
			
			public interface ITypeConverter<TType>
				where TType : TypeNode
			{
				TypeNode ConvertType<TTypeContext>(TTypeContext typeContext, TType type)
					where TTypeContext : IIRTypeContext;
			}
			
			public abstract class TypeConverter<TType> : ITypeConverter<TypeNode>
				where TType : TypeNode
			{
				protected abstract TypeNode ConvertType<TTypeContext>(
					TTypeContext typeContext,
					TType type)
					where TTypeContext : IIRTypeContext;
			
				public TypeNode ConvertType<TTypeContext>(
					TTypeContext typeContext,
					TypeNode type)
					where TTypeContext : IIRTypeContext => new();
			}
			""";

		var diagnostic = new DiagnosticResult(TypeHasMatchWithNonVirtualDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 47);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}
}