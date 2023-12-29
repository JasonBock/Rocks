﻿using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ClassConstructorMakeGeneratorV4Tests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockMake<MockTests.BaseCtor>]

			namespace MockTests
			{
				public class BaseCtor
				{
					 public BaseCtor(int a, ref string b, out string c, params string[] d) { c = "c"; }

					 public virtual void Foo() { }
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class BaseCtorMakeExpectations
				{
					internal global::MockTests.BaseCtor Instance(int @a, ref string @b, out string @c, params string[] @d)
					{
						return new RockBaseCtor(@a, ref @b, out @c, @d);
					}
					
					private sealed class RockBaseCtor
						: global::MockTests.BaseCtor
					{
						public RockBaseCtor(int @a, ref string @b, out string @c, params string[] @d)
							: base(@a, ref @b, out @c, @d)
						{
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.BaseCtor_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}