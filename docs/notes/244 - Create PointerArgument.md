Where would I need to change? Just creates, not makes.

* MockMethodValueBuilder
* MockMethodVoidBuilder
* MockMethodExtensionsBuilder
* MethodExpectationsExtensionsBuilder
* MethodExpectationsExtensionsMethodBuilder

Consider changing `T` to `TMock` to be more explicit with naming.

Why am I still generating these types? Can't I make "PointerHandlerInformation<>" and "PointerMethodAdornments<>"
* HandlerInformationForintPointer 
* MethodAdornmentsForintPointer

* MockIndexerBuilder
* IndexerExpectationsExtensionsIndexerBuilder
* ExplicitIndexerExpectationsExtensionsIndexerBuilder


PointerArgTypeBuilder can probably change to UnmanagedFunctionPointerArgTypeBuilder

OK...I still don't think I can do this, because the expectation extensions + the adornments, it's too "general". BUT, once I do cast-free work, I can come back to this and add in all the pointer types. So I'm going to capture the code here, and come back to it later.

In `TypeReferenceModel`:

```csharp
this.PointerType =
    this.IsPointer ?
        type.Kind == SymbolKind.PointerType ?
            new TypeReferenceModel(((IPointerTypeSymbol)type).PointedAtType, compilation) :
            ((IFunctionPointerTypeSymbol)type).BaseType is not null ?
                new TypeReferenceModel(((IFunctionPointerTypeSymbol)type).BaseType!, compilation) : null :
        null;
```

`PointerArgument`

```csharp
using System.ComponentModel;

namespace Rocks;

internal unsafe delegate bool PointerArgumentEvaluation<T>(T* @value) where T : unmanaged;

/// <summary>
/// Defines the type for a pointer argument expectation.
/// </summary>
/// <typeparam name="T">The type of the argument.</typeparam>
public sealed unsafe class PointerArgument<T>
	: Argument
	where T : unmanaged
{
	private readonly PointerArgumentEvaluation<T>? evaluation;
	private readonly T* value;
	private readonly ValidationState validation;

	internal PointerArgument() => this.validation = ValidationState.None;

	internal PointerArgument(T* @value)
	{
		this.value = @value;
		this.validation = ValidationState.Value;
	}

	internal PointerArgument(PointerArgumentEvaluation<T> @evaluation)
	{
		this.evaluation = @evaluation;
		this.validation = ValidationState.Evaluation;
	}

	/// <summary>
	/// Converts a pointer value to a <see cref="PointerArgument{T}"/> instance.
	/// </summary>
	/// <param name="value">The pointer value.</param>
	/// <returns>A new <see cref="PointerArgument{T}"/> instance.</returns>
	public static implicit operator PointerArgument<T>(T* @value) => new(@value);

	/// <summary>
	/// Determines if the given pointer value matches the expectation.
	/// </summary>
	/// <param name="value">The pointer value to test.</param>
	/// <returns><c>true</c> if validation is successful; otherwise, <c>false</c>.</returns>
	/// <exception cref="NotSupportedException"></exception>
	/// <exception cref="InvalidEnumArgumentException"></exception>
	public bool IsValid(T* @value) =>
		this.validation switch
		{
			ValidationState.None => true,
			ValidationState.Value => @value == this.value,
			ValidationState.Evaluation => this.evaluation!(@value),
			ValidationState.DefaultValue => throw new NotSupportedException("Cannot validate an argument value in the ValidationState.DefaultValue state."),
			_ => throw new InvalidEnumArgumentException($"Invalid value for validation: {this.validation}")
		};
}
```

`PointerHandlerInformation<T>`

```csharp
using System.Collections.Immutable;

namespace Rocks;

/// <summary>
/// Specifies expectations on a member
/// that returns a pointer value.
/// </summary>
/// <typeparam name="T">The type of the return value.</typeparam>
[Serializable]
public unsafe sealed class PointerHandlerInformation<T>
	 : HandlerInformation
	 where T : unmanaged
{
	/// <summary>
	/// Creates a new <see cref="PointerHandlerInformation{T}"/> instance
	/// with a set of argument expectations.
	/// </summary>
	/// <param name="expectations">A set of argument expectations.</param>
	internal PointerHandlerInformation(ImmutableArray<Argument> expectations)
		: base(null, expectations) => this.ReturnValue = default;

	/// <summary>
	/// Creates a new <see cref="PointerHandlerInformation{T}"/> instance
	/// with a set of argument expectations
	/// and a validation delegate.
	/// </summary>
	/// <param name="expectations">A set of argument expectations.</param>
	/// <param name="method">The validation delegate.</param>
	internal PointerHandlerInformation(Delegate? method, ImmutableArray<Argument> expectations)
		: base(method, expectations) => this.ReturnValue = default;

	/// <summary>
	/// Gets or sets the return value.
	/// </summary>
	public T* ReturnValue { get; internal set; }
}
```

`PointerMethodAdornments` types

```csharp
namespace Rocks;

/// <summary>
/// Defines adornments for a mocked method
/// with a return value.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
public unsafe sealed class PointerMethodAdornments<T, TCallback, TResult>
	: IAdornments<PointerHandlerInformation<TResult>>
	where T : class
	where TCallback : Delegate
	where TResult : unmanaged
{
	/// <summary>
	/// Creates a new <see cref="PointerMethodAdornments{T, TCallback, TResult}"/> instance
	/// with a handler.
	/// </summary>
	/// <param name="handler">The handler to wrap.</param>
	public PointerMethodAdornments(PointerHandlerInformation<TResult> handler) =>
		this.Handler = handler;

	/// <summary>
	/// Sets the handler with a call count and returns itself.
	/// </summary>
	/// <param name="expectedCallCount">The expected call count.</param>
	public PointerMethodAdornments<T, TCallback, TResult> CallCount(uint expectedCallCount)
	{
		this.Handler.SetExpectedCallCount(expectedCallCount);
		return this;
	}

	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	public PointerMethodAdornments<T, TCallback, TResult> Callback(TCallback callback)
	{
		this.Handler.SetCallback(callback);
		return this;
	}

	/// <summary>
	/// Sets the handler with a return value and returns itself.
	/// </summary>
	/// <param name="returnValue">The return value.</param>
	public PointerMethodAdornments<T, TCallback, TResult> Returns(TResult* returnValue)
	{
		this.Handler.ReturnValue = returnValue;
		return this;
	}

	/// <summary>
	/// Gets the handler.
	/// </summary>
	public PointerHandlerInformation<TResult> Handler { get; }
}
```