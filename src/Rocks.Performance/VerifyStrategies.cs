using BenchmarkDotNet.Attributes;
using Rocks.Exceptions;
using Rocks.Extensions;

namespace Rocks.Performance;

#pragma warning disable CS8618
[MemoryDiagnoser]
public class VerifyStrategies
{
	private CurrentExpectations noCurrentExpectations;
	private CurrentExpectations oneCurrentExpectations;
	private CurrentExpectations oneAllFailCurrentExpectations;
	private CurrentExpectations manyCurrentExpectations;
	private CurrentExpectations manyAllFailCurrentExpectations;

	[GlobalSetup]
	public void GlobalSetup()
	{
		this.noCurrentExpectations = new CurrentExpectations();
		_ = this.noCurrentExpectations.Instance();

		this.oneCurrentExpectations = new CurrentExpectations();
		this.oneCurrentExpectations.Setups.NoParameters();
		this.oneCurrentExpectations.Setups.OneParameter(3);
		this.oneCurrentExpectations.Setups.MultipleParameters(3, "3");
		var oneCurrent = this.oneCurrentExpectations.Instance();
		oneCurrent.NoParameters();
		oneCurrent.OneParameter(3);
		oneCurrent.MultipleParameters(3, "3");

		this.oneAllFailCurrentExpectations = new CurrentExpectations();
		this.oneAllFailCurrentExpectations.Setups.NoParameters();
		this.oneAllFailCurrentExpectations.Setups.OneParameter(3);
		this.oneAllFailCurrentExpectations.Setups.MultipleParameters(3, "3");
		_ = this.oneAllFailCurrentExpectations.Instance();

		this.manyCurrentExpectations = new CurrentExpectations();
		this.manyCurrentExpectations.Setups.NoParameters();
		this.manyCurrentExpectations.Setups.OneParameter(1);
		this.manyCurrentExpectations.Setups.OneParameter(2);
		this.manyCurrentExpectations.Setups.OneParameter(3);
		this.manyCurrentExpectations.Setups.MultipleParameters(1, "1");
		this.manyCurrentExpectations.Setups.MultipleParameters(2, "2");
		this.manyCurrentExpectations.Setups.MultipleParameters(3, "3");
		var manyCurrent = this.manyCurrentExpectations.Instance();
		manyCurrent.NoParameters();
		manyCurrent.OneParameter(1);
		manyCurrent.OneParameter(2);
		manyCurrent.OneParameter(3);
		manyCurrent.MultipleParameters(1, "1");
		manyCurrent.MultipleParameters(2, "2");
		manyCurrent.MultipleParameters(3, "3");

		this.manyAllFailCurrentExpectations = new CurrentExpectations();
		this.manyAllFailCurrentExpectations.Setups.NoParameters();
		this.manyAllFailCurrentExpectations.Setups.OneParameter(1);
		this.manyAllFailCurrentExpectations.Setups.OneParameter(2);
		this.manyAllFailCurrentExpectations.Setups.OneParameter(3);
		this.manyAllFailCurrentExpectations.Setups.MultipleParameters(1, "1");
		this.manyAllFailCurrentExpectations.Setups.MultipleParameters(2, "2");
		this.manyAllFailCurrentExpectations.Setups.MultipleParameters(3, "3");
		_ = this.manyAllFailCurrentExpectations.Instance();
	}

	[Benchmark]
	public void NoExpectationsCurrentWay() =>
		this.noCurrentExpectations.Verify();

	[Benchmark]
	public void OneExpectationCurrentWay() =>
		this.oneCurrentExpectations.Verify();

	[Benchmark]
	public void OneExpectationAllFailCurrentWay() =>
		this.oneAllFailCurrentExpectations.Verify();

	[Benchmark]
	public void ManyExpectationsCurrentWay() =>
		this.manyCurrentExpectations.Verify();

	[Benchmark]
	public void ManyExpectationsAllFailCurrentWay() =>
		this.manyAllFailCurrentExpectations.Verify();
}

public interface IInterfaceMethodVoid
{
	void NoParameters();
	void OneParameter(int a);
	void MultipleParameters(int a, string b);
}

#pragma warning disable CS8633
#pragma warning disable CS8714
#pragma warning disable CS8775

