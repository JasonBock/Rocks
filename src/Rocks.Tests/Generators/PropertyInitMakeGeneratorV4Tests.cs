﻿using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class PropertyInitMakeGeneratorV4Tests
{
	[Test]
	public static async Task GenerateWithInitAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockMake<MockTests.ITest>]

			#nullable enable

			namespace MockTests
			{
				public interface ITest
				{
					int NonNullableValueType { get; init; }
					int? NullableValueType { get; init; }
					string NonNullableReferenceType { get; init; }
					string? NullableReferenceType { get; init; }
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class ITestMakeExpectations
				{
					internal sealed class ConstructorProperties
					{
						internal int NonNullableValueType { get; init; }
						internal int? NullableValueType { get; init; }
						internal string? NonNullableReferenceType { get; init; }
						internal string? NullableReferenceType { get; init; }
					}
					
					internal global::MockTests.ITest Instance(global::MockTests.ITestMakeExpectations.ConstructorProperties? @constructorProperties)
					{
						return new RockITest(@constructorProperties);
					}
					
					private sealed class RockITest
						: global::MockTests.ITest
					{
						public RockITest(ConstructorProperties? @constructorProperties)
						{
							if (@constructorProperties is not null)
							{
								this.NonNullableValueType = @constructorProperties.NonNullableValueType;
								this.NullableValueType = @constructorProperties.NullableValueType;
								this.NonNullableReferenceType = @constructorProperties.NonNullableReferenceType!;
								this.NullableReferenceType = @constructorProperties.NullableReferenceType;
							}
						}
						
						public int NonNullableValueType
						{
							get => default!;
							init { }
						}
						public int? NullableValueType
						{
							get => default!;
							init { }
						}
						public string NonNullableReferenceType
						{
							get => default!;
							init { }
						}
						public string? NullableReferenceType
						{
							get => default!;
							init { }
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.ITest_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithRequiredAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockMake<MockTests.Test>]

			#nullable enable
			
			namespace MockTests
			{
				public class Test
				{
					public virtual void Foo() { }
					public required int NonNullableValueType { get; init; }
					public required int? NullableValueType { get; init; }
					public required string NonNullableReferenceType { get; init; }
					public required string? NullableReferenceType { get; init; }
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class TestMakeExpectations
				{
					internal sealed class ConstructorProperties
					{
						internal required int NonNullableValueType { get; init; }
						internal required int? NullableValueType { get; init; }
						internal required string? NonNullableReferenceType { get; init; }
						internal required string? NullableReferenceType { get; init; }
					}
					
					internal global::MockTests.Test Instance(global::MockTests.TestMakeExpectations.ConstructorProperties @constructorProperties)
					{
						global::System.ArgumentNullException.ThrowIfNull(constructorProperties);
						return new RockTest(@constructorProperties);
					}
					
					private sealed class RockTest
						: global::MockTests.Test
					{
						[global::System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
						public RockTest(ConstructorProperties @constructorProperties)
						{
							this.NonNullableValueType = @constructorProperties.NonNullableValueType;
							this.NullableValueType = @constructorProperties.NullableValueType;
							this.NonNullableReferenceType = @constructorProperties.NonNullableReferenceType!;
							this.NullableReferenceType = @constructorProperties.NullableReferenceType;
						}
						
						public override bool Equals(object? @obj)
						{
							return default!;
						}
						public override int GetHashCode()
						{
							return default!;
						}
						public override string? ToString()
						{
							return default!;
						}
						public override void Foo()
						{
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.Test_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}