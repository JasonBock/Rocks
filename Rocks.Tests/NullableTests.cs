using NUnit.Framework;

namespace Rocks.Tests
{
   public static class NullableTests
   {
		[Test]
		public static void MockWithNullableTypes()
		{
			var rock = Rock.Create<INullable>();
			rock.Handle(_ => _.GoWithNullableReferenceParameter<string>(Arg.IsAny<string?>()));
			rock.Handle(_ => _.GoWithNullableReferenceReturn<string>(Arg.IsAny<string>()));
			rock.Handle(_ => _.GoWithNullableValueParameter(Arg.IsAny<int?>()));
			rock.Handle(_ => _.GoWithNullableValueReturn(Arg.IsAny<int>()));

			var chunk = rock.Make();
			chunk.GoWithNullableReferenceParameter("a");
			_ = chunk.GoWithNullableReferenceReturn("a");
			chunk.GoWithNullableValueParameter<int>(1);
			_ = chunk.GoWithNullableValueReturn(1);

			rock.Verify();
		}
   }

   public interface INullable
   {
		U GoWithNullableReferenceParameter<U>(U? value) where U : class;
		U? GoWithNullableReferenceReturn<U>(U value) where U : class;
		U GoWithNullableValueParameter<U>(U? value) where U : struct;
		U? GoWithNullableValueReturn<U>(U value) where U : struct;
   }
}