using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class NonPublicTests
	{
		[Test]
		public static void GenerateWhenPublicAndNonPublicMembersExist()
		{
			var rock = Rock.Create<NonPublicAndPublicMembers>();
			Assert.That(rock.Make(), Is.Not.Null);
		}
	}

	public abstract class NonPublicAndPublicMembers
	{
		public abstract void TargetMethod();
		public abstract Guid TargetProperty { get; set; }
		public abstract event EventHandler TargetEvent;

		protected abstract Guid ProtectedMethodGuid();
		protected abstract Guid ProtectedMethodGuidWithOut(out int a);
		protected abstract void ProtectedMethodVoid();
		protected abstract void ProtectedMethodVoidWithOut(out int a);

		internal abstract Guid InternalMethodGuid();
		internal abstract Guid InternalMethodGuidWithOut(out int a);
		internal abstract void InternalMethodVoid();
		internal abstract void InternalMethodVoidWithOut(out int a);

		protected abstract Guid ProtectedProperty { get; set; }
		internal abstract Guid InternalProperty { get; set; }

		protected abstract event EventHandler ProtectedEvent;
		internal abstract event EventHandler InternalEvent;
	}
}