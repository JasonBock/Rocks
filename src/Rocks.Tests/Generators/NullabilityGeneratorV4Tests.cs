﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class NullabilityGeneratorV4Tests
{
	[Test]
	public static async Task GenerateWithAllowNullParameterAndDefaultValueAsync()
	{
		var code =
			"""
			using Rocks;
			using System.Diagnostics.CodeAnalysis;

			[assembly: RockCreate<ConnectionBuilder<object>>]

			#nullable enable

			public interface IGraphType { }

			public class ConnectionBuilder<TSourceType>
			{
				public virtual ConnectionBuilder<TSourceType> Argument<TArgumentGraphType, TArgumentType>(string name, string? description,
					[AllowNull] TArgumentType defaultValue = default!)
						where TArgumentGraphType : IGraphType => new();
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			internal sealed class ConnectionBuilderOfobjectCreateExpectations
				: global::Rocks.Expectations.ExpectationsV4
			{
				#pragma warning disable CS8618
				
				internal sealed class Handler0
					: global::Rocks.HandlerV4<global::System.Func<object?, bool>, bool>
				{
					public global::Rocks.Argument<object?> @obj { get; set; }
				}
				
				internal sealed class Handler1
					: global::Rocks.HandlerV4<global::System.Func<int>, int>
				{ }
				
				internal sealed class Handler2
					: global::Rocks.HandlerV4<global::System.Func<string?>, string?>
				{ }
				
				internal sealed class Handler3<TArgumentGraphType, TArgumentType>
					: global::Rocks.HandlerV4<global::System.Func<string, string?, TArgumentType, global::ConnectionBuilder<object>>, global::ConnectionBuilder<object>>
					where TArgumentGraphType : global::IGraphType
				{
					public global::Rocks.Argument<string> @name { get; set; }
					public global::Rocks.Argument<string?> @description { get; set; }
					public global::Rocks.Argument<TArgumentType> @defaultValue { get; set; }
				}
				
				#pragma warning restore CS8618
				
				private readonly global::System.Collections.Generic.List<global::ConnectionBuilderOfobjectCreateExpectations.Handler0> @handlers0 = new();
				private readonly global::System.Collections.Generic.List<global::ConnectionBuilderOfobjectCreateExpectations.Handler1> @handlers1 = new();
				private readonly global::System.Collections.Generic.List<global::ConnectionBuilderOfobjectCreateExpectations.Handler2> @handlers2 = new();
				private readonly global::System.Collections.Generic.List<global::Rocks.HandlerV4> @handlers3 = new();
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						failures.AddRange(this.Verify(handlers0));
						failures.AddRange(this.Verify(handlers1));
						failures.AddRange(this.Verify(handlers2));
						failures.AddRange(this.Verify(handlers3));
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class RockConnectionBuilderOfobject
					: global::ConnectionBuilder<object>
				{
					public RockConnectionBuilderOfobject(global::ConnectionBuilderOfobjectCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0, "bool Equals(object? @obj)")]
					public override bool Equals(object? @obj)
					{
						if (this.Expectations.handlers0.Count > 0)
						{
							foreach (var @handler in this.Expectations.handlers0)
							{
								if (@handler.@obj.IsValid(@obj!))
								{
									@handler.CallCount++;
									var @result = @handler.Callback is not null ?
										@handler.Callback(@obj!) : @handler.ReturnValue;
									return @result!;
								}
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool Equals(object? @obj)");
						}
						else
						{
							return base.Equals(obj: @obj!);
						}
					}
					
					[global::Rocks.MemberIdentifier(1, "int GetHashCode()")]
					public override int GetHashCode()
					{
						if (this.Expectations.handlers1.Count > 0)
						{
							var @handler = this.Expectations.handlers1[0];
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						else
						{
							return base.GetHashCode();
						}
					}
					
					[global::Rocks.MemberIdentifier(2, "string? ToString()")]
					public override string? ToString()
					{
						if (this.Expectations.handlers2.Count > 0)
						{
							var @handler = this.Expectations.handlers2[0];
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						else
						{
							return base.ToString();
						}
					}
					
					[global::Rocks.MemberIdentifier(3, "global::ConnectionBuilder<object> Argument<TArgumentGraphType, TArgumentType>(string @name, string? @description, TArgumentType @defaultValue)")]
					public override global::ConnectionBuilder<object> Argument<TArgumentGraphType, TArgumentType>(string @name, string? @description, [global::System.Diagnostics.CodeAnalysis.AllowNullAttribute] TArgumentType @defaultValue = default!)
					{
						if (this.Expectations.handlers3.Count > 0)
						{
							foreach (var @genericHandler in this.Expectations.handlers3)
							{
								if (@genericHandler is global::ConnectionBuilderOfobjectCreateExpectations.Handler3<TArgumentGraphType, TArgumentType> @handler)
								{
									if (@handler.@name.IsValid(@name!) &&
										@handler.@description.IsValid(@description!) &&
										@handler.@defaultValue.IsValid(@defaultValue!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@name!, @description!, @defaultValue!) : @handler.ReturnValue;
										return @result!;
									}
								}
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers match for global::ConnectionBuilder<object> Argument<TArgumentGraphType, TArgumentType>(string @name, string? @description, [global::System.Diagnostics.CodeAnalysis.AllowNullAttribute] TArgumentType @defaultValue = default!)");
						}
						else
						{
							return base.Argument<TArgumentGraphType, TArgumentType>(name: @name!, description: @description!, defaultValue: @defaultValue!);
						}
					}
					
					private global::ConnectionBuilderOfobjectCreateExpectations Expectations { get; }
				}
				
				internal sealed class ConnectionBuilderOfobjectMethodExpectations
				{
					internal ConnectionBuilderOfobjectMethodExpectations(global::ConnectionBuilderOfobjectCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::Rocks.AdornmentsV4<global::ConnectionBuilderOfobjectCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						
						var handler = new global::ConnectionBuilderOfobjectCreateExpectations.Handler0
						{
							@obj = @obj,
						};
						
						this.Expectations.handlers0.Add(handler);
						return new(handler);
					}
					
					internal new global::Rocks.AdornmentsV4<global::ConnectionBuilderOfobjectCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
					{
						var handler = new global::ConnectionBuilderOfobjectCreateExpectations.Handler1();
						this.Expectations.handlers1.Add(handler);
						return new(handler);
					}
					
					internal new global::Rocks.AdornmentsV4<global::ConnectionBuilderOfobjectCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
					{
						var handler = new global::ConnectionBuilderOfobjectCreateExpectations.Handler2();
						this.Expectations.handlers2.Add(handler);
						return new(handler);
					}
					
					internal global::Rocks.AdornmentsV4<global::ConnectionBuilderOfobjectCreateExpectations.Handler3<TArgumentGraphType, TArgumentType>, global::System.Func<string, string?, TArgumentType, global::ConnectionBuilder<object>>, global::ConnectionBuilder<object>> Argument<TArgumentGraphType, TArgumentType>(global::Rocks.Argument<string> @name, global::Rocks.Argument<string?> @description, global::Rocks.Argument<TArgumentType> @defaultValue) where TArgumentGraphType : global::IGraphType
					{
						global::System.ArgumentNullException.ThrowIfNull(@name);
						global::System.ArgumentNullException.ThrowIfNull(@description);
						global::System.ArgumentNullException.ThrowIfNull(@defaultValue);
						
						var handler = new global::ConnectionBuilderOfobjectCreateExpectations.Handler3<TArgumentGraphType, TArgumentType>
						{
							@name = @name,
							@description = @description,
							@defaultValue = @defaultValue.Transform(default!),
						};
						
						this.Expectations.handlers3.Add(handler);
						return new(handler);
					}
					internal global::Rocks.AdornmentsV4<global::ConnectionBuilderOfobjectCreateExpectations.Handler3<TArgumentGraphType, TArgumentType>, global::System.Func<string, string?, TArgumentType, global::ConnectionBuilder<object>>, global::ConnectionBuilder<object>> Argument<TArgumentGraphType, TArgumentType>(global::Rocks.Argument<string> @name, global::Rocks.Argument<string?> @description, TArgumentType @defaultValue = default!) where TArgumentGraphType : global::IGraphType =>
						this.Argument<TArgumentGraphType, TArgumentType>(@name, @description, global::Rocks.Arg.Is(@defaultValue));
					
					private global::ConnectionBuilderOfobjectCreateExpectations Expectations { get; }
				}
				
				internal global::ConnectionBuilderOfobjectCreateExpectations.ConnectionBuilderOfobjectMethodExpectations Methods { get; }
				
				internal ConnectionBuilderOfobjectCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::ConnectionBuilder<object> Instance()
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockConnectionBuilderOfobject(this);
						this.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "ConnectionBuilderobject_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenNullabilityChangesInOverrideAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<SubTracer>]

			#nullable enable

			public class Tracer
			{
				public virtual void TraceEvent(string? eventCache, string source, 
					string eventType, int id, 
					string? format, params object?[]? args)
				{ }
			}

			public class SubTracer
				: Tracer
			{
				public override void TraceEvent(string eventCache, string source, 
					string eventType, int id, 
					string format, params object[] args)
				{ }
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			internal sealed class SubTracerCreateExpectations
				: global::Rocks.Expectations.ExpectationsV4
			{
				#pragma warning disable CS8618
				
				internal sealed class Handler0
					: global::Rocks.HandlerV4<global::System.Func<object?, bool>, bool>
				{
					public global::Rocks.Argument<object?> @obj { get; set; }
				}
				
				internal sealed class Handler1
					: global::Rocks.HandlerV4<global::System.Func<int>, int>
				{ }
				
				internal sealed class Handler2
					: global::Rocks.HandlerV4<global::System.Func<string?>, string?>
				{ }
				
				internal sealed class Handler4
					: global::Rocks.HandlerV4<global::System.Action<string, string, string, int, string, object[]>>
				{
					public global::Rocks.Argument<string> @eventCache { get; set; }
					public global::Rocks.Argument<string> @source { get; set; }
					public global::Rocks.Argument<string> @eventType { get; set; }
					public global::Rocks.Argument<int> @id { get; set; }
					public global::Rocks.Argument<string> @format { get; set; }
					public global::Rocks.Argument<object[]> @args { get; set; }
				}
				
				#pragma warning restore CS8618
				
				private readonly global::System.Collections.Generic.List<global::SubTracerCreateExpectations.Handler0> @handlers0 = new();
				private readonly global::System.Collections.Generic.List<global::SubTracerCreateExpectations.Handler1> @handlers1 = new();
				private readonly global::System.Collections.Generic.List<global::SubTracerCreateExpectations.Handler2> @handlers2 = new();
				private readonly global::System.Collections.Generic.List<global::SubTracerCreateExpectations.Handler4> @handlers4 = new();
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						failures.AddRange(this.Verify(handlers0));
						failures.AddRange(this.Verify(handlers1));
						failures.AddRange(this.Verify(handlers2));
						failures.AddRange(this.Verify(handlers4));
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class RockSubTracer
					: global::SubTracer
				{
					public RockSubTracer(global::SubTracerCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0, "bool Equals(object? @obj)")]
					public override bool Equals(object? @obj)
					{
						if (this.Expectations.handlers0.Count > 0)
						{
							foreach (var @handler in this.Expectations.handlers0)
							{
								if (@handler.@obj.IsValid(@obj!))
								{
									@handler.CallCount++;
									var @result = @handler.Callback is not null ?
										@handler.Callback(@obj!) : @handler.ReturnValue;
									return @result!;
								}
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool Equals(object? @obj)");
						}
						else
						{
							return base.Equals(obj: @obj!);
						}
					}
					
					[global::Rocks.MemberIdentifier(1, "int GetHashCode()")]
					public override int GetHashCode()
					{
						if (this.Expectations.handlers1.Count > 0)
						{
							var @handler = this.Expectations.handlers1[0];
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						else
						{
							return base.GetHashCode();
						}
					}
					
					[global::Rocks.MemberIdentifier(2, "string? ToString()")]
					public override string? ToString()
					{
						if (this.Expectations.handlers2.Count > 0)
						{
							var @handler = this.Expectations.handlers2[0];
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						else
						{
							return base.ToString();
						}
					}
					
					[global::Rocks.MemberIdentifier(4, "void TraceEvent(string @eventCache, string @source, string @eventType, int @id, string @format, params object[] @args)")]
					public override void TraceEvent(string @eventCache, string @source, string @eventType, int @id, string @format, params object[] @args)
					{
						if (this.Expectations.handlers4.Count > 0)
						{
							var @foundMatch = false;
							
							foreach (var @handler in this.Expectations.handlers4)
							{
								if (@handler.@eventCache.IsValid(@eventCache!) &&
									@handler.@source.IsValid(@source!) &&
									@handler.@eventType.IsValid(@eventType!) &&
									@handler.@id.IsValid(@id!) &&
									@handler.@format.IsValid(@format!) &&
									@handler.@args.IsValid(@args!))
								{
									@foundMatch = true;
									@handler.CallCount++;
									@handler.Callback?.Invoke(@eventCache!, @source!, @eventType!, @id!, @format!, @args!);
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void TraceEvent(string @eventCache, string @source, string @eventType, int @id, string @format, params object[] @args)");
							}
						}
						else
						{
							base.TraceEvent(eventCache: @eventCache!, source: @source!, eventType: @eventType!, id: @id!, format: @format!, args: @args!);
						}
					}
					
					private global::SubTracerCreateExpectations Expectations { get; }
				}
				
				internal sealed class SubTracerMethodExpectations
				{
					internal SubTracerMethodExpectations(global::SubTracerCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::Rocks.AdornmentsV4<global::SubTracerCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						
						var handler = new global::SubTracerCreateExpectations.Handler0
						{
							@obj = @obj,
						};
						
						this.Expectations.handlers0.Add(handler);
						return new(handler);
					}
					
					internal new global::Rocks.AdornmentsV4<global::SubTracerCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
					{
						var handler = new global::SubTracerCreateExpectations.Handler1();
						this.Expectations.handlers1.Add(handler);
						return new(handler);
					}
					
					internal new global::Rocks.AdornmentsV4<global::SubTracerCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
					{
						var handler = new global::SubTracerCreateExpectations.Handler2();
						this.Expectations.handlers2.Add(handler);
						return new(handler);
					}
					
					internal global::Rocks.AdornmentsV4<global::SubTracerCreateExpectations.Handler4, global::System.Action<string, string, string, int, string, object[]>> TraceEvent(global::Rocks.Argument<string> @eventCache, global::Rocks.Argument<string> @source, global::Rocks.Argument<string> @eventType, global::Rocks.Argument<int> @id, global::Rocks.Argument<string> @format, global::Rocks.Argument<object[]> @args)
					{
						global::System.ArgumentNullException.ThrowIfNull(@eventCache);
						global::System.ArgumentNullException.ThrowIfNull(@source);
						global::System.ArgumentNullException.ThrowIfNull(@eventType);
						global::System.ArgumentNullException.ThrowIfNull(@id);
						global::System.ArgumentNullException.ThrowIfNull(@format);
						global::System.ArgumentNullException.ThrowIfNull(@args);
						
						var handler = new global::SubTracerCreateExpectations.Handler4
						{
							@eventCache = @eventCache,
							@source = @source,
							@eventType = @eventType,
							@id = @id,
							@format = @format,
							@args = @args,
						};
						
						this.Expectations.handlers4.Add(handler);
						return new(handler);
					}
					
					private global::SubTracerCreateExpectations Expectations { get; }
				}
				
				internal global::SubTracerCreateExpectations.SubTracerMethodExpectations Methods { get; }
				
				internal SubTracerCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::SubTracer Instance()
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockSubTracer(this);
						this.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
			}
			""";

		// The two diagnostic IDs are actually warnings, so they
		// can be ignored/suppressed. They show up because the override
		// continues the problem that the subtype introduces when it changes
		// the nullability annotations.
		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "SubTracer_Rock_Create.g.cs", generatedCode) },
			new[]
			{
				new DiagnosticResult("CS8610", DiagnosticSeverity.Error)
					.WithSpan(18, 23, 18, 33).WithArguments("args"),
				new DiagnosticResult("CS8765", DiagnosticSeverity.Error)
					.WithSpan(18, 23, 18, 33).WithArguments("eventCache"),
				new DiagnosticResult("CS8765", DiagnosticSeverity.Error)
					.WithSpan(18, 23, 18, 33).WithArguments("format"),
			}).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenPropertyInShimNeedsNullForgivingAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<IConventionSkipNavigation>]

			#nullable enable

			public interface IReadOnlyNavigationBase
			{
				IReadOnlyNavigationBase? Inverse { get; }
			}

			public interface IReadOnlySkipNavigation
				: IReadOnlyNavigationBase
			{
				new IReadOnlySkipNavigation Inverse { get; }

				IReadOnlyNavigationBase IReadOnlyNavigationBase.Inverse
				{
					get => Inverse;
				}
			}

			public interface IConventionSkipNavigation 
				: IReadOnlySkipNavigation
			{
				new IConventionSkipNavigation? Inverse
				{
					get => (IConventionSkipNavigation?)((IReadOnlySkipNavigation)this).Inverse;
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			internal sealed class IConventionSkipNavigationCreateExpectations
				: global::Rocks.Expectations.ExpectationsV4
			{
				internal sealed class Handler0
					: global::Rocks.HandlerV4<global::System.Func<global::IConventionSkipNavigation?>, global::IConventionSkipNavigation?>
				{ }
				
				internal sealed class Handler1
					: global::Rocks.HandlerV4<global::System.Func<global::IReadOnlySkipNavigation>, global::IReadOnlySkipNavigation>
				{ }
				
				internal sealed class Handler2
					: global::Rocks.HandlerV4<global::System.Func<global::IReadOnlyNavigationBase?>, global::IReadOnlyNavigationBase?>
				{ }
				
				private readonly global::System.Collections.Generic.List<global::IConventionSkipNavigationCreateExpectations.Handler0> @handlers0 = new();
				private readonly global::System.Collections.Generic.List<global::IConventionSkipNavigationCreateExpectations.Handler1> @handlers1 = new();
				private readonly global::System.Collections.Generic.List<global::IConventionSkipNavigationCreateExpectations.Handler2> @handlers2 = new();
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						failures.AddRange(this.Verify(handlers0));
						failures.AddRange(this.Verify(handlers1));
						failures.AddRange(this.Verify(handlers2));
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class RockIConventionSkipNavigation
					: global::IConventionSkipNavigation
				{
					private readonly global::IConventionSkipNavigation shimForIConventionSkipNavigation;
					public RockIConventionSkipNavigation(global::IConventionSkipNavigationCreateExpectations @expectations)
					{
						(this.Expectations, this.shimForIConventionSkipNavigation) = (@expectations, new ShimIConventionSkipNavigation396255620100734449241449954173443346123272207515(this));
					}
					
					[global::Rocks.MemberIdentifier(0, "get_Inverse()")]
					public global::IConventionSkipNavigation? Inverse
					{
						get
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								var @handler = this.Expectations.handlers0[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							else
							{
								return this.shimForIConventionSkipNavigation.Inverse;
							}
						}
					}
					[global::Rocks.MemberIdentifier(1, "global::IReadOnlySkipNavigation.get_Inverse()")]
					global::IReadOnlySkipNavigation global::IReadOnlySkipNavigation.Inverse
					{
						get
						{
							if (this.Expectations.handlers1.Count > 0)
							{
								var @handler = this.Expectations.handlers1[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::IReadOnlySkipNavigation.get_Inverse())");
						}
					}
					[global::Rocks.MemberIdentifier(2, "global::IReadOnlyNavigationBase.get_Inverse()")]
					global::IReadOnlyNavigationBase? global::IReadOnlyNavigationBase.Inverse
					{
						get
						{
							if (this.Expectations.handlers2.Count > 0)
							{
								var @handler = this.Expectations.handlers2[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::IReadOnlyNavigationBase.get_Inverse())");
						}
					}
					
					
					private sealed class ShimIConventionSkipNavigation396255620100734449241449954173443346123272207515
						: global::IConventionSkipNavigation
					{
						private readonly RockIConventionSkipNavigation mock;
						
						public ShimIConventionSkipNavigation396255620100734449241449954173443346123272207515(RockIConventionSkipNavigation @mock) =>
							this.mock = @mock;
						
						global::IReadOnlySkipNavigation global::IReadOnlySkipNavigation.Inverse
						{
							get => ((global::IConventionSkipNavigation)this.mock).Inverse!;
						}
						
						global::IReadOnlyNavigationBase? global::IReadOnlyNavigationBase.Inverse
						{
							get => ((global::IConventionSkipNavigation)this.mock).Inverse!;
						}
					}
					private global::IConventionSkipNavigationCreateExpectations Expectations { get; }
				}
				internal sealed class IConventionSkipNavigationPropertyExpectations
				{
					internal sealed class IConventionSkipNavigationPropertyGetterExpectations
					{
						internal IConventionSkipNavigationPropertyGetterExpectations(global::IConventionSkipNavigationCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.AdornmentsV4<global::IConventionSkipNavigationCreateExpectations.Handler0, global::System.Func<global::IConventionSkipNavigation?>, global::IConventionSkipNavigation?> Inverse()
						{
							var handler = new global::IConventionSkipNavigationCreateExpectations.Handler0();
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						private global::IConventionSkipNavigationCreateExpectations Expectations { get; }
					}
					
					
					internal IConventionSkipNavigationPropertyExpectations(global::IConventionSkipNavigationCreateExpectations expectations) =>
						(this.Getters) = (new(expectations));
					
					internal global::IConventionSkipNavigationCreateExpectations.IConventionSkipNavigationPropertyExpectations.IConventionSkipNavigationPropertyGetterExpectations Getters { get; }
				}
				internal sealed class IConventionSkipNavigationExplicitPropertyExpectationsForIReadOnlySkipNavigation
				{
					internal sealed class IConventionSkipNavigationExplicitPropertyGetterExpectationsForIReadOnlySkipNavigation
					{
						internal IConventionSkipNavigationExplicitPropertyGetterExpectationsForIReadOnlySkipNavigation(global::IConventionSkipNavigationCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.AdornmentsV4<global::IConventionSkipNavigationCreateExpectations.Handler1, global::System.Func<global::IReadOnlySkipNavigation>, global::IReadOnlySkipNavigation> Inverse()
						{
							var handler = new global::IConventionSkipNavigationCreateExpectations.Handler1();
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						private global::IConventionSkipNavigationCreateExpectations Expectations { get; }
					}
					
					internal IConventionSkipNavigationExplicitPropertyExpectationsForIReadOnlySkipNavigation(global::IConventionSkipNavigationCreateExpectations expectations) =>
						(this.Getters) = (new(expectations));
					
					internal global::IConventionSkipNavigationCreateExpectations.IConventionSkipNavigationExplicitPropertyExpectationsForIReadOnlySkipNavigation.IConventionSkipNavigationExplicitPropertyGetterExpectationsForIReadOnlySkipNavigation Getters { get; }
				}
				internal sealed class IConventionSkipNavigationExplicitPropertyExpectationsForIReadOnlyNavigationBase
				{
					internal sealed class IConventionSkipNavigationExplicitPropertyGetterExpectationsForIReadOnlyNavigationBase
					{
						internal IConventionSkipNavigationExplicitPropertyGetterExpectationsForIReadOnlyNavigationBase(global::IConventionSkipNavigationCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.AdornmentsV4<global::IConventionSkipNavigationCreateExpectations.Handler2, global::System.Func<global::IReadOnlyNavigationBase?>, global::IReadOnlyNavigationBase?> Inverse()
						{
							var handler = new global::IConventionSkipNavigationCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						private global::IConventionSkipNavigationCreateExpectations Expectations { get; }
					}
					
					internal IConventionSkipNavigationExplicitPropertyExpectationsForIReadOnlyNavigationBase(global::IConventionSkipNavigationCreateExpectations expectations) =>
						(this.Getters) = (new(expectations));
					
					internal global::IConventionSkipNavigationCreateExpectations.IConventionSkipNavigationExplicitPropertyExpectationsForIReadOnlyNavigationBase.IConventionSkipNavigationExplicitPropertyGetterExpectationsForIReadOnlyNavigationBase Getters { get; }
				}
				
				internal global::IConventionSkipNavigationCreateExpectations.IConventionSkipNavigationPropertyExpectations Properties { get; }
				internal global::IConventionSkipNavigationCreateExpectations.IConventionSkipNavigationExplicitPropertyExpectationsForIReadOnlySkipNavigation ExplicitPropertiesForIReadOnlySkipNavigation { get; }
				internal global::IConventionSkipNavigationCreateExpectations.IConventionSkipNavigationExplicitPropertyExpectationsForIReadOnlyNavigationBase ExplicitPropertiesForIReadOnlyNavigationBase { get; }
				
				internal IConventionSkipNavigationCreateExpectations() =>
					(this.Properties, this.ExplicitPropertiesForIReadOnlySkipNavigation, this.ExplicitPropertiesForIReadOnlyNavigationBase) = (new(this), new(this), new(this));
				
				internal global::IConventionSkipNavigation Instance()
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockIConventionSkipNavigation(this);
						this.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "IConventionSkipNavigation_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenTargetIsInterfaceAndMethodIsConstrainedByTypeParameterThatIsAssignedAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<IDestination<object>>]

			public interface IDestination<TDestination>
			{
				void As<T>() where T : TDestination;
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			internal sealed class IDestinationOfobjectCreateExpectations
				: global::Rocks.Expectations.ExpectationsV4
			{
				internal sealed class Handler0<T>
					: global::Rocks.HandlerV4<global::System.Action>
					where T : notnull
				{ }
				
				private readonly global::System.Collections.Generic.List<global::Rocks.HandlerV4> @handlers0 = new();
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						failures.AddRange(this.Verify(handlers0));
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class RockIDestinationOfobject
					: global::IDestination<object>
				{
					public RockIDestinationOfobject(global::IDestinationOfobjectCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0, "void As<T>()")]
					public void As<T>()
						where T : notnull
					{
						if (this.Expectations.handlers0.Count > 0)
						{
							var @genericHandler = this.Expectations.handlers0[0];
							if (@genericHandler is global::IDestinationOfobjectCreateExpectations.Handler0<T> @handler)
							{
								@handler.CallCount++;
								@handler.Callback?.Invoke();
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("The provided handler does not match for void As<T>()");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void As<T>()");
						}
					}
					
					private global::IDestinationOfobjectCreateExpectations Expectations { get; }
				}
				
				internal sealed class IDestinationOfobjectMethodExpectations
				{
					internal IDestinationOfobjectMethodExpectations(global::IDestinationOfobjectCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::Rocks.AdornmentsV4<global::IDestinationOfobjectCreateExpectations.Handler0<T>, global::System.Action> As<T>() where T : notnull
					{
						var handler = new global::IDestinationOfobjectCreateExpectations.Handler0<T>();
						this.Expectations.handlers0.Add(handler);
						return new(handler);
					}
					
					private global::IDestinationOfobjectCreateExpectations Expectations { get; }
				}
				
				internal global::IDestinationOfobjectCreateExpectations.IDestinationOfobjectMethodExpectations Methods { get; }
				
				internal IDestinationOfobjectCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IDestination<object> Instance()
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockIDestinationOfobject(this);
						this.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "IDestinationobject_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}
}