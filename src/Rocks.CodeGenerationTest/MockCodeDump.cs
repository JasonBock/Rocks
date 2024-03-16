using Rocks.Extensions;

#nullable enable

public interface IPixelOperations<TPixel> where TPixel : unmanaged
{
	void Destructive(Span<TPixel> destinationPixels);
}

internal sealed class IPixelOperationsCreateExpectations<TPixel>
	: global::Rocks.Expectations
	where TPixel : unmanaged
{
	internal static class Projections
	{
		internal delegate void Callback_6880507936518113545154238437489757677796400069(global::System.Span<TPixel> @destinationPixels);
		internal delegate bool ArgumentEvaluationForSpan<TPixel1>(global::System.Span<TPixel1> @value);

		internal sealed class ArgumentForSpan<TPixel1>
			: global::Rocks.Argument
		{
			private readonly global::IPixelOperationsCreateExpectations<TPixel>.Projections.ArgumentEvaluationForSpan<TPixel1>? evaluation;
			private readonly global::Rocks.ValidationState validation;

			internal ArgumentForSpan() => this.validation = global::Rocks.ValidationState.None;

			internal ArgumentForSpan(global::IPixelOperationsCreateExpectations<TPixel>.Projections.ArgumentEvaluationForSpan<TPixel1> @evaluation)
			{
				this.evaluation = @evaluation;
				this.validation = global::Rocks.ValidationState.Evaluation;
			}

			public bool IsValid(global::System.Span<TPixel1> @value) =>
				this.validation switch
				{
					global::Rocks.ValidationState.None => true,
					global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
					_ => throw new global::System.NotSupportedException("Invalid validation state."),
				};
		}
	}

#pragma warning disable CS8618
	internal sealed class Handler0
		: global::Rocks.Handler<global::IPixelOperationsCreateExpectations<TPixel>.Projections.Callback_6880507936518113545154238437489757677796400069>
	{
		public global::IPixelOperationsCreateExpectations<TPixel>.Projections.ArgumentForSpan<TPixel> @destinationPixels { get; set; }
	}
	private global::Rocks.Handlers<global::IPixelOperationsCreateExpectations<TPixel>.Handler0>? @handlers0;
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
		: global::IPixelOperations<TPixel>
	{
		public Mock(global::IPixelOperationsCreateExpectations<TPixel> @expectations)
		{
			this.Expectations = @expectations;
		}

		[global::Rocks.MemberIdentifier(0)]
		public void Destructive(global::System.Span<TPixel> @destinationPixels)
		{
			if (this.Expectations.handlers0 is not null)
			{
				var @foundMatch = false;

				foreach (var @handler in this.Expectations.handlers0)
				{
					if (@handler.@destinationPixels.IsValid(@destinationPixels!))
					{
						@foundMatch = true;
						@handler.CallCount++;
						@handler.Callback?.Invoke(@destinationPixels!);
						break;
					}
				}

				if (!@foundMatch)
				{
					throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(0)}");
				}
			}
			else
			{
				throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
			}
		}

		private global::IPixelOperationsCreateExpectations<TPixel> Expectations { get; }
	}

	internal sealed class MethodExpectations
	{
		internal MethodExpectations(global::IPixelOperationsCreateExpectations<TPixel> expectations) =>
			this.Expectations = expectations;

		internal global::IPixelOperationsCreateExpectations<TPixel>.Adornments.AdornmentsForHandler0 Destructive(global::IPixelOperationsCreateExpectations<TPixel>.Projections.ArgumentForSpan<TPixel> @destinationPixels)
		{
			global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
			global::System.ArgumentNullException.ThrowIfNull(@destinationPixels);

			var @handler = new global::IPixelOperationsCreateExpectations<TPixel>.Handler0
			{
				@destinationPixels = @destinationPixels,
			};

			if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
			else { this.Expectations.handlers0.Add(@handler); }
			return new(@handler);
		}

		private global::IPixelOperationsCreateExpectations<TPixel> Expectations { get; }
	}

	internal global::IPixelOperationsCreateExpectations<TPixel>.MethodExpectations Methods { get; }

	internal IPixelOperationsCreateExpectations() =>
		(this.Methods) = (new(this));

	internal global::IPixelOperations<TPixel> Instance()
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
		public interface IAdornmentsForIPixelOperations<TAdornments>
			: global::Rocks.IAdornments<TAdornments>
			where TAdornments : IAdornmentsForIPixelOperations<TAdornments>
		{ }

		public sealed class AdornmentsForHandler0
			: global::Rocks.Adornments<AdornmentsForHandler0, global::IPixelOperationsCreateExpectations<TPixel>.Handler0, global::IPixelOperationsCreateExpectations<TPixel>.Projections.Callback_6880507936518113545154238437489757677796400069>, IAdornmentsForIPixelOperations<AdornmentsForHandler0>
		{
			public AdornmentsForHandler0(global::IPixelOperationsCreateExpectations<TPixel>.Handler0 handler)
				: base(handler) { }
		}
	}
}
