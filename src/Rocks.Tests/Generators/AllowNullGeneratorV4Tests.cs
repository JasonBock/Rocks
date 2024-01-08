﻿using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class AllowNullGeneratorV4Tests
{
	[Test]
	public static async Task GenerateAbstractCreateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			[assembly: RockCreate<MockTests.IAllow>]

			namespace MockTests
			{
				public interface IAllow
				{
					 [AllowNull]
					 string NewLine { get; set; }
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
			
			namespace MockTests
			{
				internal sealed class IAllowCreateExpectations
					: global::Rocks.Expectations.ExpectationsV4
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.HandlerV4<global::System.Func<string>, string>
					{ }
					
					internal sealed class Handler1
						: global::Rocks.HandlerV4<global::System.Action<string>>
					{
						public global::Rocks.Argument<string> @value { get; set; }
					}
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.IAllowCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.IAllowCreateExpectations.Handler1> @handlers1 = new();
					
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
					
					private sealed class RockIAllow
						: global::MockTests.IAllow
					{
						public RockIAllow(global::MockTests.IAllowCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]
						[global::Rocks.MemberIdentifier(0, "get_NewLine()")]
						[global::Rocks.MemberIdentifier(1, "set_NewLine(value)")]
						public string NewLine
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
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_NewLine())");
							}
							set
							{
								if (this.Expectations.handlers1.Count > 0)
								{
									var @foundMatch = false;
									foreach (var @handler in this.Expectations.handlers1)
									{
										if (@handler.value.IsValid(value!))
										{
											@handler.CallCount++;
											@foundMatch = true;
											@handler.Callback?.Invoke(value!);
											
											if (!@foundMatch)
											{
												throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_NewLine(value)");
											}
											
											break;
										}
									}
								}
								else
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for set_NewLine(value)");
								}
							}
						}
						
						private global::MockTests.IAllowCreateExpectations Expectations { get; }
					}
					internal sealed class IAllowPropertyExpectations
					{
						internal sealed class IAllowPropertyGetterExpectations
						{
							internal IAllowPropertyGetterExpectations(global::MockTests.IAllowCreateExpectations expectations) =>
								this.Expectations = expectations;
							
							internal global::Rocks.AdornmentsV4<global::MockTests.IAllowCreateExpectations.Handler0, global::System.Func<string>, string> NewLine()
							{
								var handler = new global::MockTests.IAllowCreateExpectations.Handler0();
								this.Expectations.handlers0.Add(handler);
								return new(handler);
							}
							private global::MockTests.IAllowCreateExpectations Expectations { get; }
						}
						
						internal sealed class IAllowPropertySetterExpectations
						{
							internal IAllowPropertySetterExpectations(global::MockTests.IAllowCreateExpectations expectations) =>
								this.Expectations = expectations;
							
							internal global::Rocks.AdornmentsV4<global::MockTests.IAllowCreateExpectations.Handler1, global::System.Action<string>> NewLine(global::Rocks.Argument<string> @value)
							{
								var handler = new global::MockTests.IAllowCreateExpectations.Handler1
								{
									value = @value,
								};
							
								this.Expectations.handlers1.Add(handler);
								return new(handler);
							}
							private global::MockTests.IAllowCreateExpectations Expectations { get; }
						}
						
						internal IAllowPropertyExpectations(global::MockTests.IAllowCreateExpectations expectations) =>
							(this.Getters, this.Setters) = (new(expectations), new(expectations));
						
						internal global::MockTests.IAllowCreateExpectations.IAllowPropertyExpectations.IAllowPropertyGetterExpectations Getters { get; }
						internal global::MockTests.IAllowCreateExpectations.IAllowPropertyExpectations.IAllowPropertySetterExpectations Setters { get; }
					}
					
					internal global::MockTests.IAllowCreateExpectations.IAllowPropertyExpectations Properties { get; }
					
					internal IAllowCreateExpectations() =>
						(this.Properties) = (new(this));
					
					internal global::MockTests.IAllow Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockIAllow(this);
							this.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.IAllow_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateAbstractMakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			[assembly: RockMake<MockTests.IAllow>]

			namespace MockTests
			{
				public interface IAllow
				{
					 [AllowNull]
					 string NewLine { get; set; }
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class IAllowMakeExpectations
				{
					internal global::MockTests.IAllow Instance()
					{
						return new RockIAllow();
					}
					
					private sealed class RockIAllow
						: global::MockTests.IAllow
					{
						public RockIAllow()
						{
						}
						
						[global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]
						public string NewLine
						{
							get => default!;
							set { }
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.IAllow_Rock_Make.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateNonAbstractCreateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			namespace MockTests
			{
				public class Allow
				{
					 [AllowNull]
					 public virtual string NewLine { get; set; }
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<Allow>();
					}
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
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfAllowExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.Allow> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.Allow> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertyExpectations<global::MockTests.Allow> Properties(this global::Rocks.Expectations.Expectations<global::MockTests.Allow> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.Allow> Getters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.Allow> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertySetterExpectations<global::MockTests.Allow> Setters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.Allow> @self) =>
						new(@self);
					
					internal static global::MockTests.Allow Instance(this global::Rocks.Expectations.Expectations<global::MockTests.Allow> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockAllow(@self);
							@self.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockAllow
						: global::MockTests.Allow
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockAllow(global::Rocks.Expectations.Expectations<global::MockTests.Allow> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::Rocks.MemberIdentifier(0, "bool Equals(object? @obj)")]
						public override bool Equals(object? @obj)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((global::Rocks.Argument<object?>)@methodHandler.Expectations[0]).IsValid(@obj!))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											((global::System.Func<object?, bool>)@methodHandler.Method)(@obj!) :
											((global::Rocks.HandlerInformation<bool>)@methodHandler).ReturnValue;
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
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									((global::System.Func<int>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
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
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									((global::System.Func<string?>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<string?>)@methodHandler).ReturnValue;
								return @result!;
							}
							else
							{
								return base.ToString();
							}
						}
						
						[global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]
						[global::Rocks.MemberIdentifier(3, "get_NewLine()")]
						[global::Rocks.MemberIdentifier(4, "set_NewLine(value)")]
						public override string NewLine
						{
							get
							{
								if (this.handlers.TryGetValue(3, out var @methodHandlers))
								{
									var @methodHandler = @methodHandlers[0];
									@methodHandler.IncrementCallCount();
									var @result = @methodHandler.Method is not null ?
										((global::System.Func<string>)@methodHandler.Method)() :
										((global::Rocks.HandlerInformation<string>)@methodHandler).ReturnValue;
									return @result!;
								}
								else
								{
									return base.NewLine;
								}
							}
							set
							{
								if (this.handlers.TryGetValue(4, out var @methodHandlers))
								{
									var @foundMatch = false;
									foreach (var @methodHandler in @methodHandlers)
									{
										if (((global::Rocks.Argument<string>)@methodHandler.Expectations[0]).IsValid(value!))
										{
											@methodHandler.IncrementCallCount();
											@foundMatch = true;
											
											if (@methodHandler.Method is not null)
											{
												((global::System.Action<string>)@methodHandler.Method)(value!);
											}
											
											if (!@foundMatch)
											{
												throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_NewLine(value)");
											}
											
											break;
										}
									}
								}
								else
								{
									base.NewLine = value!;
								}
							}
						}
					}
				}
				
				internal static class MethodExpectationsOfAllowExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Allow> @self, global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						return new global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Allow> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Allow> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				
				internal static class PropertyGetterExpectationsOfAllowExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.Allow, global::System.Func<string>, string> NewLine(this global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.Allow> @self) =>
						new global::Rocks.PropertyAdornments<global::MockTests.Allow, global::System.Func<string>, string>(@self.Add<string>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				internal static class PropertySetterExpectationsOfAllowExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.Allow, global::System.Action<string>> NewLine(this global::Rocks.Expectations.PropertySetterExpectations<global::MockTests.Allow> @self, global::Rocks.Argument<string> @value) =>
						new global::Rocks.PropertyAdornments<global::MockTests.Allow, global::System.Action<string>>(@self.Add(4, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "MockTests.Allow_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateNonAbstractMakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			[assembly: RockMake<MockTests.Allow>]

			namespace MockTests
			{
				public class Allow
				{
					 [AllowNull]
					 public virtual string NewLine { get; set; }
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class AllowMakeExpectations
				{
					internal global::MockTests.Allow Instance()
					{
						return new RockAllow();
					}
					
					private sealed class RockAllow
						: global::MockTests.Allow
					{
						public RockAllow()
						{
						}
						
						public override bool Equals(object? @obj)
						{
							return default!;
						}
						public override int GetHashCode()
						{
							return default!;
						}
						public override string? ToString()
						{
							return default!;
						}
						[global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]
						public override string NewLine
						{
							get => default!;
							set { }
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.Allow_Rock_Make.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}
}