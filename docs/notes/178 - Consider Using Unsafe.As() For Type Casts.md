To replace casts with `Unsafe.As()`...https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafe.as

I need to import System.Runtime.CompilerServices

For method handlers:

```csharp
if (methodHandler.Method is not null)
{
	((Action<int, string>)methodHandler.Method)(a, b);
}
```

This makes sense to me to do this:

```csharp
if (methodHandler.Method is not null)
{
	Unsafe.As<Action<int, string>>(methodHandler.Method)(a, b);
}
```

For method parameters:

```csharp
public override void MultipleParameters(int a, string b)
{
	if (this.handlers.TryGetValue(5, out var methodHandlers))
	{
		var foundMatch = false;
		
		foreach (var methodHandler in methodHandlers)
		{
			if (((methodHandler.Expectations[0] as Argument<int>)?.IsValid(a) ?? false) &&
				((methodHandler.Expectations[1] as Argument<string>)?.IsValid(b) ?? false))
```
							
It's weird that I do an `as` here. Why wouldn't I just do a direct cast? Well, you could pass in something like this:

```csharp
expectations.Methods().MultipleParameters(3, null!);
```

Which would lead to `Expectations[1]` to be `null`, and that would be bad. With the current implementation, it's safe.

Could I make `Argument<T>` a `struct`? I'm not sure why it derives from `Argument`. Well, it's because:

```csharp
public HandlerInformation(ImmutableArray<Argument> expectations)
```

This is why the casts happen, and it really needs to stay this way. But I really should not allow a `null` to ever be passed into an argument. What you really want is `Arg.Is(null)`, or `(string)null!`;

```csharp
expectations.Methods().MultipleParameters(3, Arg.Is<string>(null!));

var mock = expectations.Instance();
mock.MultipleParameters(3, null!);
```

Both would pass in a valid `Argument<string>` value that would look for null.

Maybe I should add some kind of analyzer to say, hey, don't pass in null. What's arguably better is to add null checks for all of the `Argument` parameters. So this:

```csharp
internal static MethodAdornments<MethodVoidTests, Action<int, string>> MultipleParameters(
	this MethodExpectations<MethodVoidTests> self, Argument<int> a, Argument<string> b) =>
		new MethodAdornments<MethodVoidTests, Action<int, string>>(self.Add(5, new List<Argument>(2) { a, b }));
```

Would change to this:

```csharp
internal static MethodAdornments<MethodVoidTests, Action<int, string>> MultipleParameters(
	this MethodExpectations<MethodVoidTests> self, Argument<int> a, Argument<string> b)
{
	ArgumentNullException.ThrowIfNull(a);
	ArgumentNullException.ThrowIfNull(b);
	return new MethodAdornments<MethodVoidTests, Action<int, string>>(self.Add(5, new List<Argument>(2) { a, b }));
}
```

I'm still not entirely thrilled that `Handlers` on `Expectations<T>` is a getter, and that it's possible to send nulls to `.Add()`. I'm wondering if I should do this:

* On the constructors and Add(), check all arguments for nulls.
* Maybe change Add() to take an array, I always know when I code gen just how many arguments will be passed.
* Add AddOrUpdate() to Expectations<>, then Handlers wouldn't need to be exposed

Anyway, this is how it would change:

```csharp
public override void MultipleParameters(int a, string b)
{
	if (this.handlers.TryGetValue(5, out var methodHandlers))
	{
		var foundMatch = false;
		
		foreach (var methodHandler in methodHandlers)
		{
			if ((Argument<int>)(methodHandler.Expectations[0]).IsValid(a) &&
				(Argument<string>)(methodHandler.Expectations[1]).IsValid(b))
```

I think for now, these are the tasks:

* DONE - When I generate the extension methods, add null checks for all the Argument parameters (+ tests)
	* DONE - Methods
	* DONE - Indexers
* Whenever I make a cast to the expectations, change it to a cast
	* Methods
	* Indexers
* Could "Argument" turn into an interface, and call it "IArgument"? I mean, there's no reason for it to be a class.

