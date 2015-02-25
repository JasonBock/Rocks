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
		public void GetImplementedProperties()
		{
			var expectedProperties =
@"public Int32 Property { get; set; }
public Int32 ReadOnly { get; }
public Int32 WriteOnly { set; }
public String this[Int32 index] { get; set; }
public String this[String key] { get; set; }";

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
	}

	public class MyGenericEventArgs : EventArgs { }

	public interface ITypeExtensions
	{
		int Property { get; set; }
		int ReadOnly { get; }
		int WriteOnly { set; }
		string this[int index] { get; set; }
		string this[string key] { get; set; }
		event EventHandler Event;
		event EventHandler<MyGenericEventArgs> GenericEvent;
	}
}
