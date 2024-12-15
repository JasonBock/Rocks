﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class PartialExpectationsTests
{
	[Test]
	public static async Task CreateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			public interface ITarget
			{
				void DoSomething();
			}

			[RockPartial(typeof(ITarget), BuildType.Create)]
			public partial class TargetExpectationsStuff;
			""";

		var generatedCode =
			""""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			public partial class TargetExpectationsStuff
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Action>
				{ }
				private global::Rocks.Handlers<global::TargetExpectationsStuff.Handler0>? @handlers0;
				
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
					: global::ITarget
				{
					public Mock(global::TargetExpectationsStuff @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void DoSomething()
					{
						if (this.Expectations.handlers0 is not null)
						{
							var @handler = this.Expectations.handlers0.First;
							@handler.CallCount++;
							@handler.Callback?.Invoke();
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException(
								$"""
								No handlers were found for {this.GetType().GetMemberDescription(0)}
								""");
						}
					}
					
					private global::TargetExpectationsStuff Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::TargetExpectationsStuff expectations) =>
						this.Expectations = expectations;
					
					internal global::TargetExpectationsStuff.Adornments.AdornmentsForHandler0 DoSomething()
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						var handler = new global::TargetExpectationsStuff.Handler0();
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(handler); }
						else { this.Expectations.handlers0.Add(handler); }
						return new(handler);
					}
					
					private global::TargetExpectationsStuff Expectations { get; }
				}
				
				internal global::TargetExpectationsStuff.MethodExpectations Methods { get; }
				
				internal TargetExpectationsStuff() =>
					(this.Methods) = (new(this));
				
				internal global::ITarget Instance()
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
					public interface IAdornmentsForTargetExpectationsStuff<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForTargetExpectationsStuff<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::TargetExpectationsStuff.Handler0, global::System.Action>, IAdornmentsForTargetExpectationsStuff<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::TargetExpectationsStuff.Handler0 handler)
							: base(handler) { }
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			"""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[
				("ITarget_Partial_Rock_Create.g.cs", generatedCode),
			],
			[]);
	}

	[Test]
	public static async Task CreateWithProjectTypesDuplicatedWithRockAttributeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: Rock(typeof(ITarget), BuildType.Create)]

			public unsafe interface ITarget
			{
				void PointerParameter(int* value);
			}
			
			public unsafe interface IPartialTarget
			{
				void PointerParameter(int* value);
			}
			
			[RockPartial(typeof(IPartialTarget), BuildType.Create)]
			public partial class TargetExpectationsStuff;
			""";

		var assemblyGeneratedCode =
			""""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal unsafe sealed class ITargetCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<Handler0.CallbackForHandler>
				{
					internal unsafe delegate void CallbackForHandler(int* @value);
					public global::Rocks.Projections.PointerArgument<int> @value { get; set; }
				}
				private global::Rocks.Handlers<global::ITargetCreateExpectations.Handler0>? @handlers0;
				
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
					: global::ITarget
				{
					public Mock(global::ITargetCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public unsafe void PointerParameter(int* @value)
					{
						if (this.Expectations.handlers0 is not null)
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
								throw new global::Rocks.Exceptions.ExpectationException(
									$"""
									No handlers match for {this.GetType().GetMemberDescription(0)}
										value: <Not formattable>
									""");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException(
								$"""
								No handlers were found for {this.GetType().GetMemberDescription(0)}
									value: <Not formattable>
								""");
						}
					}
					
					private global::ITargetCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::ITargetCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::ITargetCreateExpectations.Adornments.AdornmentsForHandler0 PointerParameter(global::Rocks.Projections.PointerArgument<int> @value)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@value);
						
						var @handler = new global::ITargetCreateExpectations.Handler0
						{
							@value = @value,
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					private global::ITargetCreateExpectations Expectations { get; }
				}
				
				internal global::ITargetCreateExpectations.MethodExpectations Methods { get; }
				
				internal ITargetCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::ITarget Instance()
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
					public interface IAdornmentsForITarget<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForITarget<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::ITargetCreateExpectations.Handler0, global::ITargetCreateExpectations.Handler0.CallbackForHandler>, IAdornmentsForITarget<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::ITargetCreateExpectations.Handler0 handler)
							: base(handler) { }
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			"""";

		var partialGeneratedCode =
			""""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			public unsafe partial class TargetExpectationsStuff
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<Handler0.CallbackForHandler>
				{
					internal unsafe delegate void CallbackForHandler(int* @value);
					public global::Rocks.Projections.PointerArgument<int> @value { get; set; }
				}
				private global::Rocks.Handlers<global::TargetExpectationsStuff.Handler0>? @handlers0;
				
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
					: global::IPartialTarget
				{
					public Mock(global::TargetExpectationsStuff @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public unsafe void PointerParameter(int* @value)
					{
						if (this.Expectations.handlers0 is not null)
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
								throw new global::Rocks.Exceptions.ExpectationException(
									$"""
									No handlers match for {this.GetType().GetMemberDescription(0)}
										value: <Not formattable>
									""");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException(
								$"""
								No handlers were found for {this.GetType().GetMemberDescription(0)}
									value: <Not formattable>
								""");
						}
					}
					
					private global::TargetExpectationsStuff Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::TargetExpectationsStuff expectations) =>
						this.Expectations = expectations;
					
					internal global::TargetExpectationsStuff.Adornments.AdornmentsForHandler0 PointerParameter(global::Rocks.Projections.PointerArgument<int> @value)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@value);
						
						var @handler = new global::TargetExpectationsStuff.Handler0
						{
							@value = @value,
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					private global::TargetExpectationsStuff Expectations { get; }
				}
				
				internal global::TargetExpectationsStuff.MethodExpectations Methods { get; }
				
				internal TargetExpectationsStuff() =>
					(this.Methods) = (new(this));
				
				internal global::IPartialTarget Instance()
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
					public interface IAdornmentsForTargetExpectationsStuff<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForTargetExpectationsStuff<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::TargetExpectationsStuff.Handler0, global::TargetExpectationsStuff.Handler0.CallbackForHandler>, IAdornmentsForTargetExpectationsStuff<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::TargetExpectationsStuff.Handler0 handler)
							: base(handler) { }
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			"""";

		var projectionGeneratedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			using System;
			using System.ComponentModel;
			
			#nullable enable
			
			namespace Rocks.Projections;
			
			internal unsafe delegate bool PointerArgumentEvaluation<T>(T* @value) where T : unmanaged;
			
			internal sealed unsafe class PointerArgument<T>
				: Argument
				where T : unmanaged
			{
				private readonly PointerArgumentEvaluation<T>? evaluation;
				private readonly T* value;
				private readonly ValidationState validation;
			
				internal PointerArgument() => this.validation = ValidationState.None;
			
				internal PointerArgument(T* @value)
				{
					this.value = @value;
					this.validation = ValidationState.Value;
				}
			
				internal PointerArgument(PointerArgumentEvaluation<T> @evaluation)
				{
					this.evaluation = @evaluation;
					this.validation = ValidationState.Evaluation;
				}
			
				public static implicit operator PointerArgument<T>(T* @value) => new(@value);
			
				public bool IsValid(T* @value) =>
					this.validation switch
					{
						ValidationState.None => true,
						ValidationState.Value => @value == this.value,
						ValidationState.Evaluation => this.evaluation!(@value),
						ValidationState.DefaultValue => throw new NotSupportedException("Cannot validate an argument value in the ValidationState.DefaultValue state."),
						_ => throw new InvalidEnumArgumentException($"Invalid value for validation: {this.validation}")
					};
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[
				("ITarget_Rock_Create.g.cs", assemblyGeneratedCode),
				("IPartialTarget_Partial_Rock_Create.g.cs", partialGeneratedCode),
				("Pointer_Projection.g.cs", projectionGeneratedCode),
			],
			[]);
	}

	[Test]
	public static async Task CreateWithGenericsAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			public interface ITarget<T>
			{
				T DoSomething();
			}

			[RockPartial(typeof(ITarget<>), BuildType.Create)]
			public partial class TargetExpectationsStuff<T>;
			""";

		var generatedCode =
			""""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			public partial class TargetExpectationsStuff<T>
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Func<T>, T>
				{ }
				private global::Rocks.Handlers<global::TargetExpectationsStuff<T>.Handler0>? @handlers0;
				
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
					: global::ITarget<T>
				{
					public Mock(global::TargetExpectationsStuff<T> @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public T DoSomething()
					{
						if (this.Expectations.handlers0 is not null)
						{
							var @handler = this.Expectations.handlers0.First;
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException(
							$"""
							No handlers were found for {this.GetType().GetMemberDescription(0)}
							""");
					}
					
					private global::TargetExpectationsStuff<T> Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::TargetExpectationsStuff<T> expectations) =>
						this.Expectations = expectations;
					
					internal global::TargetExpectationsStuff<T>.Adornments.AdornmentsForHandler0 DoSomething()
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						var handler = new global::TargetExpectationsStuff<T>.Handler0();
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(handler); }
						else { this.Expectations.handlers0.Add(handler); }
						return new(handler);
					}
					
					private global::TargetExpectationsStuff<T> Expectations { get; }
				}
				
				internal global::TargetExpectationsStuff<T>.MethodExpectations Methods { get; }
				
				internal TargetExpectationsStuff() =>
					(this.Methods) = (new(this));
				
				internal global::ITarget<T> Instance()
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
					public interface IAdornmentsForTargetExpectationsStuff<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForTargetExpectationsStuff<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::TargetExpectationsStuff<T>.Handler0, global::System.Func<T>, T>, IAdornmentsForTargetExpectationsStuff<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::TargetExpectationsStuff<T>.Handler0 handler)
							: base(handler) { }
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			"""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[
				("ITargetT_Partial_Rock_Create.g.cs", generatedCode),
			],
			[]);
	}

	[Test]
	public static async Task CreateInDifferentNamespacesAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTarget
			{
				public interface ITarget
				{
					void DoSomething();
				}
			}

			namespace ExpectationsTarget
			{
				[RockPartial(typeof(MockTarget.ITarget), BuildType.Create)]
				public partial class TargetExpectationsStuff;
			}
			""";

		var generatedCode =
			""""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			namespace ExpectationsTarget
			{
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
				public partial class TargetExpectationsStuff
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Action>
					{ }
					private global::Rocks.Handlers<global::ExpectationsTarget.TargetExpectationsStuff.Handler0>? @handlers0;
					
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
						: global::MockTarget.ITarget
					{
						public Mock(global::ExpectationsTarget.TargetExpectationsStuff @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public void DoSomething()
						{
							if (this.Expectations.handlers0 is not null)
							{
								var @handler = this.Expectations.handlers0.First;
								@handler.CallCount++;
								@handler.Callback?.Invoke();
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException(
									$"""
									No handlers were found for {this.GetType().GetMemberDescription(0)}
									""");
							}
						}
						
						private global::ExpectationsTarget.TargetExpectationsStuff Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::ExpectationsTarget.TargetExpectationsStuff expectations) =>
							this.Expectations = expectations;
						
						internal global::ExpectationsTarget.TargetExpectationsStuff.Adornments.AdornmentsForHandler0 DoSomething()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::ExpectationsTarget.TargetExpectationsStuff.Handler0();
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(handler); }
							else { this.Expectations.handlers0.Add(handler); }
							return new(handler);
						}
						
						private global::ExpectationsTarget.TargetExpectationsStuff Expectations { get; }
					}
					
					internal global::ExpectationsTarget.TargetExpectationsStuff.MethodExpectations Methods { get; }
					
					internal TargetExpectationsStuff() =>
						(this.Methods) = (new(this));
					
					internal global::MockTarget.ITarget Instance()
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
						public interface IAdornmentsForTargetExpectationsStuff<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForTargetExpectationsStuff<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::ExpectationsTarget.TargetExpectationsStuff.Handler0, global::System.Action>, IAdornmentsForTargetExpectationsStuff<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::ExpectationsTarget.TargetExpectationsStuff.Handler0 handler)
								: base(handler) { }
						}
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			"""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[
				("MockTarget.ITarget_Partial_Rock_Create.g.cs", generatedCode),
			],
			[]);
	}

	[Test]
	public static async Task MakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			public interface ITarget
			{
				void DoSomething();
			}

			[RockPartial(typeof(ITarget), BuildType.Make)]
			public partial class TargetExpectationsStuff;
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			public partial class TargetExpectationsStuff
			{
				internal global::ITarget Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::ITarget
				{
					public Mock()
					{
					}
					
					public void DoSomething()
					{
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[
				("ITarget_Partial_Rock_Make.g.cs", generatedCode),
			],
			[]);
	}

	[Test]
	public static async Task MakeWithGenericsAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			public interface ITarget<T>
			{
				T DoSomething();
			}

			[RockPartial(typeof(ITarget<>), BuildType.Make)]
			public partial class TargetExpectationsStuff<T>;
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			public partial class TargetExpectationsStuff<T>
			{
				internal global::ITarget<T> Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::ITarget<T>
				{
					public Mock()
					{
					}
					
					public T DoSomething()
					{
						return default!;
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[
				("ITargetT_Partial_Rock_Make.g.cs", generatedCode),
			],
			[]);
	}

	[Test]
	public static async Task MakeInDifferentNamespacesAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTarget
			{
				public interface ITarget
				{
					void DoSomething();
				}
			}

			namespace ExpectationsTarget
			{
				[RockPartial(typeof(MockTarget.ITarget), BuildType.Make)]
				public partial class TargetExpectationsStuff;
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
			
			namespace ExpectationsTarget
			{
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
				public partial class TargetExpectationsStuff
				{
					internal global::MockTarget.ITarget Instance()
					{
						return new Mock();
					}
					
					private sealed class Mock
						: global::MockTarget.ITarget
					{
						public Mock()
						{
						}
						
						public void DoSomething()
						{
						}
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[
				("MockTarget.ITarget_Partial_Rock_Make.g.cs", generatedCode),
			],
			[]);
	}
}