Got to the 3rd bullet, and got some unexpected test errors with `InvalidCastException` no longer happening. So, I'm not going to go through with `Unsafe.As()`, but I am going to keep the 1st bullet point work, because I think there's value in ensuring nulls aren't passed for `Argument<>` values.

But...here's an idea. If the method has no generic parameters (which can be found with `method.IsGenericMethod`), doing the casts is "safe". So, I could do something where, if the method has no generic parameters, do the casts. Otherwise, do the `as` approach. Is that performant? Should do a benchmark test to see if that cast is faster than the "as" approach. Then, actually, I could also do the casts on the method callbacks as well

```
|     Method |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----------:|----------:|----------:|----------:|------:|--------:|----------:|------------:|
|     AsCast | 0.3223 ns | 0.0565 ns | 0.0472 ns | 0.3284 ns |  1.00 |    0.00 |         - |          NA |
| DirectCast | 0.2900 ns | 0.0425 ns | 0.0376 ns | 0.3049 ns |  0.92 |    0.23 |         - |          NA |
|   UnsafeAs | 0.0194 ns | 0.0153 ns | 0.0128 ns | 0.0234 ns |  0.06 |    0.05 |         - |          NA |


|                Method | value |      Mean |     Error |    StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|---------------------- |------ |----------:|----------:|----------:|------:|--------:|----------:|------------:|
|     IsValidWithAsCast |     2 | 10.469 ns | 0.1208 ns | 0.1130 ns |  1.00 |    0.00 |         - |          NA |
| IsValidWithDirectCast |     2 | 10.189 ns | 0.1551 ns | 0.1451 ns |  0.97 |    0.02 |         - |          NA |
|   IsValidWithUnsafeAs |     2 | 12.852 ns | 0.1545 ns | 0.1445 ns |  1.23 |    0.02 |         - |          NA |
|                       |       |           |           |           |       |         |           |             |
|     IsValidWithAsCast |     3 |  8.754 ns | 0.0875 ns | 0.0819 ns |  1.00 |    0.00 |         - |          NA |
| IsValidWithDirectCast |     3 |  8.565 ns | 0.0735 ns | 0.0574 ns |  0.98 |    0.01 |         - |          NA |
|   IsValidWithUnsafeAs |     3 |  7.963 ns | 0.1008 ns | 0.0943 ns |  0.91 |    0.02 |         - |          NA |
```

So...it seems like doing either a cast or `Unsafe.As()` is faster than `as`, but...it's very small. According to this - https://github.com/ecoAPM/BenchmarkMockNet/blob/main/Results.md - it would only shave off a couple of nanoseconds at best.

There is something else to consider:

```csharp
[MemberIdentifier(8, "TReturn BarReturn<TReturn>()")]
public override TReturn BarReturn<TReturn>()
{
	if (this.handlers.TryGetValue(8, out var methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method is not null ?
			((Func<TReturn>)methodHandler.Method)() :
			((HandlerInformation<TReturn>)methodHandler).ReturnValue;
		methodHandler.IncrementCallCount();
		return result!;
	}
	
	throw new ExpectationException("No handlers were found for TReturn BarReturn<TReturn>()");
}
```

In the case where I made the expectation for `int`, but if I invoke it like `BarReturn<string>()` (see the `CreateWithGenericParameterTypeAsReturnThatDoesNotMatch()` test), and I set `.Method` (via `.Returns()`), then I get an `InvalidCastException`. I think what I should do in the case of the method being generic is this:

```csharp
[MemberIdentifier(8, "TReturn BarReturn<TReturn>()")]
public override TReturn BarReturn<TReturn>()
{
	if (this.handlers.TryGetValue(8, out var methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method is not null && methodHandler.Method is Func<TReturn> methodReturn ?
			methodReturn() :
			methodHandler is HandlerInformation<TReturn> returnValue ?
				returnValue.ReturnValue :
				throw new MockException($"No return value could be obtained for TReturn of type {typeof(TReturn).FullName}.");
		methodHandler.IncrementCallCount();
		return result!;
	}
	
	throw new ExpectationException("No handlers were found for TReturn BarReturn<TReturn>()");
}
```

A new exception type would be created, but this I think would be slightly better. This would only be in the case where a method defines the return type as a generic.