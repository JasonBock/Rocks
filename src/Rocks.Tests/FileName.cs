﻿// <auto-generated/>

#nullable enable

using ProjectionsForIHaveRefStruct;
using Rocks.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ProjectionsForIHaveRefStruct
{
	internal delegate global::System.Span<int> Callback_82622901419900028928279950794957851302506027935(string @index);
	internal delegate global::System.Span<int> ReturnValue_305616756374865012389506681414051734154688895315();
	internal delegate void Callback_23785253474456163915852151378274352521926083328(string @index, global::System.Span<int> @value);
	internal delegate bool ArgEvaluationForSpanOfint(global::System.Span<int> @value);

	internal sealed class ArgForSpanOfint
		: global::Rocks.Argument
	{
		private readonly global::ProjectionsForIHaveRefStruct.ArgEvaluationForSpanOfint? evaluation;
		private readonly global::Rocks.ValidationState validation;

		internal ArgForSpanOfint() => this.validation = global::Rocks.ValidationState.None;

		internal ArgForSpanOfint(global::ProjectionsForIHaveRefStruct.ArgEvaluationForSpanOfint @evaluation)
		{
			this.evaluation = @evaluation;
			this.validation = global::Rocks.ValidationState.Evaluation;
		}

		public bool IsValid(global::System.Span<int> @value) =>
			this.validation switch
			{
				global::Rocks.ValidationState.None => true,
				global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
				_ => throw new global::System.NotSupportedException("Invalid validation state."),
			};
	}
}

internal sealed class IHaveRefStructCreateExpectations
	: global::Rocks.Expectations.ExpectationsV4
{
#pragma warning disable CS8618

	internal sealed class Handler0
		: global::Rocks.HandlerV4<global::ProjectionsForIHaveRefStruct.Callback_82622901419900028928279950794957851302506027935, global::ProjectionsForIHaveRefStruct.ReturnValue_305616756374865012389506681414051734154688895315>
	{
		public global::Rocks.Argument<string> @index { get; set; }
	}

	internal sealed class Handler1
		: global::Rocks.HandlerV4<global::ProjectionsForIHaveRefStruct.Callback_23785253474456163915852151378274352521926083328>
	{
		public global::Rocks.Argument<string> @index { get; set; }
		public global::ProjectionsForIHaveRefStruct.ArgForSpanOfint @value { get; set; }
	}

#pragma warning restore CS8618

	private readonly global::System.Collections.Generic.List<global::IHaveRefStructCreateExpectations.Handler0> @handlers0 = new();
	private readonly global::System.Collections.Generic.List<global::IHaveRefStructCreateExpectations.Handler1> @handlers1 = new();

	public override void Verify()
	{
		if (this.WasInstanceInvoked)
		{
			var failures = new global::System.Collections.Generic.List<string>();

			failures.AddRange(this.Verify(handlers0));
			failures.AddRange(this.Verify(handlers1));

			if (failures.Count > 0)
			{
				throw new global::Rocks.Exceptions.VerificationException(failures);
			}
		}
	}

	private sealed class RockIHaveRefStruct
		: global::IHaveRefStruct
	{
		public RockIHaveRefStruct(global::IHaveRefStructCreateExpectations @expectations)
		{
			this.Expectations = @expectations;
		}

		[global::Rocks.MemberIdentifier(0, "this[string @index]")]
		[global::Rocks.MemberIdentifier(1, "this[string @index]")]
		public global::System.Span<int> this[string @index]
		{
			get
			{
				if (this.Expectations.handlers0.Count > 0)
				{
					foreach (var @handler in this.Expectations.handlers0)
					{
						if (@handler.@index.IsValid(@index!))
						{
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback(@index!) : @handler.ReturnValue!();
							return @result!;
						}
					}

					throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[string @index]");
				}

				throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[string @index])");
			}
			set
			{
				if (this.Expectations.handlers1.Count > 0)
				{
					foreach (var @handler in this.Expectations.handlers1)
					{
						if (@handler.@index.IsValid(@index!) &&
							@handler.@value.IsValid(@value!))
						{
							@handler.CallCount++;
							@handler.Callback?.Invoke(@index!, @value!);
							return;
						}
					}

					throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[string @index]");
				}

				throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[string @index])");
			}
		}

		private global::IHaveRefStructCreateExpectations Expectations { get; }
	}
	internal sealed class IHaveRefStructIndexerExpectations
	{
		internal sealed class IHaveRefStructIndexerGetterExpectations
		{
			internal IHaveRefStructIndexerGetterExpectations(global::IHaveRefStructCreateExpectations expectations) =>
				this.Expectations = expectations;

			internal global::Rocks.AdornmentsV4<global::IHaveRefStructCreateExpectations.Handler0, global::ProjectionsForIHaveRefStruct.Callback_82622901419900028928279950794957851302506027935, global::ProjectionsForIHaveRefStruct.ReturnValue_305616756374865012389506681414051734154688895315> This(global::Rocks.Argument<string> @index)
			{
				global::System.ArgumentNullException.ThrowIfNull(@index);

				var handler = new global::IHaveRefStructCreateExpectations.Handler0
				{
					@index = @index,
				};

				this.Expectations.handlers0.Add(handler);
				return new(handler);
			}
			private global::IHaveRefStructCreateExpectations Expectations { get; }
		}

		internal sealed class IHaveRefStructIndexerSetterExpectations
		{
			internal IHaveRefStructIndexerSetterExpectations(global::IHaveRefStructCreateExpectations expectations) =>
				this.Expectations = expectations;

			internal global::Rocks.AdornmentsV4<global::IHaveRefStructCreateExpectations.Handler1, global::ProjectionsForIHaveRefStruct.Callback_23785253474456163915852151378274352521926083328> This(global::ProjectionsForIHaveRefStruct.ArgForSpanOfint @value, global::Rocks.Argument<string> @index)
			{
				global::System.ArgumentNullException.ThrowIfNull(@index);
				global::System.ArgumentNullException.ThrowIfNull(@value);

				var handler = new global::IHaveRefStructCreateExpectations.Handler1
				{
					@index = @index,
					@value = @value,
				};

				this.Expectations.handlers1.Add(handler);
				return new(handler);
			}
			private global::IHaveRefStructCreateExpectations Expectations { get; }
		}

		internal IHaveRefStructIndexerExpectations(global::IHaveRefStructCreateExpectations expectations) =>
			(this.Getters, this.Setters) = (new(expectations), new(expectations));

		internal global::IHaveRefStructCreateExpectations.IHaveRefStructIndexerExpectations.IHaveRefStructIndexerGetterExpectations Getters { get; }
		internal global::IHaveRefStructCreateExpectations.IHaveRefStructIndexerExpectations.IHaveRefStructIndexerSetterExpectations Setters { get; }
	}

	internal global::IHaveRefStructCreateExpectations.IHaveRefStructIndexerExpectations Indexers { get; }

	internal IHaveRefStructCreateExpectations() =>
		(this.Indexers) = (new(this));

	internal global::IHaveRefStruct Instance()
	{
		if (!this.WasInstanceInvoked)
		{
			this.WasInstanceInvoked = true;
			var @mock = new RockIHaveRefStruct(this);
			this.MockType = @mock.GetType();
			return @mock;
		}
		else
		{
			throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
		}
	}
}

public interface IHaveRefStruct
{
	Span<int> this[string index] { get; set; }
}
