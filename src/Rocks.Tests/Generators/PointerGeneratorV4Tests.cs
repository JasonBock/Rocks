﻿using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class PointerGeneratorV4Tests
{
	[Test]
	public static async Task CreateWithPointersAndGenericsAndUnmanagedConstraintAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<ISurface>]

			public interface ISurface
			{
				unsafe void Create<T>(T* allocator) where T : unmanaged;
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using ProjectionsForISurface;
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			namespace ProjectionsForISurface
			{
				internal unsafe delegate void CreateCallback_197220541663188455981388668427457375357930643021<T>(T* @allocator) where T : unmanaged;
			}
			
			internal sealed class ISurfaceCreateExpectations
				: global::Rocks.Expectations.ExpectationsV4
			{
				#pragma warning disable CS8618
				
				internal sealed class Handler0<T>
					: global::Rocks.HandlerV4<global::ProjectionsForISurface.CreateCallback_197220541663188455981388668427457375357930643021<T>>
					where T : unmanaged
				{
					public global::Rocks.PointerArgument<T> @allocator { get; set; }
				}
				
				#pragma warning restore CS8618
				
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
				
				private sealed class RockISurface
					: global::ISurface
				{
					public RockISurface(global::ISurfaceCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0, "void Create<T>(T* @allocator)")]
					public unsafe void Create<T>(T* @allocator)
						where T : unmanaged
					{
						if (this.Expectations.handlers0.Count > 0)
						{
							var @foundMatch = false;
							
							foreach (var @genericHandler in this.Expectations.handlers0)
							{
								if (@genericHandler is global::ISurfaceCreateExpectations.Handler0<T> @handler)
								{
									if (@handler.@allocator.IsValid(@allocator!))
									{
										@foundMatch = true;
										@handler.CallCount++;
										@handler.Callback?.Invoke(@allocator!);
										break;
									}
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for unsafe void Create<T>(T* @allocator)");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for unsafe void Create<T>(T* @allocator)");
						}
					}
					
					private global::ISurfaceCreateExpectations Expectations { get; }
				}
				
				internal sealed class ISurfaceMethodExpectations
				{
					internal ISurfaceMethodExpectations(global::ISurfaceCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::Rocks.AdornmentsV4<global::ISurfaceCreateExpectations.Handler0<T>, global::ProjectionsForISurface.CreateCallback_197220541663188455981388668427457375357930643021<T>> Create<T>(global::Rocks.PointerArgument<T> @allocator) where T : unmanaged
					{
						global::System.ArgumentNullException.ThrowIfNull(@allocator);
						
						var handler = new global::ISurfaceCreateExpectations.Handler0<T>
						{
							@allocator = @allocator,
						};
						
						this.Expectations.handlers0.Add(handler);
						return new(handler);
					}
					
					private global::ISurfaceCreateExpectations Expectations { get; }
				}
				
				internal global::ISurfaceCreateExpectations.ISurfaceMethodExpectations Methods { get; }
				
				internal ISurfaceCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::ISurface Instance()
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockISurface(this);
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
			new[] { (typeof(RockAttributeGenerator), "ISurface_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithPointerParameterAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockCreate<MockTests.IHavePointers>]

			#nullable enable

			namespace MockTests
			{
				public unsafe interface IHavePointers
				{
					void PointerParameter(int* value);
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using MockTests.ProjectionsForIHavePointers;
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			namespace MockTests
			{
				namespace ProjectionsForIHavePointers
				{
					internal unsafe delegate void PointerParameterCallback_448273544004536059019999557806138154926952273337(int* @value);
				}
				
				internal sealed class IHavePointersCreateExpectations
					: global::Rocks.Expectations.ExpectationsV4
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.HandlerV4<global::MockTests.ProjectionsForIHavePointers.PointerParameterCallback_448273544004536059019999557806138154926952273337>
					{
						public global::Rocks.PointerArgument<int> @value { get; set; }
					}
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.IHavePointersCreateExpectations.Handler0> @handlers0 = new();
					
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
					
					private sealed class RockIHavePointers
						: global::MockTests.IHavePointers
					{
						public RockIHavePointers(global::MockTests.IHavePointersCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "void PointerParameter(int* @value)")]
						public unsafe void PointerParameter(int* @value)
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								var @foundMatch = false;
								
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@value.IsValid(@value!))
									{
										@foundMatch = true;
										@handler.CallCount++;
										@handler.Callback?.Invoke(@value!);
										break;
									}
								}
								
								if (!@foundMatch)
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers match for unsafe void PointerParameter(int* @value)");
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for unsafe void PointerParameter(int* @value)");
							}
						}
						
						private global::MockTests.IHavePointersCreateExpectations Expectations { get; }
					}
					
					internal sealed class IHavePointersMethodExpectations
					{
						internal IHavePointersMethodExpectations(global::MockTests.IHavePointersCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.AdornmentsV4<global::MockTests.IHavePointersCreateExpectations.Handler0, global::MockTests.ProjectionsForIHavePointers.PointerParameterCallback_448273544004536059019999557806138154926952273337> PointerParameter(global::Rocks.PointerArgument<int> @value)
						{
							global::System.ArgumentNullException.ThrowIfNull(@value);
							
							var handler = new global::MockTests.IHavePointersCreateExpectations.Handler0
							{
								@value = @value,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.IHavePointersCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IHavePointersCreateExpectations.IHavePointersMethodExpectations Methods { get; }
					
					internal IHavePointersCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IHavePointers Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockIHavePointers(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.IHavePointers_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithPointerReturnAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockCreate<MockTests.IHavePointers>]

			#nullable enable

			namespace MockTests
			{
				public unsafe interface IHavePointers
				{
					int* PointerReturn();
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using MockTests.ProjectionsForIHavePointers;
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			namespace MockTests
			{
				namespace ProjectionsForIHavePointers
				{
					internal unsafe delegate int* PointerReturnCallback_355763855309704752655277092464969889148092134081();
				}
				
				internal sealed class IHavePointersCreateExpectations
					: global::Rocks.Expectations.ExpectationsV4
				{
					internal sealed class Handler0
						: global::Rocks.PointerHandlerV4<global::MockTests.ProjectionsForIHavePointers.PointerReturnCallback_355763855309704752655277092464969889148092134081, int>
					{ }
					
					private readonly global::System.Collections.Generic.List<global::MockTests.IHavePointersCreateExpectations.Handler0> @handlers0 = new();
					
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
					
					private sealed class RockIHavePointers
						: global::MockTests.IHavePointers
					{
						public RockIHavePointers(global::MockTests.IHavePointersCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "int* PointerReturn()")]
						public unsafe int* PointerReturn()
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								var @handler = this.Expectations.handlers0[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for unsafe int* PointerReturn()");
						}
						
						private global::MockTests.IHavePointersCreateExpectations Expectations { get; }
					}
					
					internal sealed class IHavePointersMethodExpectations
					{
						internal IHavePointersMethodExpectations(global::MockTests.IHavePointersCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.PointerAdornmentsV4<global::MockTests.IHavePointersCreateExpectations.Handler0, global::MockTests.ProjectionsForIHavePointers.PointerReturnCallback_355763855309704752655277092464969889148092134081, int> PointerReturn()
						{
							var handler = new global::MockTests.IHavePointersCreateExpectations.Handler0();
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.IHavePointersCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IHavePointersCreateExpectations.IHavePointersMethodExpectations Methods { get; }
					
					internal IHavePointersCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IHavePointers Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockIHavePointers(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.IHavePointers_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithFunctionPointerParameterAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockCreate<MockTests.IHavePointers>]

			#nullable enable

			namespace MockTests
			{
				public unsafe interface IHavePointers
				{
					void FunctionPointerParameter(delegate*<int, void> value);
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using MockTests.ProjectionsForIHavePointers;
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			namespace MockTests
			{
				namespace ProjectionsForIHavePointers
				{
					internal unsafe delegate void FunctionPointerParameterCallback_120228475323677978467545270358451296795201638975(delegate*<int, void> @value);
					internal unsafe delegate bool ArgEvaluationFordelegatePointerOfint__void(delegate*<int, void> @value);
					
					internal unsafe sealed class ArgFordelegatePointerOfint__void
						: global::Rocks.Argument
					{
						private readonly global::MockTests.ProjectionsForIHavePointers.ArgEvaluationFordelegatePointerOfint__void? evaluation;
						private readonly global::Rocks.ValidationState validation;
						
						internal ArgFordelegatePointerOfint__void() => this.validation = global::Rocks.ValidationState.None;
						
						internal ArgFordelegatePointerOfint__void(global::MockTests.ProjectionsForIHavePointers.ArgEvaluationFordelegatePointerOfint__void @evaluation)
						{
							this.evaluation = @evaluation;
							this.validation = global::Rocks.ValidationState.Evaluation;
						}
						
						public bool IsValid(delegate*<int, void> @value) =>
							this.validation switch
							{
								global::Rocks.ValidationState.None => true,
								global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
								_ => throw new global::System.NotSupportedException("Invalid validation state."),
							};
					}
				}
				
				internal sealed class IHavePointersCreateExpectations
					: global::Rocks.Expectations.ExpectationsV4
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.HandlerV4<global::MockTests.ProjectionsForIHavePointers.FunctionPointerParameterCallback_120228475323677978467545270358451296795201638975>
					{
						public global::MockTests.ProjectionsForIHavePointers.ArgFordelegatePointerOfint__void @value { get; set; }
					}
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.IHavePointersCreateExpectations.Handler0> @handlers0 = new();
					
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
					
					private sealed class RockIHavePointers
						: global::MockTests.IHavePointers
					{
						public RockIHavePointers(global::MockTests.IHavePointersCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "void FunctionPointerParameter(delegate*<int, void> @value)")]
						public unsafe void FunctionPointerParameter(delegate*<int, void> @value)
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								var @foundMatch = false;
								
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@value.IsValid(@value!))
									{
										@foundMatch = true;
										@handler.CallCount++;
										@handler.Callback?.Invoke(@value!);
										break;
									}
								}
								
								if (!@foundMatch)
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers match for unsafe void FunctionPointerParameter(delegate*<int, void> @value)");
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for unsafe void FunctionPointerParameter(delegate*<int, void> @value)");
							}
						}
						
						private global::MockTests.IHavePointersCreateExpectations Expectations { get; }
					}
					
					internal sealed class IHavePointersMethodExpectations
					{
						internal IHavePointersMethodExpectations(global::MockTests.IHavePointersCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.AdornmentsV4<global::MockTests.IHavePointersCreateExpectations.Handler0, global::MockTests.ProjectionsForIHavePointers.FunctionPointerParameterCallback_120228475323677978467545270358451296795201638975> FunctionPointerParameter(global::MockTests.ProjectionsForIHavePointers.ArgFordelegatePointerOfint__void @value)
						{
							global::System.ArgumentNullException.ThrowIfNull(@value);
							
							var handler = new global::MockTests.IHavePointersCreateExpectations.Handler0
							{
								@value = @value,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.IHavePointersCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IHavePointersCreateExpectations.IHavePointersMethodExpectations Methods { get; }
					
					internal IHavePointersCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IHavePointers Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockIHavePointers(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.IHavePointers_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithFunctionPointerReturnAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockCreate<MockTests.IHavePointers>]

			#nullable enable

			namespace MockTests
			{
				public unsafe interface IHavePointers
				{
					delegate*<int, void> FunctionPointerReturn();
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using MockTests.ProjectionsForIHavePointers;
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			namespace MockTests
			{
				namespace ProjectionsForIHavePointers
				{
					internal unsafe delegate delegate*<int, void> FunctionPointerReturnCallback_251504302799091288200726897881163423629002400244();
					internal unsafe class HandlerFordelegatePointerOfint__void
						: global::Rocks.HandlerV4<global::MockTests.ProjectionsForIHavePointers.FunctionPointerReturnCallback_251504302799091288200726897881163423629002400244>
					{
						internal delegate*<int, void> ReturnValue { get; set; }
					}
					internal unsafe sealed class AdornmentsFordelegatePointerOfint__void
						: global::Rocks.AdornmentsV4<HandlerFordelegatePointerOfint__void, global::MockTests.ProjectionsForIHavePointers.FunctionPointerReturnCallback_251504302799091288200726897881163423629002400244>
					{
						internal AdornmentsFordelegatePointerOfint__void(HandlerFordelegatePointerOfint__void handler) 
							: base(handler) 
						{ }
					
						internal AdornmentsFordelegatePointerOfint__void ReturnValue(delegate*<int, void> returnValue)
						{
							this.handler.ReturnValue = returnValue;
							return this;
						}
					}
				}
				
				internal sealed class IHavePointersCreateExpectations
					: global::Rocks.Expectations.ExpectationsV4
				{
					internal sealed class Handler0
						: global::MockTests.ProjectionsForIHavePointers.HandlerFordelegatePointerOfint__void
					{ }
					
					private readonly global::System.Collections.Generic.List<global::MockTests.IHavePointersCreateExpectations.Handler0> @handlers0 = new();
					
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
					
					private sealed class RockIHavePointers
						: global::MockTests.IHavePointers
					{
						public RockIHavePointers(global::MockTests.IHavePointersCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "delegate*<int, void> FunctionPointerReturn()")]
						public unsafe delegate*<int, void> FunctionPointerReturn()
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								var @handler = this.Expectations.handlers0[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for unsafe delegate*<int, void> FunctionPointerReturn()");
						}
						
						private global::MockTests.IHavePointersCreateExpectations Expectations { get; }
					}
					
					internal sealed class IHavePointersMethodExpectations
					{
						internal IHavePointersMethodExpectations(global::MockTests.IHavePointersCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.ProjectionsForIHavePointers.AdornmentsFordelegatePointerOfint__void FunctionPointerReturn()
						{
							var handler = new global::MockTests.IHavePointersCreateExpectations.Handler0();
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.IHavePointersCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IHavePointersCreateExpectations.IHavePointersMethodExpectations Methods { get; }
					
					internal IHavePointersCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IHavePointers Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockIHavePointers(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.IHavePointers_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithPointerPropertyAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockCreate<MockTests.IHavePointers>]

			#nullable enable

			namespace MockTests
			{
				public unsafe interface IHavePointers
				{
					int* Data { get; set; }
				}
			}
			""";

		var generatedCode = "";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.IHavePointers_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithFunctionPointerPropertyAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockCreate<MockTests.IHavePointers>]

			#nullable enable

			namespace MockTests
			{
				public unsafe interface IHavePointers
				{
					delegate*<int, void> Data { get; set; }
				}
			}
			""";

		var generatedCode = "";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.IHavePointers_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithPointerIndexerAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockCreate<MockTests.IHavePointers>]

			#nullable enable

			namespace MockTests
			{
				public unsafe interface IHavePointers
				{
					string this[int* index] { get; set; }
				}
			}
			""";

		var generatedCode = "";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.IHavePointers_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithFunctionPointerIndexerAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockCreate<MockTests.IHavePointers>]

			#nullable enable

			namespace MockTests
			{
				public unsafe interface IHavePointers
				{
					string this[delegate*<int, void> index] { get; set; }
				}
			}
			""";

		var generatedCode = "";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.IHavePointers_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}