using Rocks.Extensions;
using System;
using System.Linq.Expressions;

#nullable enable

public interface IHasMapOptions<TClass, TMember> { }

public interface IHasMap<TClass>
{
	IHasMapOptions<TClass, TMember> Map<TMember>(Expression<Func<TClass, TMember>> expression, bool useExistingMap = true);
}

public interface IHasTypeConverterOptions<TClass, TMember>
	: IHasMap<TClass>
{ }

internal sealed class IHasTypeConverterOptionsCreateExpectations<TClass, TMember>
	: global::Rocks.Expectations
{
#pragma warning disable CS8618
	internal sealed class Handler0<TMember1>
		: global::Rocks.Handler<global::System.Func<global::System.Linq.Expressions.Expression<global::System.Func<TClass, TMember1>>, bool, global::IHasMapOptions<TClass, TMember1>>, global::IHasMapOptions<TClass, TMember1>>
	{
		public global::Rocks.Argument<global::System.Linq.Expressions.Expression<global::System.Func<TClass, TMember1>>> @expression { get; set; }
		public global::Rocks.Argument<bool> @useExistingMap { get; set; }
	}
	private global::Rocks.Handlers<global::Rocks.Handler>? @handlers0;
#pragma warning restore CS8618

	public override void Verify()
	{
		if (this.WasInstanceInvoked)
		{
			var failures = new global::System.Collections.Generic.List<string>();

			if (this.handlers0 is not null) { failures.AddRange(this.Verify(this.handlers0, 0)); }

			if (failures.Count > 0)
			{
				throw new global::Rocks.Exceptions.VerificationException(failures);
			}
		}
	}

	private sealed class Mock
		: global::IHasTypeConverterOptions<TClass, TMember>
	{
		public Mock(global::IHasTypeConverterOptionsCreateExpectations<TClass, TMember> @expectations)
		{
			this.Expectations = @expectations;
		}

		[global::Rocks.MemberIdentifier(0)]
		public global::IHasMapOptions<TClass, TMember1> Map<TMember1>(global::System.Linq.Expressions.Expression<global::System.Func<TClass, TMember1>> @expression, bool @useExistingMap = true)
		{
			if (this.Expectations.handlers0 is not null)
			{
				foreach (var @genericHandler in this.Expectations.handlers0)
				{
					if (@genericHandler is global::IHasTypeConverterOptionsCreateExpectations<TClass, TMember>.Handler0<TMember1> @handler)
					{
						if (@handler.@expression.IsValid(@expression!) &&
							@handler.@useExistingMap.IsValid(@useExistingMap!))
						{
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback(@expression!, @useExistingMap!) : @handler.ReturnValue;
							return @result!;
						}
					}
				}

				throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(0)}");
			}

			throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
		}

		private global::IHasTypeConverterOptionsCreateExpectations<TClass, TMember> Expectations { get; }
	}

	internal sealed class MethodExpectations
	{
		internal MethodExpectations(global::IHasTypeConverterOptionsCreateExpectations<TClass, TMember> expectations) =>
			this.Expectations = expectations;

		internal global::IHasTypeConverterOptionsCreateExpectations<TClass, TMember>.Adornments.AdornmentsForHandler0<TMember1> Map<TMember1>(global::Rocks.Argument<global::System.Linq.Expressions.Expression<global::System.Func<TClass, TMember1>>> @expression, global::Rocks.Argument<bool> @useExistingMap)
		{
			global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
			global::System.ArgumentNullException.ThrowIfNull(@expression);
			global::System.ArgumentNullException.ThrowIfNull(@useExistingMap);

			var @handler = new global::IHasTypeConverterOptionsCreateExpectations<TClass, TMember>.Handler0<TMember1>
			{
				@expression = @expression,
				@useExistingMap = @useExistingMap.Transform(true),
			};

			if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
			else { this.Expectations.handlers0.Add(@handler); }
			return new(@handler);
		}
		internal global::IHasTypeConverterOptionsCreateExpectations<TClass, TMember>.Adornments.AdornmentsForHandler0<TMember1> Map<TMember1>(global::Rocks.Argument<global::System.Linq.Expressions.Expression<global::System.Func<TClass, TMember1>>> @expression, bool @useExistingMap = true) =>
			this.Map<TMember1>(@expression, global::Rocks.Arg.Is(@useExistingMap));

		private global::IHasTypeConverterOptionsCreateExpectations<TClass, TMember> Expectations { get; }
	}

	internal global::IHasTypeConverterOptionsCreateExpectations<TClass, TMember>.MethodExpectations Methods { get; }

	internal IHasTypeConverterOptionsCreateExpectations() =>
		(this.Methods) = (new(this));

	internal global::IHasTypeConverterOptions<TClass, TMember> Instance()
	{
		if (!this.WasInstanceInvoked)
		{
			this.WasInstanceInvoked = true;
			var @mock = new Mock(this);
			this.MockType = @mock.GetType();
			return @mock;
		}
		else
		{
			throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
		}
	}

	internal static class Adornments
	{
		public interface IAdornmentsForIHasTypeConverterOptions<TAdornments>
			: global::Rocks.IAdornments<TAdornments>
			where TAdornments : IAdornmentsForIHasTypeConverterOptions<TAdornments>
		{ }

		public sealed class AdornmentsForHandler0<TMember1>
			: global::Rocks.Adornments<AdornmentsForHandler0<TMember1>, global::IHasTypeConverterOptionsCreateExpectations<TClass, TMember>.Handler0<TMember1>, global::System.Func<global::System.Linq.Expressions.Expression<global::System.Func<TClass, TMember1>>, bool, global::IHasMapOptions<TClass, TMember1>>, global::IHasMapOptions<TClass, TMember1>>, IAdornmentsForIHasTypeConverterOptions<AdornmentsForHandler0<TMember1>>
		{
			public AdornmentsForHandler0(global::IHasTypeConverterOptionsCreateExpectations<TClass, TMember>.Handler0<TMember1> handler)
				: base(handler) { }
		}
	}
}
