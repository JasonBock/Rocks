﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public enum ObjectExtensionValues
{
	None, Some, All
}

public static class ObjectExtensionsTests
{
	[TestCase("public class Test { public void Foo(string value = \"b\") { } }", "\"b\"")]
	[TestCase("public class Test { public void Foo(bool value = true) { } }", "true")]
	[TestCase("public class Test { public void Foo(bool value = false) { } }", "false")]
	[TestCase("public enum DefaultValue { One = 1, Two, Three } public class Test { public void Foo(DefaultValue value = DefaultValue.Two) { } }", "(global::DefaultValue)(2)")]
	[TestCase("public enum DefaultValue { MinusOne = -1, Two, Three } public class Test { public void Foo(DefaultValue value = DefaultValue.MinusOne) { } }", "(global::DefaultValue)(-1)")]
	[TestCase("public class Test { public void Foo(byte value = byte.MaxValue) { } }", "byte.MaxValue")]
	[TestCase("public class Test { public void Foo(byte value = byte.MinValue) { } }", "byte.MinValue")]
	[TestCase("public class Test { public void Foo(byte value = 22) { } }", "22")]
	[TestCase("public class Test { public void Foo(sbyte value = sbyte.MaxValue) { } }", "sbyte.MaxValue")]
	[TestCase("public class Test { public void Foo(sbyte value = sbyte.MinValue) { } }", "sbyte.MinValue")]
	[TestCase("public class Test { public void Foo(sbyte value = 22) { } }", "22")]
	[TestCase("public class Test { public void Foo(char value = char.MaxValue) { } }", "char.MaxValue")]
	[TestCase("public class Test { public void Foo(char value = char.MinValue) { } }", "char.MinValue")]
	[TestCase("public class Test { public void Foo(char value = 'A') { } }", "'A'")]
	[TestCase("public class Test { public void Foo(double value = double.PositiveInfinity) { } }", "double.PositiveInfinity")]
	[TestCase("public class Test { public void Foo(double value = double.NegativeInfinity) { } }", "double.NegativeInfinity")]
	[TestCase("public class Test { public void Foo(double value = double.MaxValue) { } }", "double.MaxValue")]
	[TestCase("public class Test { public void Foo(double value = double.MinValue) { } }", "double.MinValue")]
	[TestCase("public class Test { public void Foo(double value = double.NaN) { } }", "double.NaN")]
	[TestCase("public class Test { public void Foo(double value = double.Epsilon) { } }", "double.Epsilon")]
	[TestCase("public class Test { public void Foo(double value = 22.473) { } }", "22.473")]
	[TestCase("public class Test { public void Foo(float value = float.PositiveInfinity) { } }", "float.PositiveInfinity")]
	[TestCase("public class Test { public void Foo(float value = float.NegativeInfinity) { } }", "float.NegativeInfinity")]
	[TestCase("public class Test { public void Foo(float value = float.MaxValue) { } }", "float.MaxValue")]
	[TestCase("public class Test { public void Foo(float value = float.MinValue) { } }", "float.MinValue")]
	[TestCase("public class Test { public void Foo(float value = float.NaN) { } }", "float.NaN")]
	[TestCase("public class Test { public void Foo(float value = float.Epsilon) { } }", "float.Epsilon")]
	[TestCase("public class Test { public void Foo(float value = (float)22.473) { } }", "22.473")]
	[TestCase("public class Test { public void Foo(int value = int.MaxValue) { } }", "int.MaxValue")]
	[TestCase("public class Test { public void Foo(int value = int.MinValue) { } }", "int.MinValue")]
	[TestCase("public class Test { public void Foo(int value = 22) { } }", "22")]
	[TestCase("public class Test { public void Foo(uint value = uint.MaxValue) { } }", "uint.MaxValue")]
	[TestCase("public class Test { public void Foo(uint value = uint.MinValue) { } }", "uint.MinValue")]
	[TestCase("public class Test { public void Foo(uint value = 22) { } }", "22")]
	[TestCase("public class Test { public void Foo(long value = long.MaxValue) { } }", "long.MaxValue")]
	[TestCase("public class Test { public void Foo(long value = long.MinValue) { } }", "long.MinValue")]
	[TestCase("public class Test { public void Foo(long value = 22) { } }", "22")]
	[TestCase("public class Test { public void Foo(ulong value = ulong.MaxValue) { } }", "ulong.MaxValue")]
	[TestCase("public class Test { public void Foo(ulong value = ulong.MinValue) { } }", "ulong.MinValue")]
	[TestCase("public class Test { public void Foo(ulong value = 22) { } }", "22")]
	[TestCase("public class Test { public void Foo(short value = short.MaxValue) { } }", "short.MaxValue")]
	[TestCase("public class Test { public void Foo(short value = short.MinValue) { } }", "short.MinValue")]
	[TestCase("public class Test { public void Foo(short value = 22) { } }", "22")]
	[TestCase("public class Test { public void Foo(ushort value = ushort.MaxValue) { } }", "ushort.MaxValue")]
	[TestCase("public class Test { public void Foo(ushort value = ushort.MinValue) { } }", "ushort.MinValue")]
	[TestCase("public class Test { public void Foo(ushort value = 22) { } }", "22")]
	[TestCase("public class Test { public void Foo(string? value = null) { } }", "null")]
	[TestCase("public class Test { public void Foo(int? value = null) { } }", "default")]
	[TestCase("public class Test { public void Foo<T>(T value = default) { } }", "default!")]
	[TestCase("public class Test { public void Foo(decimal value = decimal.MaxValue) { } }", "decimal.MaxValue")]
	[TestCase("public class Test { public void Foo(decimal value = decimal.MinusOne) { } }", "decimal.MinusOne")]
	[TestCase("public class Test { public void Foo(decimal value = decimal.MinValue) { } }", "decimal.MinValue")]
	[TestCase("public class Test { public void Foo(decimal value = decimal.One) { } }", "decimal.One")]
	[TestCase("public class Test { public void Foo(decimal value = decimal.Zero) { } }", "decimal.Zero")]
	[TestCase("public class Test { public void Foo(decimal value = 22) { } }", "22")]
	public static void GetDefaultValue(string code, string expectedResult)
	{
		var (parameter, compilation) = ObjectExtensionsTests.GetParameterSymbol(code);
		Assert.That(parameter.ExplicitDefaultValue.GetDefaultValue(parameter.Type, compilation), Is.EqualTo(expectedResult));
	}

	[Test]
	public static void FormatValue() =>
		Assert.That(33.FormatValue(), Is.EqualTo("33"));

	[Test]
	public static void FormatEnumValue() =>
		Assert.That(ObjectExtensionValues.Some.FormatValue(), Is.EqualTo("Rocks.Analysis.Tests.Extensions.ObjectExtensionValues.Some"));

	[Test]
	public static void FormatEnumValueWhenValueIsIncorrect() =>
		Assert.That(((ObjectExtensionValues)22).FormatValue(), Is.EqualTo("(Rocks.Analysis.Tests.Extensions.ObjectExtensionValues)22"));

	[Test]
	public static void FormatNullValue() =>
		Assert.That((null as string).FormatValue(), Is.EqualTo("null"));

	[Test]
	public static void FormatCollectionValue() =>
		Assert.That((new int[] { 1, 2, 3 }).FormatValue(), Is.EqualTo("System.Int32[], Count = 3"));

	private static (IParameterSymbol, Compilation) GetParameterSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0], compilation);
	}
}