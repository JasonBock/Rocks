using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Rocks.Tests.Extensions
{
	public static class IEnumerableOfAssemblyExtensionsTests
	{
		[Test]
		public static void Transform()
		{
			var references = new[] { typeof(Guid).Assembly }.Transform().ToList();
			Assert.That(references.Count, Is.EqualTo(1));
		}

		[Test]
		public static void TransformWhenAssemblyIsDynamic()
		{
			var @dynamic = new DynamicAssembly();
			var references = new[] { @dynamic }.Transform().ToList();
			Assert.That(references.Count, Is.EqualTo(0));
		}

		[Test]
		public static void TransformWhenAssemblyHasNoLocation()
		{
			var rock = Rock.Create<IHaveNoLocation>();
			rock.Handle(_ => _.Foo());
			var chunk = rock.Make();

			var references = new[] { chunk.GetType().Assembly }.Transform().ToList();
			Assert.That(references.Count, Is.EqualTo(0));
		}

		internal sealed class DynamicAssembly 
			: Assembly
		{
			public override bool IsDynamic => true;
		}
	}

	public interface IHaveNoLocation
	{
		void Foo();
	}
}
