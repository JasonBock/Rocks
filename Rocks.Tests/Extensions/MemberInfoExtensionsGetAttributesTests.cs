using NUnit.Framework;
using System;
using static Rocks.Extensions.MemberInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MemberInfoExtensionsGetAttributesTests
	{
		[Test]
		public void GetAttributesForMemberWithNoAttributes()
		{
			Assert.AreEqual(string.Empty, typeof(HaveNoAttributes).GetAttributes());
		}

		[Test]
		public void GetAttributesForMemberWithAttribute()
		{
			Assert.AreEqual("[GetAttributes(True)]", typeof(HaveAttribute).GetAttributes());
		}

		[Test]
		public void GetAttributesForMemberWithAttributeUsingEnumInConstructor()
		{
			Assert.AreEqual("[GetAttributes((SomeValues)2)]", typeof(HaveAttributeWithEnumInConstructor).GetAttributes());
		}

		[Test]
		public void GetAttributesForMemberWithAttributeUsingNamedArguments()
		{
			Assert.AreEqual("[GetAttributes(True, TargetString = \"TargetString\")]", typeof(HaveAttributeUsingNamedArguments).GetAttributes());
		}

		[Test]
		public void GetAttributesForMemberWithAttributeUsingMultipleConstructorAndNamedArguments()
		{
			Assert.AreEqual("[GetAttributes(True, 2, TargetString = \"TargetString\", TargetInt = 3)]", typeof(HaveAttributeUsingMultipleConstructorAndNamedArguments).GetAttributes());
		}

		[Test]
		public void GetAttributesForMemberWithMultipleAttributes()
		{
			Assert.AreEqual("[Mutliple, GetAttributes(True)]", typeof(HaveMultipleAttributes).GetAttributes());
		}
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
		public string TargetString { get; set;  }
		public int TargetInt { get; set; }
	}

	public sealed class MutlipleAttribute : Attribute { }

	public class HaveNoAttributes { }

	[Mutliple, GetAttributes(true)]
	public class HaveMultipleAttributes { }

	[GetAttributes(true)]
	public class HaveAttribute { }

	[GetAttributes(SomeValues.OneMore)]
	public class HaveAttributeWithEnumInConstructor { }

	[GetAttributes(true, TargetString = "TargetString")]
	public class HaveAttributeUsingNamedArguments { }

	[GetAttributes(true, 2, TargetString = "TargetString", TargetInt = 3)]
	public class HaveAttributeUsingMultipleConstructorAndNamedArguments { }
}
