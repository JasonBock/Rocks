//namespace Rocks.CodeGenerationTest;

//// The base stuff.
//public interface ITestAdornments<TAdornments>
//	where TAdornments : ITestAdornments<TAdornments>
//{
//	TAdornments AddRaiseEvent(RaiseEventInformation raiseEventInformation);
//	TAdornments ExpectedCallCount(uint expectedCallCount);
//}

//public interface ITestAdornments<TAdornments, TCallback>
//	: ITestAdornments<TAdornments>
//	where TAdornments : ITestAdornments<TAdornments, TCallback>
//	where TCallback : Delegate
//{
//	TAdornments Callback(TCallback callback);
//}

//public interface ITestAdornments<TAdornments, TCallback, TReturnValue>
//	: ITestAdornments<TAdornments>
//	where TAdornments : ITestAdornments<TAdornments, TCallback, TReturnValue>
//	where TCallback : Delegate
//{
//	TAdornments Callback(TCallback callback);
//	TAdornments ReturnValue(TReturnValue returnValue);
//}

//public class TestAdornments<TAdornments, TCallback>
//	: ITestAdornments<TAdornments, TCallback>
//	where TAdornments : TestAdornments<TAdornments, TCallback>
//	where TCallback : Delegate
//{
//   public TAdornments AddRaiseEvent(RaiseEventInformation raiseEventInformation) => (TAdornments)this;
//	public TAdornments Callback(TCallback callback) => (TAdornments)this;
//	public TAdornments ExpectedCallCount(uint expectedCallCount) => (TAdornments)this;
//}

//public class TestAdornments<TAdornments, TCallback, TReturnValue>
//	: ITestAdornments<TAdornments, TCallback, TReturnValue>
//	where TAdornments : TestAdornments<TAdornments, TCallback, TReturnValue>
//	where TCallback : Delegate
//{
//	public TAdornments AddRaiseEvent(RaiseEventInformation raiseEventInformation) => (TAdornments)this;
//	public TAdornments Callback(TCallback callback) => (TAdornments)this;
//	public TAdornments ExpectedCallCount(uint expectedCallCount) => (TAdornments)this;
//	public TAdornments ReturnValue(TReturnValue returnValue) => (TAdornments)this;
//}

//// Custom for a type called "Mock".
//public interface ITestAdornmentsForMock<TAdornments>
//	: ITestAdornments<TAdornments>
//	where TAdornments : ITestAdornmentsForMock<TAdornments>
//{ }

//// Custom for a type called "Handler0" on "Mock".
//// The gen'd code would just add the handler name,
//// but for the example it's clearer.
//public sealed class AdornmentsForHandler0OnMock
//	: TestAdornments<AdornmentsForHandler0OnMock, Action>, ITestAdornmentsForMock<AdornmentsForHandler0OnMock>
//{ }

//public static class MockAdornmentsEventExtensions
//{
//	public static TAdornments RaiseMyEvent<TAdornments>(this TAdornments self, EventArgs args) where TAdornments : ITestAdornmentsForMock<TAdornments> =>
//		self.AddRaiseEvent(new("MyEvent", args));
//}

//// Custom for a type called "Rock" .
//public interface ITestAdornmentsForRock<TAdornments>
//	: ITestAdornments<TAdornments>
//	where TAdornments : ITestAdornmentsForRock<TAdornments>
//{ }

//// Custom for a type called "Handler0" on "Rock".
//// The gen'd code would just add the handler name,
//// but for the example it's clearer.
//public sealed class AdornmentsForHandler0OnRock
//	: TestAdornments<AdornmentsForHandler0OnRock, Action>, ITestAdornmentsForRock<AdornmentsForHandler0OnRock>
//{ }

//public static class RockAdornmentsEventExtensions
//{
//	public static TAdornments RaiseRockThrown<TAdornments>(this TAdornments self, EventArgs args) where TAdornments : ITestAdornmentsForRock<TAdornments> =>
//		self.AddRaiseEvent(new("RockThrown", args));
//}

//public static class TestAdornmentsIdea
//{
//	public static void Go()
//	{
//		var adornments = new AdornmentsForHandler0OnMock();
//		adornments.RaiseMyEvent(new()).ExpectedCallCount(2).ExpectedCallCount(3);

//		var rockAdornments = new AdornmentsForHandler0OnRock();
//		rockAdornments.RaiseRockThrown(new()).ExpectedCallCount(2).ExpectedCallCount(3);
//	}
//}
