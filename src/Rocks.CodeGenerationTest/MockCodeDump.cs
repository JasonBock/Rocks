using Rocks.Extensions;

#pragma warning disable CS8618
#pragma warning disable CS8633
#pragma warning disable CS8714
#pragma warning disable CS8775

namespace Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes;

public interface IInterfaceGenericMethod<T>
{
	void Foo(List<string> values);
	void Quux(T value);
	void Bar<TParam>(TParam value);
	List<string> FooReturn();
	T QuuxReturn();
	TReturn BarReturn<TReturn>();
	TData? NullableValues<TData>(TData? data);
}

/// <summary>
/// Contains mocking infrastructure code for <see cref="IInterfaceGenericMethod{T}"/>.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
internal sealed class IInterfaceGenericMethodCreateExpectations<T>
	: global::Rocks.Expectations
{
	private readonly global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.SetupsExpectations setups;

	/// <summary>
	/// Contains expectation setups for mockable members on <see cref="IInterfaceGenericMethod{T}"/>.
	/// </summary>
	internal sealed class SetupsExpectations
	{
		private readonly global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T> parent;

		/// <summary>
		/// Creates a new <see cref="SetupsExpectations"/> instance.
		/// </summary>
		internal SetupsExpectations(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T> parent) =>
			this.parent = parent;

		/// <summary>
		/// Sets up an expectation for <see cref="IInterfaceGenericMethod{T}.Foo(List{string})"/>.
		/// </summary>
		internal global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.FooAdornmentsA2442B55 Foo(global::Rocks.Argument<global::System.Collections.Generic.List<string>> @values)
		{
			global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
			global::System.ArgumentNullException.ThrowIfNull(@values);

			var @handler = new global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler0
			{
				@values = @values,
			};

			this.parent.handlers0 ??= new(1);
			this.parent.handlers0.Add(@handler);
			return new(@handler, this.parent);
		}

		internal global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.QuuxAdornmentsCDCAB7EA Quux(global::Rocks.Argument<T> @value)
		{
			global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
			global::System.ArgumentNullException.ThrowIfNull(@value);

			var @handler = new global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler1
			{
				@value = @value,
			};

			if (this.parent.handlers1 is null) { this.parent.handlers1 = new(1); }
			this.parent.handlers1.Add(@handler);
			return new(@handler, this.parent);
		}

		internal global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.BarAdornmentsEC7C4823<TParam> Bar<TParam>(global::Rocks.Argument<TParam> @value)
		{
			global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
			global::System.ArgumentNullException.ThrowIfNull(@value);

			var @handler = new global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler2<TParam>
			{
				@value = @value,
			};

			if (this.parent.handlers2 is null) { this.parent.handlers2 = new(1); }
			this.parent.handlers2.Add(@handler);
			return new(@handler, this.parent);
		}

		internal global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.FooReturnAdornments2D2816FE FooReturn()
		{
			global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
			var handler = new global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler3();
			this.parent.handlers3 = handler;
			return new(handler, this.parent);
		}

		internal global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.QuuxReturnAdornments2D2816FE QuuxReturn()
		{
			global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
			var handler = new global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler4();
			this.parent.handlers4 = handler;
			return new(handler, this.parent);
		}

		internal global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.BarReturnAdornments2D2816FE<TReturn> BarReturn<TReturn>()
		{
			global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);

			var @handler = new global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler5<TReturn>
			{
			};

			if (this.parent.handlers5 is null) { this.parent.handlers5 = new(1); }
			this.parent.handlers5.Add(@handler);
			return new(@handler, this.parent);
		}

		internal global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.NullableValuesAdornments94A705A3<TData> NullableValues<TData>(global::Rocks.Argument<TData?> @data)
		{
			global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
			global::System.ArgumentNullException.ThrowIfNull(@data);

			var @handler = new global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler6<TData>
			{
				@data = @data,
			};

			if (this.parent.handlers6 is null) { this.parent.handlers6 = new(1); }
			this.parent.handlers6.Add(@handler);
			return new(@handler, this.parent);
		}
	}

	/// <summary>
	/// Contains all the setups for mockable members on <see cref="IInterfaceGenericMethod{T}"/>.
	/// </summary>
	internal global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.SetupsExpectations Setups => this.setups;

	internal sealed class Handler0
		: global::Rocks.Handler<global::System.Action<global::System.Collections.Generic.List<string>>>
	{
		internal global::Rocks.Argument<global::System.Collections.Generic.List<string>> @values { get; set; }
	}
	private global::System.Collections.Generic.List<global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler0>? @handlers0;

	internal sealed class Handler1
		: global::Rocks.Handler<global::System.Action<T>>
	{
		internal global::Rocks.Argument<T> @value { get; set; }
	}
	private global::System.Collections.Generic.List<global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler1>? @handlers1;

	internal sealed class Handler2<TParam>
		: global::Rocks.Handler<global::System.Action<TParam>>
	{
		internal global::Rocks.Argument<TParam> @value { get; set; }
	}
	private global::System.Collections.Generic.List<global::Rocks.Handler>? @handlers2;

	internal sealed class Handler3
		: global::Rocks.Handler<global::System.Func<global::System.Collections.Generic.List<string>>, global::System.Collections.Generic.List<string>>
	{ }
	private global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler3? @handlers3;

	internal sealed class Handler4
		: global::Rocks.Handler<global::System.Func<T>, T>
	{ }
	private global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler4? @handlers4;

	internal sealed class Handler5<TReturn>
		: global::Rocks.Handler<global::System.Func<TReturn>, TReturn>
	{ }
	private global::System.Collections.Generic.List<global::Rocks.Handler>? @handlers5;

	internal sealed class Handler6<TData>
		: global::Rocks.Handler<global::System.Func<TData?, TData?>, TData?>
	{
		internal global::Rocks.Argument<TData?> @data { get; set; }
	}
	private global::System.Collections.Generic.List<global::Rocks.Handler>? @handlers6;

	/// <summary>
	/// Verifies that all set expectations were matched.
	/// </summary>
	/// <exception cref="global::Rocks.Exceptions.VerificationException">Thrown if any expectations were not met.</exception>
	public override void Verify()
	{
		if (!this.WasInstanceInvoked)
		{
			throw new global::Rocks.Exceptions.VerificationException([$"An instance of global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T> was never made."]);
		}
		else if (!this.WasExceptionThrown)
		{
			var failures = new global::System.Collections.Generic.List<string>();

			if (this.handlers0 is not null) { failures.AddRange(global::Rocks.Expectations.Verify(this.handlers0, 0, typeof(Mock))); }
			if (this.handlers1 is not null) { failures.AddRange(global::Rocks.Expectations.Verify(this.handlers1, 1, typeof(Mock))); }
			if (this.handlers2 is not null) { failures.AddRange(global::Rocks.Expectations.Verify(this.handlers2, 2, typeof(Mock))); }
			if (this.handlers3 is not null) { failures.AddRange(global::Rocks.Expectations.Verify([this.handlers3], 3, typeof(Mock))); }
			if (this.handlers4 is not null) { failures.AddRange(global::Rocks.Expectations.Verify([this.handlers4], 4, typeof(Mock))); }
			if (this.handlers5 is not null) { failures.AddRange(global::Rocks.Expectations.Verify(this.handlers5, 5, typeof(Mock))); }
			if (this.handlers6 is not null) { failures.AddRange(global::Rocks.Expectations.Verify(this.handlers6, 6, typeof(Mock))); }

			if (failures.Count > 0)
			{
				throw new global::Rocks.Exceptions.VerificationException(failures);
			}
		}
	}

	private sealed class Mock
		: global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethod<T>
	{
		public Mock(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T> @expectations) => 
			this.Expectations = @expectations;

		[global::Rocks.MemberIdentifier(0)]
		public void Foo(global::System.Collections.Generic.List<string> @values)
		{
			if (this.Expectations.handlers0 is not null)
			{
				var @foundMatch = false;

				foreach (var @handler in global::System.Runtime.InteropServices.CollectionsMarshal.AsSpan(this.Expectations.handlers0))
				{
					if (@handler.@values.IsValid(@values!))
					{
						@foundMatch = true;
						@handler.CallCount++;
						if (@handler.Exception is not null) { throw @handler.Exception; }
						@handler.Callback?.Invoke(@values!);
						break;
					}
				}

				if (!@foundMatch)
				{
					this.Expectations.WasExceptionThrown = true;
					throw new global::Rocks.Exceptions.ExpectationException(
						$"""
						No handlers match for {typeof(Mock).GetMemberDescription(0)}
							values: {@values.FormatValue()}
						""");
				}
			}
			else
			{
				this.Expectations.WasExceptionThrown = true;
				throw new global::Rocks.Exceptions.ExpectationException(
					$"""
					No handlers were found for {typeof(Mock).GetMemberDescription(0)}
						values: {@values.FormatValue()}
					""");
			}
		}

		[global::Rocks.MemberIdentifier(1)]
		public void Quux(T @value)
		{
			if (this.Expectations.handlers1 is not null)
			{
				var @foundMatch = false;

				foreach (var @handler in global::System.Runtime.InteropServices.CollectionsMarshal.AsSpan(this.Expectations.handlers1))
				{
					if (@handler.@value.IsValid(@value!))
					{
						@foundMatch = true;
						@handler.CallCount++;
						if (@handler.Exception is not null) { throw @handler.Exception; }
						@handler.Callback?.Invoke(@value!);
						break;
					}
				}

				if (!@foundMatch)
				{
					this.Expectations.WasExceptionThrown = true;
					throw new global::Rocks.Exceptions.ExpectationException(
						$"""
						No handlers match for {typeof(Mock).GetMemberDescription(1)}
							value: {@value.FormatValue()}
						""");
				}
			}
			else
			{
				this.Expectations.WasExceptionThrown = true;
				throw new global::Rocks.Exceptions.ExpectationException(
					$"""
					No handlers were found for {typeof(Mock).GetMemberDescription(1)}
						value: {@value.FormatValue()}
					""");
			}
		}

		[global::Rocks.MemberIdentifier(2)]
		public void Bar<TParam>(TParam @value)
		{
			if (this.Expectations.handlers2 is not null)
			{
				var @foundMatch = false;

				foreach (var @genericHandler in global::System.Runtime.InteropServices.CollectionsMarshal.AsSpan(this.Expectations.handlers2))
				{
					if (@genericHandler is global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler2<TParam> @handler)
					{
						if (@handler.@value.IsValid(@value!))
						{
							@foundMatch = true;
							@handler.CallCount++;
							if (@handler.Exception is not null) { throw @handler.Exception; }
							@handler.Callback?.Invoke(@value!);
							break;
						}
					}
				}

				if (!@foundMatch)
				{
					this.Expectations.WasExceptionThrown = true;
					throw new global::Rocks.Exceptions.ExpectationException(
						$"""
						No handlers match for {typeof(Mock).GetMemberDescription(2)}
							value: {@value.FormatValue()}
						""");
				}
			}
			else
			{
				this.Expectations.WasExceptionThrown = true;
				throw new global::Rocks.Exceptions.ExpectationException(
					$"""
					No handlers were found for {typeof(Mock).GetMemberDescription(2)}
						value: {@value.FormatValue()}
					""");
			}
		}

		[global::Rocks.MemberIdentifier(3)]
		public global::System.Collections.Generic.List<string> FooReturn()
		{
			if (this.Expectations.handlers3 is not null)
			{
				var @handler = this.Expectations.handlers3;
				@handler.CallCount++;
				if (@handler.Exception is not null) { throw @handler.Exception; }
				var @result = @handler.Callback is not null ?
					@handler.Callback() : @handler.ReturnValue;
				return @result!;
			}
			else
			{
				this.Expectations.WasExceptionThrown = true;
				throw new global::Rocks.Exceptions.ExpectationException(
					$"""
					No handlers were found for {typeof(Mock).GetMemberDescription(3)}
					""");
			}
		}

		[global::Rocks.MemberIdentifier(4)]
		public T QuuxReturn()
		{
			if (this.Expectations.handlers4 is not null)
			{
				var @handler = this.Expectations.handlers4;
				@handler.CallCount++;
				if (@handler.Exception is not null) { throw @handler.Exception; }
				var @result = @handler.Callback is not null ?
					@handler.Callback() : @handler.ReturnValue;
				return @result!;
			}
			else
			{
				this.Expectations.WasExceptionThrown = true;
				throw new global::Rocks.Exceptions.ExpectationException(
					$"""
					No handlers were found for {typeof(Mock).GetMemberDescription(4)}
					""");
			}
		}

		[global::Rocks.MemberIdentifier(5)]
		public TReturn BarReturn<TReturn>()
		{
			if (this.Expectations.handlers5 is not null)
			{
				foreach (var @genericHandler in global::System.Runtime.InteropServices.CollectionsMarshal.AsSpan(this.Expectations.handlers5))
				{
					if (@genericHandler is global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler5<TReturn> @handler)
					{
						@handler.CallCount++;
						if (@handler.Exception is not null) { throw @handler.Exception; }
						var @result = @handler.Callback is not null ?
							@handler.Callback() : @handler.ReturnValue;
						return @result!;
					}
				}

				this.Expectations.WasExceptionThrown = true;
				throw new global::Rocks.Exceptions.ExpectationException(
					$"""
					No handlers match for {typeof(Mock).GetMemberDescription(5)}
					""");
			}
			else
			{
				this.Expectations.WasExceptionThrown = true;
				throw new global::Rocks.Exceptions.ExpectationException(
					$"""
					No handlers were found for {typeof(Mock).GetMemberDescription(5)}
					""");
			}
		}

		[global::Rocks.MemberIdentifier(6)]
		public TData? NullableValues<TData>(TData? @data)
		{
			if (this.Expectations.handlers6 is not null)
			{
				foreach (var @genericHandler in global::System.Runtime.InteropServices.CollectionsMarshal.AsSpan(this.Expectations.handlers6))
				{
					if (@genericHandler is global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler6<TData> @handler)
					{
						if (@handler.@data.IsValid(@data!))
						{
							@handler.CallCount++;
							if (@handler.Exception is not null) { throw @handler.Exception; }
							var @result = @handler.Callback is not null ?
								@handler.Callback(@data!) : @handler.ReturnValue;
							return @result!;
						}
					}
				}

				this.Expectations.WasExceptionThrown = true;
				throw new global::Rocks.Exceptions.ExpectationException(
					$"""
					No handlers match for {typeof(Mock).GetMemberDescription(6)}
						data: {@data.FormatValue()}
					""");
			}
			else
			{
				this.Expectations.WasExceptionThrown = true;
				throw new global::Rocks.Exceptions.ExpectationException(
					$"""
					No handlers were found for {typeof(Mock).GetMemberDescription(6)}
						data: {@data.FormatValue()}
					""");
			}
		}

		private global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T> Expectations { get; }
	}

	/// <summary>
	/// Create a new <see cref="IInterfaceGenericMethodCreateExpectations"/> instance.
	/// </summary>
	public IInterfaceGenericMethodCreateExpectations() => this.setups = new(this);

	/// <summary>
	/// Creates a mock instance that derives from <see cref="IInterfaceGenericMethod{T}"/>.
	/// </summary>
	/// <returns>A new mock instance.</returns>
	/// <exception cref="global::Rocks.Exceptions.NewMockInstanceException">Thrown if a mock has already been created for the current expectations instance.</exception>
	internal global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethod<T> Instance()
	{
		if (!this.WasInstanceInvoked)
		{
			this.WasInstanceInvoked = true;
			return new Mock(this);
		}
		else
		{
			throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
		}
	}

	internal static class Adornments
	{
		internal interface IAdornmentsForIInterfaceGenericMethod<TAdornments>
			: global::Rocks.IAdornments<TAdornments>
			where TAdornments : IAdornmentsForIInterfaceGenericMethod<TAdornments>
		{ }

		internal sealed class FooAdornmentsA2442B55
			: global::Rocks.Adornments<FooAdornmentsA2442B55, global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler0, global::System.Action<global::System.Collections.Generic.List<string>>>, IAdornmentsForIInterfaceGenericMethod<FooAdornmentsA2442B55>
		{
			internal FooAdornmentsA2442B55(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler0 handler, global::Rocks.Expectations expectations)
				: base(handler, expectations) { }
		}

		internal sealed class QuuxAdornmentsCDCAB7EA
			: global::Rocks.Adornments<QuuxAdornmentsCDCAB7EA, global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler1, global::System.Action<T>>, IAdornmentsForIInterfaceGenericMethod<QuuxAdornmentsCDCAB7EA>
		{
			internal QuuxAdornmentsCDCAB7EA(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler1 handler, global::Rocks.Expectations expectations)
				: base(handler, expectations) { }
		}

		internal sealed class BarAdornmentsEC7C4823<TParam>
			: global::Rocks.Adornments<BarAdornmentsEC7C4823<TParam>, global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler2<TParam>, global::System.Action<TParam>>, IAdornmentsForIInterfaceGenericMethod<BarAdornmentsEC7C4823<TParam>>
		{
			internal BarAdornmentsEC7C4823(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler2<TParam> handler, global::Rocks.Expectations expectations)
				: base(handler, expectations) { }
		}

		internal sealed class FooReturnAdornments2D2816FE
			: global::Rocks.Adornments<FooReturnAdornments2D2816FE, global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler3, global::System.Func<global::System.Collections.Generic.List<string>>, global::System.Collections.Generic.List<string>>, IAdornmentsForIInterfaceGenericMethod<FooReturnAdornments2D2816FE>
		{
			internal FooReturnAdornments2D2816FE(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler3 handler, global::Rocks.Expectations expectations)
				: base(handler, expectations) { }
		}

		internal sealed class QuuxReturnAdornments2D2816FE
			: global::Rocks.Adornments<QuuxReturnAdornments2D2816FE, global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler4, global::System.Func<T>, T>, IAdornmentsForIInterfaceGenericMethod<QuuxReturnAdornments2D2816FE>
		{
			internal QuuxReturnAdornments2D2816FE(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler4 handler, global::Rocks.Expectations expectations)
				: base(handler, expectations) { }
		}

		internal sealed class BarReturnAdornments2D2816FE<TReturn>
			: global::Rocks.Adornments<BarReturnAdornments2D2816FE<TReturn>, global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler5<TReturn>, global::System.Func<TReturn>, TReturn>, IAdornmentsForIInterfaceGenericMethod<BarReturnAdornments2D2816FE<TReturn>>
		{
			internal BarReturnAdornments2D2816FE(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler5<TReturn> handler, global::Rocks.Expectations expectations)
				: base(handler, expectations) { }
		}

		internal sealed class NullableValuesAdornments94A705A3<TData>
			: global::Rocks.Adornments<NullableValuesAdornments94A705A3<TData>, global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler6<TData>, global::System.Func<TData?, TData?>, TData?>, IAdornmentsForIInterfaceGenericMethod<NullableValuesAdornments94A705A3<TData>>
		{
			internal NullableValuesAdornments94A705A3(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Handler6<TData> handler, global::Rocks.Expectations expectations)
				: base(handler, expectations) { }
		}
	}

	internal void Remove(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.FooAdornmentsA2442B55 adornments)
	{
		adornments.Remove(this.@handlers0);
		if (this.@handlers0?.Count == 0) { this.@handlers0 = null; }
	}

	internal void Remove(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.QuuxAdornmentsCDCAB7EA adornments)
	{
		adornments.Remove(this.@handlers1);
		if (this.@handlers1?.Count == 0) { this.@handlers1 = null; }
	}

	internal void Remove<TParam>(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.BarAdornmentsEC7C4823<TParam> adornments)
	{
		adornments.RemoveHandler(this.@handlers2);
		if (this.@handlers2?.Count == 0) { this.@handlers2 = null; }
	}

	internal void Remove(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.FooReturnAdornments2D2816FE adornments) =>
		this.@handlers3 = null;

	internal void Remove(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.QuuxReturnAdornments2D2816FE adornments) =>
		this.@handlers4 = null;

	internal void Remove<TReturn>(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.BarReturnAdornments2D2816FE<TReturn> adornments)
	{
		adornments.RemoveHandler(this.@handlers5);
		if (this.@handlers5?.Count == 0) { this.@handlers5 = null; }
	}

	internal void Remove<TData>(global::Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes.IInterfaceGenericMethodCreateExpectations<T>.Adornments.NullableValuesAdornments94A705A3<TData> adornments)
	{
		adornments.RemoveHandler(this.@handlers6);
		if (this.@handlers6?.Count == 0) { this.@handlers6 = null; }
	}
}

#pragma warning restore CS8618
#pragma warning restore CS8633
#pragma warning restore CS8714
#pragma warning restore CS8775