internal sealed class CurrentExpectations
	: Expectations
{
	private readonly SetupsExpectations setups;

	internal sealed class SetupsExpectations
	{
		private readonly CurrentExpectations parent;

		internal SetupsExpectations(CurrentExpectations parent) =>
			this.parent = parent;

		internal Adornments.AdornmentsForHandler0 NoParameters()
		{
			ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
			var handler = new Handler0();
			if (this.parent.handlers0 is null) { this.parent.handlers0 = new(1); }
			this.parent.handlers0.Add(handler);
			return new(handler, this.parent);
		}

		internal Adornments.AdornmentsForHandler1 OneParameter(Argument<int> @a)
		{
			ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
			ArgumentNullException.ThrowIfNull(@a);

			var @handler = new Handler1
			{
				@a = @a,
			};

			if (this.parent.handlers1 is null) { this.parent.handlers1 = new(1); }
			this.parent.handlers1.Add(@handler);
			return new(@handler, this.parent);
		}

		internal Adornments.AdornmentsForHandler2 MultipleParameters(Argument<int> @a, Argument<string> @b)
		{
			ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
			ArgumentNullException.ThrowIfNull(@a);
			ArgumentNullException.ThrowIfNull(@b);

			var @handler = new Handler2
			{
				@a = @a,
				@b = @b,
			};

			if (this.parent.handlers2 is null) { this.parent.handlers2 = new(1); }
			this.parent.handlers2.Add(@handler);
			return new(@handler, this.parent);
		}
	}

	internal SetupsExpectations Setups => this.setups;

	internal sealed class Handler0
		: Handler<Action>
	{ }
	private List<Handler0>? @handlers0;

	internal sealed class Handler1
		: Handler<Action<int>>
	{
		public Argument<int> @a { get; set; }
	}
	private List<Handler1>? @handlers1;

	internal sealed class Handler2
		: Handler<Action<int, string>>
	{
		public Argument<int> @a { get; set; }
		public Argument<string> @b { get; set; }
	}
	private List<Handler2>? @handlers2;

	// Note: Verify used to return a List<string>.
	// These performance tests has a Verify2() that returned an IEnumerable<string>,
	// which ended up being slightly better.
	// Verify2() was removed, and Verify() was changed to return IEnumerable<string>.
	public override void Verify()
	{
		var failures = new List<string>();

		if (this.handlers0 is not null) { failures.AddRange(this.Verify(this.handlers0, 0)); }
		if (this.handlers1 is not null) { failures.AddRange(this.Verify(this.handlers1, 1)); }
		if (this.handlers2 is not null) { failures.AddRange(this.Verify(this.handlers2, 2)); }
	}

	private sealed class Mock
		: IInterfaceMethodVoid
	{
		public Mock(CurrentExpectations @expectations) => this.Expectations = @expectations;

		[MemberIdentifier(0)]
		public void NoParameters()
		{
			if (this.Expectations.handlers0 is not null)
			{
				var @handler = this.Expectations.handlers0[0];
				@handler.CallCount++;
				@handler.Callback?.Invoke();
			}
			else
			{
				this.Expectations.WasExceptionThrown = true;
				throw new Exceptions.ExpectationException(
					$"""
					No handlers were found for {this.GetType().GetMemberDescription(0)}
					""");
			}
		}

		[MemberIdentifier(1)]
		public void OneParameter(int @a)
		{
			if (this.Expectations.handlers1 is not null)
			{
				var @foundMatch = false;

				foreach (var @handler in this.Expectations.handlers1)
				{
					if (@handler.@a.IsValid(@a!))
					{
						@foundMatch = true;
						@handler.CallCount++;
						@handler.Callback?.Invoke(@a!);
						break;
					}
				}

				if (!@foundMatch)
				{
					this.Expectations.WasExceptionThrown = true;
					throw new Exceptions.ExpectationException(
						$"""
						No handlers match for {this.GetType().GetMemberDescription(1)}
							a: {@a.FormatValue()}
						""");
				}
			}
			else
			{
				this.Expectations.WasExceptionThrown = true;
				throw new Exceptions.ExpectationException(
					$"""
					No handlers were found for {this.GetType().GetMemberDescription(1)}
						a: {@a.FormatValue()}
					""");
			}
		}

		[MemberIdentifier(2)]
		public void MultipleParameters(int @a, string @b)
		{
			if (this.Expectations.handlers2 is not null)
			{
				var @foundMatch = false;

				foreach (var @handler in this.Expectations.handlers2)
				{
					if (@handler.@a.IsValid(@a!) &&
						@handler.@b.IsValid(@b!))
					{
						@foundMatch = true;
						@handler.CallCount++;
						@handler.Callback?.Invoke(@a!, @b!);
						break;
					}
				}

				if (!@foundMatch)
				{
					this.Expectations.WasExceptionThrown = true;
					throw new Exceptions.ExpectationException(
						$"""
						No handlers match for {this.GetType().GetMemberDescription(2)}
							a: {@a.FormatValue()}
							b: {@b.FormatValue()}
						""");
				}
			}
			else
			{
				this.Expectations.WasExceptionThrown = true;
				throw new Exceptions.ExpectationException(
					$"""
					No handlers were found for {this.GetType().GetMemberDescription(2)}
						a: {@a.FormatValue()}
						b: {@b.FormatValue()}
					""");
			}
		}

		private CurrentExpectations Expectations { get; }
	}

	public CurrentExpectations() => this.setups = new(this);

	internal IInterfaceMethodVoid Instance()
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
			throw new NewMockInstanceException("Can only create a new mock once.");
		}
	}

	internal static class Adornments
	{
		public interface IAdornmentsForIInterfaceMethodVoid<TAdornments>
			: IAdornments<TAdornments>
			where TAdornments : IAdornmentsForIInterfaceMethodVoid<TAdornments>
		{ }

		public sealed class AdornmentsForHandler0
			: Adornments<AdornmentsForHandler0, Handler0, Action>, IAdornmentsForIInterfaceMethodVoid<AdornmentsForHandler0>
		{
			public AdornmentsForHandler0(Handler0 handler, Expectations expectations)
				: base(handler, expectations) { }
		}

		public sealed class AdornmentsForHandler1
			: Adornments<AdornmentsForHandler1, Handler1, Action<int>>, IAdornmentsForIInterfaceMethodVoid<AdornmentsForHandler1>
		{
			public AdornmentsForHandler1(Handler1 handler, Expectations expectations)
				: base(handler, expectations) { }
		}

		public sealed class AdornmentsForHandler2
			: Adornments<AdornmentsForHandler2, Handler2, Action<int, string>>, IAdornmentsForIInterfaceMethodVoid<AdornmentsForHandler2>
		{
			public AdornmentsForHandler2(Handler2 handler, Expectations expectations)
				: base(handler, expectations) { }
		}
	}
}
