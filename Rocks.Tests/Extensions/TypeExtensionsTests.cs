using NUnit.Framework;
using Rocks.Construction.InMemory;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsTests
	{
		[Test]
		public void ContainsRefArguments() =>
			Assert.That(typeof(IAmTypeWithMethodWithRefArgument).ContainsRefAndOrOutParameters(), Is.True);

		[Test]
		public void ContainsOutArguments() =>
			Assert.That(typeof(IAmTypeWithMethodWithOutArgument).ContainsRefAndOrOutParameters(), Is.True);

		[Test]
		public void ContainsByValArguments() =>
			Assert.That(typeof(IAmTypeWithMethodWithByValArgument).ContainsRefAndOrOutParameters(), Is.False);

		[Test]
		public void GetMockableMethodsFromSubInterfaceWhenBaseInterfaceHasIdenticalMethod()
		{
			var methods = typeof(IHaveSameMethodAsBaseInterface).GetMockableMethods(new InMemoryNameGenerator());
			Assert.That(methods.Count, Is.EqualTo(1));
			Assert.That(methods.Where(_ => _.Value.Name == nameof(IHaveSameMethodAsBaseInterface.GetNames)).Any(), Is.True);
		}
	}

	public class HasNoPublicMembers
	{
		protected virtual void Target() { }
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

	public interface IHaveSameMethodAsSubInterface
	{
		void GetNames(int memid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2), Out]string[] rgBstrNames, int cMaxNames, out int pcNames);
	}

	public interface IHaveSameMethodAsBaseInterface
		: IHaveSameMethodAsSubInterface
	{
		new void GetNames(int memid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2), Out]string[] rgBstrNames, int cMaxNames, out int pcNames);
	}

	public interface IAmTypeWithMethodWithOutArgument
	{
		void Target(out int a);
	}

	public interface IAmTypeWithMethodWithRefArgument
	{
		void Target(out int a);
	}

	public interface IAmTypeWithMethodWithByValArgument
	{
		void Target(int a);
	}
}