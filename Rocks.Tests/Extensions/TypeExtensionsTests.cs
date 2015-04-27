using NUnit.Framework;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsTests
	{
		[Test]
		public void IsUnsafeToMockWithSafeInterfaceWithSafeMembers()
		{
			Assert.IsFalse(typeof(ISafeMembers).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeInterfaceWithUnsafeMethodWithUnsafeReturnValue()
		{
			Assert.IsTrue(typeof(IUnsafeMethodWithUnsafeReturnValue).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeInterfaceWithUnsafeMethodWithUnsafeArguments()
		{
			Assert.IsTrue(typeof(IUnsafeMethodWithUnsafeArguments).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeInterfaceWithUnsafePropertyType()
		{
			Assert.IsTrue(typeof(IUnsafePropertyWithUnsafePropertyType).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeInterfaceWithUnsafeIndexer()
		{
			Assert.IsTrue(typeof(IUnsafePropertyWithUnsafeIndexer).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithSafeInterfaceWithUnsafeEventArgs()
		{
			Assert.IsFalse(typeof(ISafeEventWithUnsafeEventArgs).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithSafeClassWithSafeMembers()
		{
			Assert.IsFalse(typeof(SafeMembers).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeClassWithUnsafeMethodWithUnsafeReturnValue()
		{
			Assert.IsTrue(typeof(UnsafeMethodWithUnsafeReturnValue).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeClassWithUnsafeMethodWithUnsafeArguments()
		{
			Assert.IsTrue(typeof(UnsafeMethodWithUnsafeArguments).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeClassWithUnsafePropertyType()
		{
			Assert.IsTrue(typeof(UnsafePropertyWithUnsafePropertyType).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeClassWithUnsafeIndexer()
		{
			Assert.IsTrue(typeof(UnsafePropertyWithUnsafeIndexer).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithSafeClassWithUnsafeEventArgs()
		{
			Assert.IsFalse(typeof(SafeEventWithUnsafeEventArgs).IsUnsafeToMock());
		}

		[Test]
		public void FindMethodWithMethodOnGivenType()
		{
			var method = typeof(IMetadata).GetMethod(nameof(IMetadata.Target));

			var foundMethod = typeof(IMetadata).FindMethod(method.MetadataToken);

			Assert.AreEqual(method, foundMethod);
		}

		[Test]
		public void FindMethodWhenTokenDoesNotExist()
		{
			Assert.IsNull(typeof(IMetadata).FindMethod(0));
		}

		[Test]
		public void FindMethodWithMethodOnBaseType()
		{
			var method = typeof(IMetadata).GetMethod(nameof(IMetadata.Target));

			var foundMethod = typeof(Metadata).FindMethod(method.MetadataToken);

			Assert.AreEqual(method, foundMethod);
		}

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
		public void GetSafeNameWithOpenGenerics()
		{
			Assert.AreEqual("IHaveGenerics", typeof(IHaveGenerics<>).GetSafeName());
		}

		[Test]
		public void GetSafeNameWithClosedGenerics()
		{
			Assert.AreEqual("IHaveGenerics", typeof(IHaveGenerics<string>).GetSafeName());
		}

		[Test]
		public void GetSafeNameWithNestedOpenGenerics()
		{
			Assert.AreEqual("NestedGenerics.IHaveGenerics", typeof(NestedGenerics.IHaveGenerics<>).GetSafeName());
		}

		[Test]
		public void GetSafeNameWithNestedClosedGenerics()
		{
			Assert.AreEqual("NestedGenerics.IHaveGenerics", typeof(NestedGenerics.IHaveGenerics<string>).GetSafeName());
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
			Assert.AreEqual("RefTargetWithGeneric",
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
			Assert.AreEqual("TypeExtensionsTests.RefTargetWithGeneric",
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
			Assert.AreEqual("MapForGeneric",
				typeof(Rocks.Tests.Extensions.MapForGeneric<Guid>).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenerics()
		{
			Assert.AreEqual("MapForGeneric",
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
			Assert.AreEqual("MapForGeneric",
				typeof(Rocks.Tests.Extensions.MapForGeneric<Guid>).GetSafeName(
					typeof(IMapToDelegates).GetMethod(nameof(IMapToDelegates.MapForGeneric)), namespaces));
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenericsAndContext()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("MapForGeneric",
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
		public void GetMockableEventsFromBaseInterface()
		{
			var events = typeof(IMockableEventsBase).GetMockableEvents();
			Assert.AreEqual(1, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == nameof(IMockableEventsBase.BaseInterfaceEvent)).Any());
		}

		[Test]
		public void GetMockableEventsFromSubInterface()
		{
			var events = typeof(IMockableEventsSub).GetMockableEvents();
			Assert.AreEqual(2, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == nameof(IMockableEventsBase.BaseInterfaceEvent)).Any());
			Assert.IsTrue(events.Where(_ => _.Name == nameof(IMockableEventsSub.SubInterfaceEvent)).Any());
		}

		[Test]
		public void GetMockableEventsFromAbstractClass()
		{
			var events = typeof(MockableEventsAbstract).GetMockableEvents();
			Assert.AreEqual(1, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == "AbstractClassEvent").Any());
		}

		[Test]
		public void GetMockableEventsFromSubClassFromAbstract()
		{
			var events = typeof(MockableEventsSubFromAbstract).GetMockableEvents();
			Assert.AreEqual(1, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == "AbstractClassEvent").Any());
		}

		[Test]
		public void GetMockableEventsFromBaseClass()
		{
			var events = typeof(MockableEventsBase).GetMockableEvents();
			Assert.AreEqual(1, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == nameof(MockableEventsBase.BaseVirtualClassEvent)).Any());
		}

		[Test]
		public void GetMockableEventsFromSubClass()
		{
			var events = typeof(MockableEventsSub).GetMockableEvents();
			Assert.AreEqual(1, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == nameof(MockableEventsBase.BaseVirtualClassEvent)).Any());
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

	public class HasNoPublicMembers
	{
		protected virtual void Target() { }
	}

	public class NestedGenerics
	{
		public interface IHaveGenerics<T> { }
	}

	public interface IHaveGenerics<T> { }

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

	public interface IMetadata
	{
		void Target();
	}

	public class Metadata
		: IMetadata
	{
		public void Target() { }
	}

	public interface ISafeMembers
	{
		void Target();
		int TargetReturn();
		int TargetProperty { get; set; }
		int this[int a] { get; set; }
		event EventHandler MyEvent;
	}

	public unsafe class UnsafeByteEventArgs : EventArgs
	{
		public byte* Value { get; set; }
	}

	public unsafe interface IUnsafeMethodWithUnsafeReturnValue
	{
		byte* Target();
	}

	public unsafe interface IUnsafeMethodWithUnsafeArguments
	{
		void Target(byte* a);
	}

	public unsafe interface IUnsafePropertyWithUnsafePropertyType
	{
		byte* Target { get; set; }
	}

	public unsafe interface IUnsafePropertyWithUnsafeIndexer
	{
		int this[byte* a] { get; set; }
	}

	public interface ISafeEventWithUnsafeEventArgs
	{
		event EventHandler<UnsafeByteEventArgs> Target;
	}

	public class SafeMembers
	{
		public virtual void Target() { }
		public virtual int TargetReturn() { return 0; }
		public virtual int TargetProperty { get; set; }
		public virtual int this[int a] { get { return 0; } set { } }
#pragma warning disable 67
		public virtual event EventHandler MyEvent;
#pragma warning restore 67
	}

	public unsafe class UnsafeMethodWithUnsafeReturnValue
	{
		public virtual byte* Target() { return default(byte*); }
	}

	public unsafe class UnsafeMethodWithUnsafeArguments
	{
		public virtual void Target(byte* a) { }
	}

	public unsafe class UnsafePropertyWithUnsafePropertyType
	{
		public virtual byte* Target { get; set; }
	}

	public unsafe class UnsafePropertyWithUnsafeIndexer
	{
		public virtual int this[byte* a] { get { return 0; } set { } }
	}

	public class SafeEventWithUnsafeEventArgs
	{
#pragma warning disable 67
		public virtual event EventHandler<UnsafeByteEventArgs> Target;
#pragma warning restore 67
	}

	public interface IMockableEventsBase
	{
		event EventHandler BaseInterfaceEvent;
	}

	public interface IMockableEventsSub
		: IMockableEventsBase
	{
		event EventHandler SubInterfaceEvent;
	}

	public abstract class MockableEventsAbstract
	{
		protected abstract event EventHandler AbstractClassEvent;
	}

#pragma warning disable 67
	public class MockableEventsSubFromAbstract
		: MockableEventsAbstract
	{
		protected override event EventHandler AbstractClassEvent;
	}

	public class MockableEventsBase
	{
		public event EventHandler BaseClassEvent;
		public virtual event EventHandler BaseVirtualClassEvent;
	}

	public class MockableEventsSub
		: MockableEventsBase, IMockableEventsSub
	{
		public event EventHandler BaseInterfaceEvent;
		public event EventHandler SubInterfaceEvent;
	}
#pragma warning restore 67
}