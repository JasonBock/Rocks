using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Collections.Generic;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsTests
	{
		[Test]
		public void GetSafeName()
		{
			Assert.AreEqual("TypeExtensionsTests.SubnestedClass.IAmSubnested",
				typeof(SubnestedClass.IAmSubnested).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithNoGenerics()
		{
			Assert.AreEqual("RefTargetWithoutGeneric",
				typeof(Rocks.Tests.Extensions.RefTargetWithoutGeneric).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithGenerics()
		{
			Assert.AreEqual("RefTargetWithGeneric<Guid>",
				typeof(Rocks.Tests.Extensions.RefTargetWithGeneric<Guid>).GetSafeName());
		}

		[Test]
		public void GetSafeNameForNestedDelegateWithNoGenerics()
		{
			Assert.AreEqual("TypeExtensionsTests.RefTargetWithoutGeneric",
				typeof(Rocks.Tests.Extensions.TypeExtensionsTests.RefTargetWithoutGeneric).GetSafeName());
		}

		[Test]
		public void GetSafeNameForNestedDelegateWithGenerics()
		{
			Assert.AreEqual("TypeExtensionsTests.RefTargetWithGeneric<Guid>",
				typeof(Rocks.Tests.Extensions.TypeExtensionsTests.RefTargetWithGeneric<Guid>).GetSafeName());
		}

		[Test]
		public void ContainsRefArguments()
		{
			Assert.IsTrue(typeof(IHaveMethodWithRefArgument).ContainsRefAndOrOutParameters());
		}

		[Test]
		public void ContainsOutArguments()
		{
			Assert.IsTrue(typeof(IHaveMethodWithOutArgument).ContainsRefAndOrOutParameters());
		}

		[Test]
		public void ContainsByValArguments()
		{
			Assert.IsFalse(typeof(IHaveMethodWithByValArgument).ContainsRefAndOrOutParameters());
		}

		[Test]
		public void GetImplementedProperties()
		{
			var expectedProperties =
@"public Int32 Property { get; set; }
public Int32 ReadOnly { get; }
public Int32 WriteOnly { set; }
public String this[Int32 index] { set; }
public String this[String key] { get; }";

			var type = typeof(ITypeExtensions);
			var namespaces = new SortedSet<string>();
			var properties = type.GetImplementedProperties(namespaces);
			Assert.AreEqual(expectedProperties, properties, nameof(properties));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
		}

		[Test]
		public void GetImplementedEvents()
		{
			var expectedEvents =
@"public event EventHandler Event;
public event EventHandler<MyGenericEventArgs> GenericEvent;";

			var type = typeof(ITypeExtensions);
			var namespaces = new SortedSet<string>();
			var events = type.GetImplementedEvents(namespaces);
			Assert.AreEqual(expectedEvents, events, nameof(events));
			Assert.AreEqual(2, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains("Rocks.Tests.Extensions"), nameof(namespaces.Contains));
		}

		public class SubnestedClass
		{
			public interface IAmSubnested { }
		}

		public delegate void RefTargetWithoutGeneric(ref Guid a);
		public delegate void RefTargetWithGeneric<T>(ref T a);
	}

	public delegate void RefTargetWithoutGeneric(ref Guid a);
	public delegate void RefTargetWithGeneric<T>(ref T a);

	public class MyGenericEventArgs : EventArgs { }

	public interface IHaveMethodWithOutArgument
	{
		void Target(out int a);
	}

	public interface IHaveMethodWithRefArgument
	{
		void Target(out int a);
	}

	public interface IHaveMethodWithByValArgument
	{
		void Target(int a);
	}

	public interface ITypeExtensions
	{
		int Property { get; set; }
		int ReadOnly { get; }
		int WriteOnly { set; }
		string this[int index] { set; }
		string this[string key] { get; }
		event EventHandler Event;
		event EventHandler<MyGenericEventArgs> GenericEvent;
	}
}
