using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsGetMethodDescriptionTests
	{
		[Test]
		public void GetMethodDescription()
		{
			var target = this.GetType().GetTypeInfo()
				.GetMethod(nameof(this.TargetWithArguments));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, Is.EqualTo("void TargetWithArguments(Int32 a, String c)"), nameof(description));
			Assert.That(namespaces.Count, Is.EqualTo(1), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(object).Namespace), Is.True, nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionForInterfaceMethod()
		{
			var target = typeof(IMethodInfoExtensionsTests).GetTypeInfo()
				.GetMethod(nameof(IMethodInfoExtensionsTests.Target));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, Is.EqualTo("void Target()"), nameof(description));
			Assert.That(namespaces.Count, Is.EqualTo(1), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(object).Namespace), Is.True, nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithOutArgument()
		{
			var target = this.GetType().GetTypeInfo()
				.GetMethod(nameof(this.TargetWithOutArgument));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, Is.EqualTo("void TargetWithOutArgument(out Int32 a)"), nameof(description));
			Assert.That(namespaces.Count, Is.EqualTo(1), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(object).Namespace), Is.True, nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithParamsArgument()
		{
			var target = this.GetType().GetTypeInfo()
				.GetMethod(nameof(this.TargetWithParamsArgument));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, Is.EqualTo("void TargetWithParamsArgument(params Int32[] a)"), nameof(description));
			Assert.That(namespaces.Count, Is.EqualTo(1), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(object).Namespace), Is.True, nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithRefArgument()
		{
			var target = this.GetType().GetTypeInfo()
				.GetMethod(nameof(this.TargetWithRefArgument));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, Is.EqualTo("void TargetWithRefArgument(ref Int32 a)"), nameof(description));
			Assert.That(namespaces.Count, Is.EqualTo(1), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(object).Namespace), Is.True, nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithReturnValue()
		{
			var target = this.GetType().GetTypeInfo()
				.GetMethod(nameof(this.TargetWithArgumentsAndReturnValue));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, Is.EqualTo("Int32 TargetWithArgumentsAndReturnValue(Int32 a, String c)"), nameof(description));
			Assert.That(namespaces.Count, Is.EqualTo(1), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(object).Namespace), Is.True, nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithGenericArguments()
		{
			var target = this.GetType().GetTypeInfo()
				.GetMethod(nameof(this.TargetWithGenericsAndReturnValue));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, Is.EqualTo("U TargetWithGenericsAndReturnValue<U, V>(Int32 a, U b, String c, V d)"), nameof(description));
			Assert.That(namespaces.Count, Is.EqualTo(2), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(object).Namespace), Is.True, nameof(namespaces.Contains));
			Assert.That(namespaces.Contains(this.GetType().Namespace), Is.True, nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithDefinedGenericArguments()
		{
			var target = this.GetType().GetTypeInfo()
				.GetMethod(nameof(this.TargetWithGenericsAndReturnValue)).MakeGenericMethod(typeof(Guid), typeof(double));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, Is.EqualTo("U TargetWithGenericsAndReturnValue<U, V>(Int32 a, U b, String c, V d)"), nameof(description));
			Assert.That(namespaces.Count, Is.EqualTo(2), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(object).Namespace), Is.True, nameof(namespaces.Contains));
			Assert.That(namespaces.Contains(this.GetType().Namespace), Is.True, nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithArrayArgumentss()
		{
			var target = this.GetType().GetTypeInfo()
				.GetMethod(nameof(this.TargetWithArrayArguments));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, 
				Is.EqualTo("void TargetWithArrayArguments(Int32[] a, String[] b, ref Guid[] c, out Double[] d)"), nameof(description));
			Assert.That(namespaces.Count, Is.EqualTo(1), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(object).Namespace), Is.True, nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithConstraints()
		{
			var target = this.GetType().GetTypeInfo()
				.GetMethod(nameof(this.TargetWithMultipleConstraints));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, 
				Is.EqualTo("void TargetWithMultipleConstraints<U, V, W, X>(U a, V b, W c, X d) where U : class, new() where V : MethodInfoExtensionsGetMethodDescriptionTests.Source, MethodInfoExtensionsGetMethodDescriptionTests.ISource where W : struct where X : V"), 
				nameof(description));
			Assert.That(namespaces.Count, Is.EqualTo(2), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(object).Namespace), Is.True, nameof(namespaces.Contains));
			Assert.That(namespaces.Contains(this.GetType().Namespace), Is.True, nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithComplexGenericReturnTypeAndArgumentTypes()
		{
			var target = typeof(HaveMethodWithComplexGenericReturnType<>).GetTypeInfo()
				.GetMethod(nameof(HaveMethodWithComplexGenericReturnType<int>.Target));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, 
				Is.EqualTo("IEnumerable<KeyValuePair<Int64, TSource>> Target(IEnumerable<KeyValuePair<Int64, TSource>> a)"), 
				nameof(description));
			Assert.That(namespaces.Count, Is.EqualTo(3), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(object).Namespace), Is.True, nameof(namespaces.Contains));
			Assert.That(namespaces.Contains(this.GetType().Namespace), Is.True, nameof(namespaces.Contains));
			Assert.That(namespaces.Contains(typeof(IEnumerable<>).Namespace), Is.True, nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionForMemberWithNoAttributes()
		{
			var target = typeof(HaveNoAttributes).GetTypeInfo()
				.GetMethod(nameof(HaveNoAttributes.Target));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, Is.EqualTo("Guid Target<T>(T a, Guid b)"), nameof(description));
		}

		[Test]
		public void GetMethodDescriptionForMemberWithAttribute()
		{
			var target = typeof(HaveAttribute).GetTypeInfo()
				.GetMethod(nameof(HaveAttribute.Target));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, 
				Is.EqualTo("Guid Target<[GetAttributes(True)]T>([GetAttributes(True)]T a, [GetAttributes(True)]Guid b)"), 
				nameof(description));
		}

		[Test]
		public void GetMethodDescriptionForMemberWithAttributeUsingEnumInConstructor()
		{
			var target = typeof(HaveAttributeWithEnumInConstructor).GetTypeInfo()
				.GetMethod(nameof(HaveAttributeWithEnumInConstructor.Target));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, 
				Is.EqualTo("Guid Target<[GetAttributes((SomeValues)2)]T>([GetAttributes((SomeValues)2)]T a, [GetAttributes((SomeValues)2)]Guid b)"), 
				nameof(description));
		}

		[Test]
		public void GetMethodDescriptionForMemberWithAttributeUsingNamedArguments()
		{
			var target = typeof(HaveAttributeUsingNamedArguments).GetTypeInfo()
				.GetMethod(nameof(HaveAttributeUsingNamedArguments.Target));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, 
				Is.EqualTo("Guid Target<[GetAttributes(True, TargetString = \"TargetString\")]T>([GetAttributes(True, TargetString = \"TargetString\")]T a, [GetAttributes(True, TargetString = \"TargetString\")]Guid b)"), 
				nameof(description));
		}

		[Test]
		public void GetMethodDescriptionForMemberWithAttributeUsingMultipleConstructorAndNamedArguments()
		{
			var target = typeof(HaveAttributeUsingMultipleConstructorAndNamedArguments).GetTypeInfo()
				.GetMethod(nameof(HaveAttributeUsingMultipleConstructorAndNamedArguments.Target));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, 
				Is.EqualTo("Guid Target<[GetAttributes(True, 2, TargetString = \"TargetString\", TargetInt = 3)]T>([GetAttributes(True, 2, TargetString = \"TargetString\", TargetInt = 3)]T a, [GetAttributes(True, 2, TargetString = \"TargetString\", TargetInt = 3)]Guid b)"),
				nameof(description));
		}

		[Test]
		public void GetMethodDescriptionForMemberWithMultipleAttributes()
		{
			var target = typeof(HaveMultipleAttributes).GetTypeInfo()
				.GetMethod(nameof(HaveMultipleAttributes.Target));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.That(description, 
				Is.EqualTo("Guid Target<[Mutliple, GetAttributes(True)]T>([Mutliple, GetAttributes(True)]T a, [Mutliple, GetAttributes(True)]Guid b)"), 
				nameof(description));
		}

		public void TargetWithNoArguments() { }
		public int TargetWithNoArgumentsAndReturnValue() => 0; 
		public void TargetWithArguments(int a, string c) { }
		public int TargetWithArgumentsAndReturnValue(int a, string c) => 0; 
		public void TargetWithGenerics<U, V>(int a, U b, string c, V d) { }
		public U TargetWithGenericsAndReturnValue<U, V>(int a, U b, string c, V d) => default(U);
		public void TargetWithOutArgument(out int a) => a = 0;
		public void TargetWithRefArgument(ref int a) { }
		public void TargetWithParamsArgument(params int[] a) { }
		public void TargetWithArrayArguments(int[] a, string[] b, ref Guid[] c, out double[] d) => d = null; 
		public void TargetWithMultipleConstraints<U, V, W, X>(U a, V b, W c, X d) where U : class, new() where V : Source, ISource where W : struct where X : V { }

		public interface ISource { }
		public class Source { }
	}

	public interface IMethodInfoExtensionsTests
	{
		void Target();
	}

	public class HaveMethodWithComplexGenericReturnType<TSource>
	{
		public virtual IEnumerable<KeyValuePair<long, TSource>> Target(IEnumerable<KeyValuePair<long, TSource>> a) => null; 
	}


	public enum SomeValues
	{
		HereIsOne,
		AndAnother,
		OneMore
	}

	public sealed class GetAttributesAttribute : Attribute
	{
		public GetAttributesAttribute(SomeValues TargetEnum) { }
		public GetAttributesAttribute(bool targetBool) { }
		public GetAttributesAttribute(bool targetBool, int targetInt) { }
		public GetAttributesAttribute(string targetString) { }

		public SomeValues TargetEnum { get; set; }
		public bool TargetBool { get; }
		public string TargetString { get; set; }
		public int TargetInt { get; set; }
	}

	public sealed class MutlipleAttribute : Attribute { }

	public class HaveNoAttributes
	{
		public Guid Target<T>(T a, Guid b) => Guid.Empty; 
	}

	public class HaveMultipleAttributes
	{
		public Guid Target<[Mutliple, GetAttributes(true)]T>([Mutliple, GetAttributes(true)]T a, [Mutliple, GetAttributes(true)]Guid b) => Guid.Empty; 
	}

	public class HaveAttribute
	{
		public Guid Target<[GetAttributes(true)]T>([GetAttributes(true)]T a, [GetAttributes(true)]Guid b) => Guid.Empty;
	}

	public class HaveAttributeWithEnumInConstructor
	{
		public Guid Target<[GetAttributes(SomeValues.OneMore)]T>([GetAttributes(SomeValues.OneMore)]T a, [GetAttributes(SomeValues.OneMore)]Guid b) => Guid.Empty; 
	}

	public class HaveAttributeUsingNamedArguments
	{
		public Guid Target<[GetAttributes(true, TargetString = "TargetString")]T>([GetAttributes(true, TargetString = "TargetString")]T a, [GetAttributes(true, TargetString = "TargetString")]Guid b) => Guid.Empty; 
	}

	public class HaveAttributeUsingMultipleConstructorAndNamedArguments
	{
		public Guid Target<[GetAttributes(true, 2, TargetString = "TargetString", TargetInt = 3)]T>([GetAttributes(true, 2, TargetString = "TargetString", TargetInt = 3)]T a, [GetAttributes(true, 2, TargetString = "TargetString", TargetInt = 3)]Guid b) => Guid.Empty;
	}
}