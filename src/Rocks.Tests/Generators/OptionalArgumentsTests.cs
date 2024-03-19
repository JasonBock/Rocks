﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class OptionalArgumentsTests
{
	[Test]
	public static async Task CreateWithOptionalArgumentsAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<IHaveOptionalArguments>]

			public interface IHaveOptionalArguments
			{
				void Foo(int a, string b = "b", double c = 3.2);
				int this[int a, string b = "b"] { get; set; }
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			internal sealed class IHaveOptionalArgumentsCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Action<int, string, double>>
				{
					public global::Rocks.Argument<int> @a { get; set; }
					public global::Rocks.Argument<string> @b { get; set; }
					public global::Rocks.Argument<double> @c { get; set; }
				}
				private global::Rocks.Handlers<global::IHaveOptionalArgumentsCreateExpectations.Handler0>? @handlers0;
				internal sealed class Handler1
					: global::Rocks.Handler<global::System.Func<int, string, int>, int>
				{
					public global::Rocks.Argument<int> @a { get; set; }
					public global::Rocks.Argument<string> @b { get; set; }
				}
				private global::Rocks.Handlers<global::IHaveOptionalArgumentsCreateExpectations.Handler1>? @handlers1;
				internal sealed class Handler2
					: global::Rocks.Handler<global::System.Action<int, string, int>>
				{
					public global::Rocks.Argument<int> @a { get; set; }
					public global::Rocks.Argument<string> @b { get; set; }
					public global::Rocks.Argument<int> @value { get; set; }
				}
				private global::Rocks.Handlers<global::IHaveOptionalArgumentsCreateExpectations.Handler2>? @handlers2;
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						if (this.handlers0 is not null) { failures.AddRange(this.Verify(this.handlers0, 0)); }
						if (this.handlers1 is not null) { failures.AddRange(this.Verify(this.handlers1, 1)); }
						if (this.handlers2 is not null) { failures.AddRange(this.Verify(this.handlers2, 2)); }
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class Mock
					: global::IHaveOptionalArguments
				{
					public Mock(global::IHaveOptionalArgumentsCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void Foo(int @a, string @b = "b", double @c = 3.2)
					{
						if (this.Expectations.handlers0 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @handler in this.Expectations.handlers0)
							{
								if (@handler.@a.IsValid(@a!) &&
									@handler.@b.IsValid(@b!) &&
									@handler.@c.IsValid(@c!))
								{
									@foundMatch = true;
									@handler.CallCount++;
									@handler.Callback?.Invoke(@a!, @b!, @c!);
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
					
					[global::Rocks.MemberIdentifier(1, global::Rocks.PropertyAccessor.Get)]
					[global::Rocks.MemberIdentifier(2, global::Rocks.PropertyAccessor.Set)]
					public int this[int @a, string @b = "b"]
					{
						get
						{
							if (this.Expectations.handlers1 is not null)
							{
								foreach (var @handler in this.Expectations.handlers1)
								{
									if (@handler.@a.IsValid(@a!) &&
										@handler.@b.IsValid(@b!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@a!, @b!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(1)}");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(1)})");
						}
						set
						{
							if (this.Expectations.handlers2 is not null)
							{
								foreach (var @handler in this.Expectations.handlers2)
								{
									if (@handler.@a.IsValid(@a!) &&
										@handler.@b.IsValid(@b!) &&
										@handler.@value.IsValid(@value!))
									{
										@handler.CallCount++;
										@handler.Callback?.Invoke(@a!, @b!, @value!);
										return;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(2)}");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(2)}");
						}
					}
					
					private global::IHaveOptionalArgumentsCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IHaveOptionalArgumentsCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::IHaveOptionalArgumentsCreateExpectations.Adornments.AdornmentsForHandler0 Foo(global::Rocks.Argument<int> @a, global::Rocks.Argument<string> @b, global::Rocks.Argument<double> @c)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@a);
						global::System.ArgumentNullException.ThrowIfNull(@b);
						global::System.ArgumentNullException.ThrowIfNull(@c);
						
						var @handler = new global::IHaveOptionalArgumentsCreateExpectations.Handler0
						{
							@a = @a,
							@b = @b.Transform("b"),
							@c = @c.Transform(3.2),
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					internal global::IHaveOptionalArgumentsCreateExpectations.Adornments.AdornmentsForHandler0 Foo(global::Rocks.Argument<int> @a, string @b = "b", double @c = 3.2) =>
						this.Foo(@a, global::Rocks.Arg.Is(@b), global::Rocks.Arg.Is(@c));
					
					private global::IHaveOptionalArgumentsCreateExpectations Expectations { get; }
				}
				
				internal sealed class IndexerExpectations
				{
					internal sealed class IndexerGetterExpectations
					{
						internal IndexerGetterExpectations(global::IHaveOptionalArgumentsCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::IHaveOptionalArgumentsCreateExpectations.Adornments.AdornmentsForHandler1 This(global::Rocks.Argument<int> @a, global::Rocks.Argument<string> @b)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@a);
							global::System.ArgumentNullException.ThrowIfNull(@b);
							
							var @handler = new global::IHaveOptionalArgumentsCreateExpectations.Handler1
							{
								@a = @a,
								@b = @b.Transform("b"),
							};
							
							if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(@handler); }
							else { this.Expectations.handlers1.Add(@handler); }
							return new(@handler);
						}
						internal global::IHaveOptionalArgumentsCreateExpectations.Adornments.AdornmentsForHandler1 This(global::Rocks.Argument<int> @a, string @b = "b") =>
							this.This(@a, global::Rocks.Arg.Is(@b));
						private global::IHaveOptionalArgumentsCreateExpectations Expectations { get; }
					}
					
					internal sealed class IndexerSetterExpectations
					{
						internal IndexerSetterExpectations(global::IHaveOptionalArgumentsCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::IHaveOptionalArgumentsCreateExpectations.Adornments.AdornmentsForHandler2 This(global::Rocks.Argument<int> @value, global::Rocks.Argument<int> @a, global::Rocks.Argument<string> @b)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@a);
							global::System.ArgumentNullException.ThrowIfNull(@b);
							global::System.ArgumentNullException.ThrowIfNull(@value);
							
							var @handler = new global::IHaveOptionalArgumentsCreateExpectations.Handler2
							{
								@a = @a,
								@b = @b.Transform("b"),
								@value = @value,
							};
							
							if (this.Expectations.handlers2 is null) { this.Expectations.handlers2 = new(@handler); }
							else { this.Expectations.handlers2.Add(@handler); }
							return new(@handler);
						}
						internal global::IHaveOptionalArgumentsCreateExpectations.Adornments.AdornmentsForHandler2 This(global::Rocks.Argument<int> @value, global::Rocks.Argument<int> @a, string @b = "b") =>
							this.This(@value, @a, global::Rocks.Arg.Is(@b));
						private global::IHaveOptionalArgumentsCreateExpectations Expectations { get; }
					}
					
					internal IndexerExpectations(global::IHaveOptionalArgumentsCreateExpectations expectations) =>
						(this.Getters, this.Setters) = (new(expectations), new(expectations));
					
					internal global::IHaveOptionalArgumentsCreateExpectations.IndexerExpectations.IndexerGetterExpectations Getters { get; }
					internal global::IHaveOptionalArgumentsCreateExpectations.IndexerExpectations.IndexerSetterExpectations Setters { get; }
				}
				
				internal global::IHaveOptionalArgumentsCreateExpectations.MethodExpectations Methods { get; }
				internal global::IHaveOptionalArgumentsCreateExpectations.IndexerExpectations Indexers { get; }
				
				internal IHaveOptionalArgumentsCreateExpectations() =>
					(this.Methods, this.Indexers) = (new(this), new(this));
				
				internal global::IHaveOptionalArguments Instance()
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
					public interface IAdornmentsForIHaveOptionalArguments<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIHaveOptionalArguments<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::IHaveOptionalArgumentsCreateExpectations.Handler0, global::System.Action<int, string, double>>, IAdornmentsForIHaveOptionalArguments<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::IHaveOptionalArgumentsCreateExpectations.Handler0 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler1
						: global::Rocks.Adornments<AdornmentsForHandler1, global::IHaveOptionalArgumentsCreateExpectations.Handler1, global::System.Func<int, string, int>, int>, IAdornmentsForIHaveOptionalArguments<AdornmentsForHandler1>
					{
						public AdornmentsForHandler1(global::IHaveOptionalArgumentsCreateExpectations.Handler1 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler2
						: global::Rocks.Adornments<AdornmentsForHandler2, global::IHaveOptionalArgumentsCreateExpectations.Handler2, global::System.Action<int, string, int>>, IAdornmentsForIHaveOptionalArguments<AdornmentsForHandler2>
					{
						public AdornmentsForHandler2(global::IHaveOptionalArgumentsCreateExpectations.Handler2 handler)
							: base(handler) { }
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "IHaveOptionalArguments_Rock_Create.g.cs", generatedCode)],
			[]);
	}
}