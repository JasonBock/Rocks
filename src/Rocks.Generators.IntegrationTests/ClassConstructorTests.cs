using NUnit.Framework;

namespace Rocks.IntegrationTests
{
	public class ClassConstructor
	{
		protected ClassConstructor(string stringData) => 
			this.StringData = stringData;
		public ClassConstructor(int intData) => 
			this.IntData = intData;

		public virtual int NoParameters() => default;

		public int IntData { get;  }
		public string? StringData { get; }
	}

	public static class ClassConstructorTests
	{
		[Test]
		public static void MockWithNoParametersAndPublicConstructor()
		{
			var rock = Rock.Create<ClassConstructor>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance(3);
			var value = chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(chunk.IntData, Is.EqualTo(3));
				Assert.That(chunk.StringData, Is.Null);
			});
		}

		[Test]
		public static void MockWithNoParametersAndProtectedConstructor()
		{
			var rock = Rock.Create<ClassConstructor>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance("b");
			var value = chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(chunk.IntData, Is.EqualTo(default(int)));
				Assert.That(chunk.StringData, Is.EqualTo("b"));
			});
		}
	}
}