using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ProjectedTypesGeneratorTests
{
	[Test]
	public static async Task CreateWithRefLikeTypeWithOpenGenericsAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			public interface IUseSpanWithOpenGeneric
			{
				 void From<TSourcePixel>(
					  ReadOnlySpan<TSourcePixel> sourcePixels)
					  where TSourcePixel : unmanaged;
			}

			public static class Test
			{
				 public static void Go()
				 {
					  var expectations = Rock.Create<IUseSpanWithOpenGeneric>();
				 }
			}
			""";

		var generatedCode =
			"""
			using ProjectionsForIUseSpanWithOpenGeneric;
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			namespace ProjectionsForIUseSpanWithOpenGeneric
			{
				internal delegate void FromCallback_92766876440491954433706353246551017062742057391<TSourcePixel>(global::System.ReadOnlySpan<TSourcePixel> @sourcePixels) where TSourcePixel : unmanaged;
				internal delegate bool ArgEvaluationForReadOnlySpan<TSourcePixel>(global::System.ReadOnlySpan<TSourcePixel> @value);
				
				internal sealed class ArgForReadOnlySpan<TSourcePixel>
					: global::Rocks.Argument
				{
					private readonly global::ProjectionsForIUseSpanWithOpenGeneric.ArgEvaluationForReadOnlySpan<TSourcePixel>? evaluation;
					private readonly global::Rocks.ValidationState validation;
					
					internal ArgForReadOnlySpan() => this.validation = global::Rocks.ValidationState.None;
					
					internal ArgForReadOnlySpan(global::ProjectionsForIUseSpanWithOpenGeneric.ArgEvaluationForReadOnlySpan<TSourcePixel> @evaluation)
					{
						this.evaluation = @evaluation;
						this.validation = global::Rocks.ValidationState.Evaluation;
					}
					
					public bool IsValid(global::System.ReadOnlySpan<TSourcePixel> @value) =>
						this.validation switch
						{
							global::Rocks.ValidationState.None => true,
							global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
							_ => throw new global::System.NotSupportedException("Invalid validation state."),
						};
				}
			}
			
			internal static class CreateExpectationsOfIUseSpanWithOpenGenericExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::IUseSpanWithOpenGeneric> Methods(this global::Rocks.Expectations.Expectations<global::IUseSpanWithOpenGeneric> @self) =>
					new(@self);
				
				internal static global::IUseSpanWithOpenGeneric Instance(this global::Rocks.Expectations.Expectations<global::IUseSpanWithOpenGeneric> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						var @mock = new RockIUseSpanWithOpenGeneric(@self);
						@self.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIUseSpanWithOpenGeneric
					: global::IUseSpanWithOpenGeneric
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIUseSpanWithOpenGeneric(global::Rocks.Expectations.Expectations<global::IUseSpanWithOpenGeneric> @expectations)
					{
						this.handlers = @expectations.Handlers;
					}
					
					[global::Rocks.MemberIdentifier(0, "void From<TSourcePixel>(global::System.ReadOnlySpan<TSourcePixel> @sourcePixels)")]
					public void From<TSourcePixel>(global::System.ReadOnlySpan<TSourcePixel> @sourcePixels)
						where TSourcePixel : unmanaged
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
						{
							var @foundMatch = false;
							
							foreach (var @methodHandler in @methodHandlers)
							{
								if (global::System.Runtime.CompilerServices.Unsafe.As<global::ProjectionsForIUseSpanWithOpenGeneric.ArgForReadOnlySpan<TSourcePixel>>(@methodHandler.Expectations[0]).IsValid(@sourcePixels))
								{
									@foundMatch = true;
									
									@methodHandler.IncrementCallCount();
									if (@methodHandler.Method is not null && @methodHandler.Method is global::ProjectionsForIUseSpanWithOpenGeneric.FromCallback_92766876440491954433706353246551017062742057391<TSourcePixel> @method)
									{
										@method(@sourcePixels);
									}
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void From<TSourcePixel>(global::System.ReadOnlySpan<TSourcePixel> @sourcePixels)");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void From<TSourcePixel>(global::System.ReadOnlySpan<TSourcePixel> @sourcePixels)");
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfIUseSpanWithOpenGenericExtensions
			{
				internal static global::Rocks.MethodAdornments<global::IUseSpanWithOpenGeneric, global::ProjectionsForIUseSpanWithOpenGeneric.FromCallback_92766876440491954433706353246551017062742057391<TSourcePixel>> From<TSourcePixel>(this global::Rocks.Expectations.MethodExpectations<global::IUseSpanWithOpenGeneric> @self, global::ProjectionsForIUseSpanWithOpenGeneric.ArgForReadOnlySpan<TSourcePixel> @sourcePixels) where TSourcePixel : unmanaged
				{
					global::System.ArgumentNullException.ThrowIfNull(@sourcePixels);
					return new global::Rocks.MethodAdornments<global::IUseSpanWithOpenGeneric, global::ProjectionsForIUseSpanWithOpenGeneric.FromCallback_92766876440491954433706353246551017062742057391<TSourcePixel>>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @sourcePixels }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGeneratorV3>(code,
			new[] { (typeof(RockCreateGeneratorV3), "IUseSpanWithOpenGeneric_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithPointersAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			#nullable enable
			namespace MockTests
			{
				public unsafe interface IHavePointers
				{
					void DelegatePointerParameter(delegate*<int, void> value);
					delegate*<int, void> DelegatePointerReturn();
					void PointerParameter(int* value);
					int* PointerReturn();
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IHavePointers>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			using MockTests.ProjectionsForIHavePointers;
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			namespace MockTests
			{
				namespace ProjectionsForIHavePointers
				{
					internal unsafe delegate void DelegatePointerParameterCallback_67936425010368164897427961753475545964149702154(delegate*<int, void> @value);
					internal unsafe delegate delegate*<int, void> DelegatePointerReturnCallback_348091753837477554967687149303310703900832221476();
					internal unsafe delegate void PointerParameterCallback_448273544004536059019999557806138154926952273337(int* @value);
					internal unsafe delegate int* PointerReturnCallback_355763855309704752655277092464969889148092134081();
					internal unsafe delegate bool ArgumentEvaluationFordelegatePointerOfint__void(delegate*<int, void> @value);
					
					internal unsafe sealed class ArgumentFordelegatePointerOfint__void
						: global::Rocks.Argument
					{
						private readonly global::MockTests.ProjectionsForIHavePointers.ArgumentEvaluationFordelegatePointerOfint__void? evaluation;
						private readonly delegate*<int, void> value;
						private readonly global::Rocks.ValidationState validation;
						
						internal ArgumentFordelegatePointerOfint__void() => this.validation = global::Rocks.ValidationState.None;
						
						internal ArgumentFordelegatePointerOfint__void(delegate*<int, void> @value)
						{
							this.value = @value;
							this.validation = global::Rocks.ValidationState.Value;
						}
						
						internal ArgumentFordelegatePointerOfint__void(global::MockTests.ProjectionsForIHavePointers.ArgumentEvaluationFordelegatePointerOfint__void @evaluation)
						{
							this.evaluation = @evaluation;
							this.validation = global::Rocks.ValidationState.Evaluation;
						}
						
						public bool IsValid(delegate*<int, void> @value) =>
							this.validation switch
							{
								global::Rocks.ValidationState.None => true,
								#pragma warning disable CS8909
								global::Rocks.ValidationState.Value => @value == this.value,
								#pragma warning restore CS8909
								global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
								global::Rocks.ValidationState.DefaultValue => throw new global::System.NotSupportedException("Cannot validate an argument value in the ValidationState.DefaultValue state."),
								_ => throw new global::System.ComponentModel.InvalidEnumArgumentException($"Invalid value for validation: {{this.validation}}")
							};
					}
					internal unsafe delegate bool ArgumentEvaluationForintPointer(int* @value);
					
					internal unsafe sealed class ArgumentForintPointer
						: global::Rocks.Argument
					{
						private readonly global::MockTests.ProjectionsForIHavePointers.ArgumentEvaluationForintPointer? evaluation;
						private readonly int* value;
						private readonly global::Rocks.ValidationState validation;
						
						internal ArgumentForintPointer() => this.validation = global::Rocks.ValidationState.None;
						
						internal ArgumentForintPointer(int* @value)
						{
							this.value = @value;
							this.validation = global::Rocks.ValidationState.Value;
						}
						
						internal ArgumentForintPointer(global::MockTests.ProjectionsForIHavePointers.ArgumentEvaluationForintPointer @evaluation)
						{
							this.evaluation = @evaluation;
							this.validation = global::Rocks.ValidationState.Evaluation;
						}
						
						public static implicit operator ArgumentForintPointer(int* @value) => new(@value);
						
						public bool IsValid(int* @value) =>
							this.validation switch
							{
								global::Rocks.ValidationState.None => true,
								global::Rocks.ValidationState.Value => @value == this.value,
								global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
								global::Rocks.ValidationState.DefaultValue => throw new global::System.NotSupportedException("Cannot validate an argument value in the ValidationState.DefaultValue state."),
								_ => throw new global::System.ComponentModel.InvalidEnumArgumentException($"Invalid value for validation: {{this.validation}}")
							};
					}
					internal unsafe sealed class HandlerInformationFordelegatePointerOfint__void
						: global::Rocks.HandlerInformation
					{
						internal HandlerInformationFordelegatePointerOfint__void(global::System.Collections.Immutable.ImmutableArray<global::Rocks.Argument> @expectations)
							: base(null, @expectations) => this.ReturnValue = default;
						
						internal HandlerInformationFordelegatePointerOfint__void(global::System.Delegate? @method, global::System.Collections.Immutable.ImmutableArray<global::Rocks.Argument> @expectations)
							: base(@method, @expectations) => this.ReturnValue = default;
						
						internal delegate*<int, void> ReturnValue { get; set; }
					}
					internal unsafe sealed class HandlerInformationForintPointer
						: global::Rocks.HandlerInformation
					{
						internal HandlerInformationForintPointer(global::System.Collections.Immutable.ImmutableArray<global::Rocks.Argument> @expectations)
							: base(null, @expectations) => this.ReturnValue = default;
						
						internal HandlerInformationForintPointer(global::System.Delegate? @method, global::System.Collections.Immutable.ImmutableArray<global::Rocks.Argument> @expectations)
							: base(@method, @expectations) => this.ReturnValue = default;
						
						internal int* ReturnValue { get; set; }
					}
					internal static class ExpectationsExtensions
					{
						internal static HandlerInformationFordelegatePointerOfint__void AddFordelegatePointerOfint__void(this global::Rocks.Expectations.Expectations<global::MockTests.IHavePointers> @self, int @memberIdentifier, global::System.Collections.Generic.List<global::Rocks.Argument> @arguments)
						{
							var @information = new HandlerInformationFordelegatePointerOfint__void(@arguments.ToImmutableArray());
							@self.Handlers.AddOrUpdate(@memberIdentifier,
								() => new global::System.Collections.Generic.List<global::Rocks.HandlerInformation>(1) { @information }, _ => _.Add(@information));
							return @information;
						}
						internal static HandlerInformationForintPointer AddForintPointer(this global::Rocks.Expectations.Expectations<global::MockTests.IHavePointers> @self, int @memberIdentifier, global::System.Collections.Generic.List<global::Rocks.Argument> @arguments)
						{
							var @information = new HandlerInformationForintPointer(@arguments.ToImmutableArray());
							@self.Handlers.AddOrUpdate(@memberIdentifier,
								() => new global::System.Collections.Generic.List<global::Rocks.HandlerInformation>(1) { @information }, _ => _.Add(@information));
							return @information;
						}
					}
					internal sealed class MethodAdornmentsFordelegatePointerOfint__void<T, TCallback>
						: global::Rocks.IAdornments<HandlerInformationFordelegatePointerOfint__void>
						where T : class
						where TCallback : global::System.Delegate
					{
						internal MethodAdornmentsFordelegatePointerOfint__void(HandlerInformationFordelegatePointerOfint__void @handler) =>
							this.Handler = @handler;
						
						internal MethodAdornmentsFordelegatePointerOfint__void<T, TCallback> CallCount(uint @expectedCallCount)
						{
							this.Handler.SetExpectedCallCount(@expectedCallCount);
							return this;
						}
						
						internal MethodAdornmentsFordelegatePointerOfint__void<T, TCallback> Callback(TCallback @callback)
						{
							this.Handler.SetCallback(@callback);
							return this;
						}
						
						internal unsafe MethodAdornmentsFordelegatePointerOfint__void<T, TCallback> Returns(delegate*<int, void> @returnValue)
						{
							this.Handler.ReturnValue = @returnValue;
							return this;
						}
						
						public HandlerInformationFordelegatePointerOfint__void Handler { get; }
					}
					internal sealed class MethodAdornmentsForintPointer<T, TCallback>
						: global::Rocks.IAdornments<HandlerInformationForintPointer>
						where T : class
						where TCallback : global::System.Delegate
					{
						internal MethodAdornmentsForintPointer(HandlerInformationForintPointer @handler) =>
							this.Handler = @handler;
						
						internal MethodAdornmentsForintPointer<T, TCallback> CallCount(uint @expectedCallCount)
						{
							this.Handler.SetExpectedCallCount(@expectedCallCount);
							return this;
						}
						
						internal MethodAdornmentsForintPointer<T, TCallback> Callback(TCallback @callback)
						{
							this.Handler.SetCallback(@callback);
							return this;
						}
						
						internal unsafe MethodAdornmentsForintPointer<T, TCallback> Returns(int* @returnValue)
						{
							this.Handler.ReturnValue = @returnValue;
							return this;
						}
						
						public HandlerInformationForintPointer Handler { get; }
					}
				}
				
				internal static class CreateExpectationsOfIHavePointersExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.IHavePointers> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.IHavePointers> @self) =>
						new(@self);
					
					internal static global::MockTests.IHavePointers Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IHavePointers> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockIHavePointers(@self);
							@self.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIHavePointers
						: global::MockTests.IHavePointers
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIHavePointers(global::Rocks.Expectations.Expectations<global::MockTests.IHavePointers> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::Rocks.MemberIdentifier(0, "void DelegatePointerParameter(delegate*<int, void> @value)")]
						public unsafe void DelegatePointerParameter(delegate*<int, void> @value)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								var @foundMatch = false;
								
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHavePointers.ArgumentFordelegatePointerOfint__void>(@methodHandler.Expectations[0]).IsValid(@value))
									{
										@foundMatch = true;
										
										@methodHandler.IncrementCallCount();
										if (@methodHandler.Method is not null)
										{
											global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHavePointers.DelegatePointerParameterCallback_67936425010368164897427961753475545964149702154>(@methodHandler.Method)(@value);
										}
										break;
									}
								}
								
								if (!@foundMatch)
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers match for unsafe void DelegatePointerParameter(delegate*<int, void> @value)");
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for unsafe void DelegatePointerParameter(delegate*<int, void> @value)");
							}
						}
						
						[global::Rocks.MemberIdentifier(1, "delegate*<int, void> DelegatePointerReturn()")]
						public unsafe delegate*<int, void> DelegatePointerReturn()
						{
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHavePointers.DelegatePointerReturnCallback_348091753837477554967687149303310703900832221476>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHavePointers.HandlerInformationFordelegatePointerOfint__void>(@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for unsafe delegate*<int, void> DelegatePointerReturn()");
						}
						
						[global::Rocks.MemberIdentifier(2, "void PointerParameter(int* @value)")]
						public unsafe void PointerParameter(int* @value)
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								var @foundMatch = false;
								
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHavePointers.ArgumentForintPointer>(@methodHandler.Expectations[0]).IsValid(@value))
									{
										@foundMatch = true;
										
										@methodHandler.IncrementCallCount();
										if (@methodHandler.Method is not null)
										{
											global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHavePointers.PointerParameterCallback_448273544004536059019999557806138154926952273337>(@methodHandler.Method)(@value);
										}
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
						
						[global::Rocks.MemberIdentifier(3, "int* PointerReturn()")]
						public unsafe int* PointerReturn()
						{
							if (this.handlers.TryGetValue(3, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHavePointers.PointerReturnCallback_355763855309704752655277092464969889148092134081>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHavePointers.HandlerInformationForintPointer>(@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for unsafe int* PointerReturn()");
						}
						
					}
				}
				
				internal static class MethodExpectationsOfIHavePointersExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IHavePointers, global::MockTests.ProjectionsForIHavePointers.DelegatePointerParameterCallback_67936425010368164897427961753475545964149702154> DelegatePointerParameter(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHavePointers> @self, global::MockTests.ProjectionsForIHavePointers.ArgumentFordelegatePointerOfint__void @value)
					{
						global::System.ArgumentNullException.ThrowIfNull(@value);
						return new global::Rocks.MethodAdornments<global::MockTests.IHavePointers, global::MockTests.ProjectionsForIHavePointers.DelegatePointerParameterCallback_67936425010368164897427961753475545964149702154>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
					}
					internal static global::MockTests.ProjectionsForIHavePointers.MethodAdornmentsFordelegatePointerOfint__void<global::MockTests.IHavePointers, global::MockTests.ProjectionsForIHavePointers.DelegatePointerReturnCallback_348091753837477554967687149303310703900832221476> DelegatePointerReturn(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHavePointers> @self) =>
						new global::MockTests.ProjectionsForIHavePointers.MethodAdornmentsFordelegatePointerOfint__void<global::MockTests.IHavePointers, global::MockTests.ProjectionsForIHavePointers.DelegatePointerReturnCallback_348091753837477554967687149303310703900832221476>(@self.AddFordelegatePointerOfint__void(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.IHavePointers, global::MockTests.ProjectionsForIHavePointers.PointerParameterCallback_448273544004536059019999557806138154926952273337> PointerParameter(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHavePointers> @self, global::MockTests.ProjectionsForIHavePointers.ArgumentForintPointer @value)
					{
						global::System.ArgumentNullException.ThrowIfNull(@value);
						return new global::Rocks.MethodAdornments<global::MockTests.IHavePointers, global::MockTests.ProjectionsForIHavePointers.PointerParameterCallback_448273544004536059019999557806138154926952273337>(@self.Add(2, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
					}
					internal static global::MockTests.ProjectionsForIHavePointers.MethodAdornmentsForintPointer<global::MockTests.IHavePointers, global::MockTests.ProjectionsForIHavePointers.PointerReturnCallback_355763855309704752655277092464969889148092134081> PointerReturn(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHavePointers> @self) =>
						new global::MockTests.ProjectionsForIHavePointers.MethodAdornmentsForintPointer<global::MockTests.IHavePointers, global::MockTests.ProjectionsForIHavePointers.PointerReturnCallback_355763855309704752655277092464969889148092134081>(@self.AddForintPointer(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGeneratorV3>(code,
			new[] { (typeof(RockCreateGeneratorV3), "IHavePointers_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithRefStructAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			#nullable enable
			namespace MockTests
			{
				public interface IHaveInAndOutSpan
				{
					Span<int> Foo(Span<int> values);
					Span<byte> Values { get; set; }
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IHaveInAndOutSpan>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			using MockTests.ProjectionsForIHaveInAndOutSpan;
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			namespace MockTests
			{
				namespace ProjectionsForIHaveInAndOutSpan
				{
					internal delegate global::System.Span<int> FooCallback_313462758925781114777251005406226933687057720153(global::System.Span<int> @values);
					internal delegate global::System.Span<int> FooReturnValue_313462758925781114777251005406226933687057720153();
					internal delegate global::System.Span<byte> get_ValuesCallback_609802712345030162120672576179552875459533180788();
					internal delegate global::System.Span<byte> get_ValuesReturnValue_609802712345030162120672576179552875459533180788();
					internal delegate void set_ValuesCallback_273510090488501594048307694596437261624168638626(global::System.Span<byte> @value);
					internal delegate bool ArgEvaluationForSpanOfint(global::System.Span<int> @value);
					
					internal sealed class ArgForSpanOfint
						: global::Rocks.Argument
					{
						private readonly global::MockTests.ProjectionsForIHaveInAndOutSpan.ArgEvaluationForSpanOfint? evaluation;
						private readonly global::Rocks.ValidationState validation;
						
						internal ArgForSpanOfint() => this.validation = global::Rocks.ValidationState.None;
						
						internal ArgForSpanOfint(global::MockTests.ProjectionsForIHaveInAndOutSpan.ArgEvaluationForSpanOfint @evaluation)
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
					internal delegate bool ArgEvaluationForSpanOfbyte(global::System.Span<byte> @value);
					
					internal sealed class ArgForSpanOfbyte
						: global::Rocks.Argument
					{
						private readonly global::MockTests.ProjectionsForIHaveInAndOutSpan.ArgEvaluationForSpanOfbyte? evaluation;
						private readonly global::Rocks.ValidationState validation;
						
						internal ArgForSpanOfbyte() => this.validation = global::Rocks.ValidationState.None;
						
						internal ArgForSpanOfbyte(global::MockTests.ProjectionsForIHaveInAndOutSpan.ArgEvaluationForSpanOfbyte @evaluation)
						{
							this.evaluation = @evaluation;
							this.validation = global::Rocks.ValidationState.Evaluation;
						}
						
						public bool IsValid(global::System.Span<byte> @value) =>
							this.validation switch
							{
								global::Rocks.ValidationState.None => true,
								global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
								_ => throw new global::System.NotSupportedException("Invalid validation state."),
							};
					}
				}
				
				internal static class CreateExpectationsOfIHaveInAndOutSpanExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveInAndOutSpan> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveInAndOutSpan> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertyExpectations<global::MockTests.IHaveInAndOutSpan> Properties(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveInAndOutSpan> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.IHaveInAndOutSpan> Getters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.IHaveInAndOutSpan> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertySetterExpectations<global::MockTests.IHaveInAndOutSpan> Setters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.IHaveInAndOutSpan> @self) =>
						new(@self);
					
					internal static global::MockTests.IHaveInAndOutSpan Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveInAndOutSpan> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockIHaveInAndOutSpan(@self);
							@self.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIHaveInAndOutSpan
						: global::MockTests.IHaveInAndOutSpan
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIHaveInAndOutSpan(global::Rocks.Expectations.Expectations<global::MockTests.IHaveInAndOutSpan> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::Rocks.MemberIdentifier(0, "global::System.Span<int> Foo(global::System.Span<int> @values)")]
						public global::System.Span<int> Foo(global::System.Span<int> @values)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHaveInAndOutSpan.ArgForSpanOfint>(@methodHandler.Expectations[0]).IsValid(@values))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHaveInAndOutSpan.FooCallback_313462758925781114777251005406226933687057720153>(@methodHandler.Method)(@values) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<global::MockTests.ProjectionsForIHaveInAndOutSpan.FooReturnValue_313462758925781114777251005406226933687057720153>>(@methodHandler).ReturnValue!.Invoke();
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for global::System.Span<int> Foo(global::System.Span<int> @values)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::System.Span<int> Foo(global::System.Span<int> @values)");
						}
						
						[global::Rocks.MemberIdentifier(1, "get_Values()")]
						[global::Rocks.MemberIdentifier(2, "set_Values(@value)")]
						public global::System.Span<byte> Values
						{
							get
							{
								if (this.handlers.TryGetValue(1, out var @methodHandlers))
								{
									var @methodHandler = @methodHandlers[0];
									@methodHandler.IncrementCallCount();
									var @result = @methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHaveInAndOutSpan.get_ValuesCallback_609802712345030162120672576179552875459533180788>(@methodHandler.Method)() :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<global::MockTests.ProjectionsForIHaveInAndOutSpan.get_ValuesReturnValue_609802712345030162120672576179552875459533180788>>(@methodHandler).ReturnValue!.Invoke();
									return @result!;
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_Values())");
							}
							set
							{
								if (this.handlers.TryGetValue(2, out var @methodHandlers))
								{
									var @foundMatch = false;
									foreach (var @methodHandler in @methodHandlers)
									{
										if (global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHaveInAndOutSpan.ArgForSpanOfbyte>(@methodHandler.Expectations[0]).IsValid(@value))
										{
											@methodHandler.IncrementCallCount();
											@foundMatch = true;
											
											if (@methodHandler.Method is not null)
											{
												global::System.Runtime.CompilerServices.Unsafe.As<global::MockTests.ProjectionsForIHaveInAndOutSpan.set_ValuesCallback_273510090488501594048307694596437261624168638626>(@methodHandler.Method)(@value);
											}
											
											if (!@foundMatch)
											{
												throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_Values(@value)");
											}
											
											break;
										}
									}
								}
								else
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for set_Values(@value)");
								}
							}
						}
					}
				}
				
				internal static class MethodExpectationsOfIHaveInAndOutSpanExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IHaveInAndOutSpan, global::MockTests.ProjectionsForIHaveInAndOutSpan.FooCallback_313462758925781114777251005406226933687057720153, global::MockTests.ProjectionsForIHaveInAndOutSpan.FooReturnValue_313462758925781114777251005406226933687057720153> Foo(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveInAndOutSpan> @self, global::MockTests.ProjectionsForIHaveInAndOutSpan.ArgForSpanOfint @values)
					{
						global::System.ArgumentNullException.ThrowIfNull(@values);
						return new global::Rocks.MethodAdornments<global::MockTests.IHaveInAndOutSpan, global::MockTests.ProjectionsForIHaveInAndOutSpan.FooCallback_313462758925781114777251005406226933687057720153, global::MockTests.ProjectionsForIHaveInAndOutSpan.FooReturnValue_313462758925781114777251005406226933687057720153>(@self.Add<global::MockTests.ProjectionsForIHaveInAndOutSpan.FooReturnValue_313462758925781114777251005406226933687057720153>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @values }));
					}
				}
				
				internal static class PropertyGetterExpectationsOfIHaveInAndOutSpanExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.IHaveInAndOutSpan, global::MockTests.ProjectionsForIHaveInAndOutSpan.get_ValuesCallback_609802712345030162120672576179552875459533180788, global::MockTests.ProjectionsForIHaveInAndOutSpan.get_ValuesReturnValue_609802712345030162120672576179552875459533180788> Values(this global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.IHaveInAndOutSpan> @self) =>
						new global::Rocks.PropertyAdornments<global::MockTests.IHaveInAndOutSpan, global::MockTests.ProjectionsForIHaveInAndOutSpan.get_ValuesCallback_609802712345030162120672576179552875459533180788, global::MockTests.ProjectionsForIHaveInAndOutSpan.get_ValuesReturnValue_609802712345030162120672576179552875459533180788>(@self.Add<global::MockTests.ProjectionsForIHaveInAndOutSpan.get_ValuesReturnValue_609802712345030162120672576179552875459533180788>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				internal static class PropertySetterExpectationsOfIHaveInAndOutSpanExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.IHaveInAndOutSpan, global::MockTests.ProjectionsForIHaveInAndOutSpan.set_ValuesCallback_273510090488501594048307694596437261624168638626> Values(this global::Rocks.Expectations.PropertySetterExpectations<global::MockTests.IHaveInAndOutSpan> @self, global::Rocks.Argument<global::MockTests.ProjectionsForIHaveInAndOutSpan.ArgForSpanOfbyte> @value) =>
						new global::Rocks.PropertyAdornments<global::MockTests.IHaveInAndOutSpan, global::MockTests.ProjectionsForIHaveInAndOutSpan.set_ValuesCallback_273510090488501594048307694596437261624168638626>(@self.Add(2, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGeneratorV3>(code,
			new[] { (typeof(RockCreateGeneratorV3), "IHaveInAndOutSpan_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}