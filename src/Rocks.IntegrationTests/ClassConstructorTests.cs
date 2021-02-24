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
		public static void CreateWithNoParametersAndPublicConstructor()
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
		public static void MakeWithNoParametersAndPublicConstructor()
		{
			var chunk = Rock.Make<ClassConstructor>().Instance(3);
			var value = chunk.NoParameters();

			Assert.That(value, Is.EqualTo(default(int)));
		}

		[Test]
		public static void CreateWithNoParametersAndProtectedConstructor()
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

		[Test]
		public static void MakeWithNoParametersAndProtectedConstructor()
		{
			var chunk = Rock.Make<ClassConstructor>().Instance("b");
			var value = chunk.NoParameters();

			Assert.That(value, Is.EqualTo(default(int)));
		}
	}
}