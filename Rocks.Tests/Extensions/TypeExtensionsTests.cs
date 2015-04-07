using NUnit.Framework;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsTests
	{
		[Test]
		public void FindPropertyWhenPropertyDoesNotExist()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty("x", PropertyAccessors.Get));
		}

		[Test]
		public void FindReadOnlyPropertyWhenPropertyAccessorIsGet()
		{
			var name = nameof(ITypeExtensions.ReadOnly);
			Assert.AreEqual(name, typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.Get).Name);
		}

		[Test]
		public void FindReadOnlyPropertyWhenPropertyAccessorIsGetAndSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.GetAndSet));
		}

		[Test]
		public void FindReadOnlyPropertyWhenPropertyAccessorIsSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.Set));
		}

		[Test]
		public void FindWriteOnlyPropertyWhenPropertyAccessorIsGet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.Get));
		}

		[Test]
		public void FindWriteOnlyPropertyWhenPropertyAccessorIsGetAndSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.GetAndSet));
		}

		[Test]
		public void FindWriteOnlyPropertyWhenPropertyAccessorIsSet()
		{
			var name = nameof(ITypeExtensions.WriteOnly);
			Assert.AreEqual(name, typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.Set).Name);
		}

		[Test]
		public void FindReadWritePropertyWhenPropertyAccessorIsGet()
		{
			var name = nameof(ITypeExtensions.Property);
			Assert.AreEqual(name, typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.Get).Name);
		}

		[Test]
		public void FindReadWritePropertyWhenPropertyAccessorIsGetAndSet()
		{
			var name = nameof(ITypeExtensions.Property);
			Assert.AreEqual(name, typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.GetAndSet).Name);
		}

		[Test]
		public void FindReadWritePropertyWhenPropertyAccessorIsSet()
		{
			var name = nameof(ITypeExtensions.Property);
			Assert.AreEqual(name, typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.Set).Name);
		}

		[Test]
		public void FindIndexerPropertyWhenPropertyDoesNotExist()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(new Type[] { typeof(double) }, PropertyAccessors.Get));
		}

		[Test]
		public void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsGet()
		{
			Assert.AreEqual("Item", typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.Get).Name);
		}

		[Test]
		public void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsGetAndSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.GetAndSet));
		}

		[Test]
		public void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.Set));
		}

		[Test]
		public void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsGet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.Get));
		}

		[Test]
		public void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsGetAndSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.GetAndSet));
		}

		[Test]
		public void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsSet()
		{
			Assert.AreEqual("Item", typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.Set).Name);
		}

		[Test]
		public void FindReadWriteIndexerPropertyWhenPropertyAccessorIsGet()
		{
			Assert.AreEqual("Item", typeof(ITypeExtensions).FindProperty(new[] { typeof(Guid) }, PropertyAccessors.Get).Name);
		}

		[Test]
		public void FindReadWriteIndexerPropertyWhenPropertyAccessorIsGetAndSet()
		{
			Assert.AreEqual("Item", typeof(ITypeExtensions).FindProperty(new[] { typeof(Guid) }, PropertyAccessors.GetAndSet).Name);
		}

		[Test]
		public void FindReadWriteIndexerPropertyWhenPropertyAccessorIsSet()
		{
			Assert.AreEqual("Item", typeof(ITypeExtensions).FindProperty(new[] { typeof(Guid) }, PropertyAccessors.Set).Name);
		}

		[Test]
		public void GetConstraintsForTypeWithNoConstraints()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual(string.Empty, typeof(IHaveGenericsWithNoConstraints<>).GetGenericArguments()[0].GetConstraints(namespaces));
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}

		[Test]
		public void GetConstraintsForMethodWithConstraints()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("where T : class", typeof(IHaveGenericsWithConstraints<>).GetGenericArguments()[0].GetConstraints(namespaces));
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}

		[Test]
		public void GetSafeName()
		{
			Assert.AreEqual("TypeExtensionsTests.SubnestedClass.IAmSubnested",
				typeof(SubnestedClass.IAmSubnested).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithNoGenericsAndRefArguments()
		{
			Assert.AreEqual("RefTargetWithoutGeneric",
				typeof(Rocks.Tests.Extensions.RefTargetWithoutGeneric).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenericsAndRefArguments()
		{
			Assert.AreEqual("RefTargetWithGeneric<T>",
				typeof(Rocks.Tests.Extensions.RefTargetWithGeneric<>).GetSafeName());
		}

		[Test]
		public void GetSafeNameForNestedDelegateWithNoGenericsAndRefArguments()
		{
			Assert.AreEqual("TypeExtensionsTests.RefTargetWithoutGeneric",
				typeof(Rocks.Tests.Extensions.TypeExtensionsTests.RefTargetWithoutGeneric).GetSafeName());
		}

		[Test]
		public void GetSafeNameForNestedDelegateWithGenericsAndRefArguments()
		{
			Assert.AreEqual("TypeExtensionsTests.RefTargetWithGeneric<Guid>",
				typeof(Rocks.Tests.Extensions.TypeExtensionsTests.RefTargetWithGeneric<Guid>).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithNoGenerics()
		{
			Assert.AreEqual("MapForNonGeneric",
				typeof(Rocks.Tests.Extensions.MapForNonGeneric).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithSpecifiedGenerics()
		{
			Assert.AreEqual("MapForGeneric<Guid>",
				typeof(Rocks.Tests.Extensions.MapForGeneric<Guid>).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenerics()
		{
			Assert.AreEqual("MapForGeneric<T>",
				typeof(Rocks.Tests.Extensions.MapForGeneric<>).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithNoGenericsAndContext()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("MapForNonGeneric",
				typeof(Rocks.Tests.Extensions.MapForNonGeneric).GetSafeName(
					typeof(IMapToDelegates).GetMethod(nameof(IMapToDelegates.MapForNonGeneric)), namespaces));
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}

		[Test]
		public void GetSafeNameForDelegateWithSpecifiedGenericsAndContext()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("MapForGeneric<T>",
				typeof(Rocks.Tests.Extensions.MapForGeneric<Guid>).GetSafeName(
					typeof(IMapToDelegates).GetMethod(nameof(IMapToDelegates.MapForGeneric)), namespaces));
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenericsAndContext()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("MapForGeneric<T>",
				typeof(Rocks.Tests.Extensions.MapForGeneric<>).GetSafeName(
					typeof(IMapToDelegates).GetMethod(nameof(IMapToDelegates.MapForGeneric)), namespaces));
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
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
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains(this.GetType().Namespace), nameof(namespaces.Contains));
		}

		public interface IHaveGenericsWithNoConstraints<T> { }

		public interface IHaveGenericsWithConstraints<T> where T : class { }

		public class SubnestedClass
		{
			public interface IAmSubnested { }
		}

		public delegate void RefTargetWithoutGeneric(ref Guid a);
		public delegate void RefTargetWithGeneric<T>(ref T a);
	}

	public interface IMapToDelegates
	{
		void MapForNonGeneric(int a);
		void MapForGeneric<T>(T a);
	}

	public delegate void MapForNonGeneric(int a);
	public delegate void MapForGeneric<T>(T a);

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
		string this[Guid data] { get; set; }
		string this[string key] { get; }
		string this[int index] { set; }
		event EventHandler Event;
		event EventHandler<MyGenericEventArgs> GenericEvent;
	}
}
