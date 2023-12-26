﻿using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class V4GeneratorTests
{
	[Test]
	public static async Task GenerateWithInterfaceAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: Rock<ITest>(BuildType.Create)]

			public sealed class Holder { }

			public interface ITest
			{
				void NoArgumentsNoReturn();
				void ArgumentsNoReturn(Holder holder, string value);

				int NoArgumentsReturn();
				int ArgumentsReturn(Holder holder, string value);
			
				Guid Data { get; set; }

				Holder this[long index] { get; }
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			internal sealed class ITestCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.HandlerV4<global::System.Action>
				{ }
				
				internal sealed class Handler1
					: global::Rocks.HandlerV4<global::System.Action<global::Holder, string>>
				{
					#pragma warning disable CS8618
					public global::Rocks.Argument<global::Holder> holder { get; set; }
					public global::Rocks.Argument<string> value { get; set; }
					#pragma warning restore CS8618
				}
				
				internal sealed class Handler2
					: global::Rocks.HandlerV4<global::System.Func<int>, int>
				{ }
				
				internal sealed class Handler3
					: global::Rocks.HandlerV4<global::System.Func<global::Holder, string, int>, int>
				{
					#pragma warning disable CS8618
					public global::Rocks.Argument<global::Holder> holder { get; set; }
					public global::Rocks.Argument<string> value { get; set; }
					#pragma warning restore CS8618
				}
				
				internal sealed class Handler4
					: global::Rocks.HandlerV4<global::System.Func<global::System.Guid>, global::System.Guid>
				{ }
				
				internal sealed class Handler5
					: global::Rocks.HandlerV4<global::System.Action<global::System.Guid>>
				{
					#pragma warning disable CS8618
					public global::Rocks.Argument<global::System.Guid> value { get; set; }
					#pragma warning restore CS8618
				}
				
				internal sealed class Handler6
					: global::Rocks.HandlerV4<global::System.Func<long, global::Holder>, global::Holder>
				{
					#pragma warning disable CS8618
					public global::Rocks.Argument<long> index { get; set; }
					#pragma warning restore CS8618
				}
				
				private readonly global::System.Collections.Generic.List<global::ITestCreateExpectations.Handler0> @handlers0 = new();
				private readonly global::System.Collections.Generic.List<global::ITestCreateExpectations.Handler1> @handlers1 = new();
				private readonly global::System.Collections.Generic.List<global::ITestCreateExpectations.Handler2> @handlers2 = new();
				private readonly global::System.Collections.Generic.List<global::ITestCreateExpectations.Handler3> @handlers3 = new();
				private readonly global::System.Collections.Generic.List<global::ITestCreateExpectations.Handler4> @handlers4 = new();
				private readonly global::System.Collections.Generic.List<global::ITestCreateExpectations.Handler5> @handlers5 = new();
				private readonly global::System.Collections.Generic.List<global::ITestCreateExpectations.Handler6> @handlers6 = new();
				
				internal global::ITestCreateExpectations.ITestMethodExpectations Methods { get; }
				internal global::ITestCreateExpectations.ITestPropertyExpectations Properties { get; }
				internal global::ITestCreateExpectations.ITestIndexerExpectations Indexers { get; }
				
				internal ITestCreateExpectations() =>
					(this.Methods, this.Properties, this.Indexers) = (new(this), new(this), new(this));
				
				internal static global::ITest Instance()
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockITest(this);
						this.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						failures.AddRange(this.Verify(handlers0));
						failures.AddRange(this.Verify(handlers1));
						failures.AddRange(this.Verify(handlers2));
						failures.AddRange(this.Verify(handlers3));
						failures.AddRange(this.Verify(handlers4));
						failures.AddRange(this.Verify(handlers5));
						failures.AddRange(this.Verify(handlers6));
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class RockITest
					: global::ITest
				{
					private readonly global::ITestCreateExpectations expectations;
					
					public RockITest(global::ITestCreateExpectations @expectations)
					{
						this.expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0, "void NoArgumentsNoReturn()")]
					public void NoArgumentsNoReturn()
					{
						if (this.expectations.handler0.Count > 0)
						{
							var @handler = this.expectations.handler0[0];
							@handler.CallCount++;
							
							if (@handler.Callback is not null)
							{
								@handler.Callback();
							}
							
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void NoArgumentsNoReturn()");
						}
					}
					
					[global::Rocks.MemberIdentifier(1, "void ArgumentsNoReturn(global::Holder @holder, string @value)")]
					public void ArgumentsNoReturn(global::Holder @holder, string @value)
					{
						if (this.expectations.handler1.Count > 0)
						{
							var @foundMatch = false;
							
							foreach (var @handler in this.expectations.handler1)
							{
								if (@handler.holder.IsValid(@holder!) &&
									@handler.value.IsValid(@value!))
								{
									@foundMatch = true;
									
									@handler.CallCount++;
									
									if (@handler.Callback is not null)
									{
										@handler.Callback(@holder!, @value!);
									}
									
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void ArgumentsNoReturn(global::Holder @holder, string @value)");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void ArgumentsNoReturn(global::Holder @holder, string @value)");
						}
					}
					
					[global::Rocks.MemberIdentifier(2, "int NoArgumentsReturn()")]
					public int NoArgumentsReturn()
					{
						if (this.expectations.handler2.Count > 0)
						{
							var @handler = this.expectations.handler2[0];
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() :
								@handler.ReturnValue;
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int NoArgumentsReturn()");
					}
					
					[global::Rocks.MemberIdentifier(3, "int ArgumentsReturn(global::Holder @holder, string @value)")]
					public int ArgumentsReturn(global::Holder @holder, string @value)
					{
						if (this.expectations.handler3.Count > 0)
						{
							foreach (var @handler in this.expectations.handler3)
							{
								if ((@handler.holder.IsValid(@holder!) &&
									@handler.value.IsValid(@value!))
								{
									@handler.CallCount++;
									var @result = @handler.Callback is not null ?
										@handler.Callback(@holder!, @value!) :
										@handler.ReturnValue;
									return @result!;
								}
							}
						}
						
						throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int ArgumentsReturn(global::Holder @holder, string @value)");
					}
					
					[global::Rocks.MemberIdentifier(4, "get_Data()")]
					[global::Rocks.MemberIdentifier(5, "set_Data(value)")]
					public global::System.Guid Data
					{
						get
						{
							if (this.expectations.handler4.Count > 0)
							{
								var @handler = this.expectations.handler4[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() :
									@handler.ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_Data())");
						}
						set
						{
							if (this.expectations.handler5)
							{
								var @foundMatch = false;
								foreach (var @handler in this.expectations.handler5)
								{
									if (@handler.value.IsValid(value!))
									{
										@handler.CallCount++;
										@foundMatch = true;
										
										if (@handler.Method is not null)
										{
											@methodHandler.Method(value!);
										}
										
										if (!@foundMatch)
										{
											throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_Data(value)");
										}
										
										break;
									}
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for set_Data(value)");
							}
						}
					}
					[global::Rocks.MemberIdentifier(6, "this[long @index]")]
					public global::Holder this[long @index]
					{
						get
						{
							if (this.handlers.TryGetValue(6, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((global::Rocks.Argument<long>)@methodHandler.Expectations[0]).IsValid(@index!))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											((global::System.Func<long, global::Holder>)@methodHandler.Method)(@index!) :
											((global::Rocks.HandlerInformation<global::Holder>)@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[long @index]");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[long @index])");
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockGenerator>(code,
			new[] { (typeof(RockGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}