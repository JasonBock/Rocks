﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ProjectionGenericsAndPointersGeneratorTests
{
	[Test]
	public static async Task GenerateWithPointersAndGenericsAndUnmanagedConstraintAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(ISurface), BuildType.Create | BuildType.Make)]

			public interface ISurface
			{
				unsafe void Create<T>(T* allocator) where T : unmanaged;
			}
			""";

		var createGeneratedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class ISurfaceCreateExpectations
				: global::Rocks.Expectations
			{
				internal static class Projections
				{
					internal unsafe delegate void Callback_455733297849133658792680279988631751332086808415<T>(T* @allocator) where T : unmanaged;
					internal unsafe delegate bool ArgumentEvaluationForTPointer<T>(T* @value) where T : unmanaged;
					
					internal unsafe sealed class ArgumentForTPointer<T>
						: global::Rocks.Argument where T : unmanaged
					{
						private readonly global::ISurfaceCreateExpectations.Projections.ArgumentEvaluationForTPointer<T>? evaluation;
						private readonly T* value;
						private readonly global::Rocks.ValidationState validation;
						
						internal ArgumentForTPointer() => this.validation = global::Rocks.ValidationState.None;
						
						internal ArgumentForTPointer(T* @value)
						{
							this.value = @value;
							this.validation = global::Rocks.ValidationState.Value;
						}
						
						internal ArgumentForTPointer(global::ISurfaceCreateExpectations.Projections.ArgumentEvaluationForTPointer<T> @evaluation)
						{
							this.evaluation = @evaluation;
							this.validation = global::Rocks.ValidationState.Evaluation;
						}
						
						public static implicit operator ArgumentForTPointer<T>(T* @value) => new(@value);
						
						public bool IsValid(T* @value) =>
							this.validation switch
							{
								global::Rocks.ValidationState.None => true,
								global::Rocks.ValidationState.Value => @value == this.value,
								global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
								global::Rocks.ValidationState.DefaultValue => throw new global::System.NotSupportedException("Cannot validate an argument value in the ValidationState.DefaultValue state."),
								_ => throw new global::System.ComponentModel.InvalidEnumArgumentException($"Invalid value for validation: {this.validation}")
							};
					}
				}
				
				internal sealed class Handler0<T>
					: global::Rocks.Handler<global::ISurfaceCreateExpectations.Projections.Callback_455733297849133658792680279988631751332086808415<T>>
					where T : unmanaged
				{
					public global::ISurfaceCreateExpectations.Projections.ArgumentForTPointer<T> @allocator { get; set; }
				}
				private global::Rocks.Handlers<global::Rocks.Handler>? @handlers0;
				
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
					: global::ISurface
				{
					public Mock(global::ISurfaceCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public unsafe void Create<T>(T* @allocator)
						where T : unmanaged
					{
						if (this.Expectations.handlers0 is not null)
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
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(0)}");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
						}
					}
					
					private global::ISurfaceCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::ISurfaceCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::ISurfaceCreateExpectations.Adornments.AdornmentsForHandler0<T> Create<T>(global::ISurfaceCreateExpectations.Projections.ArgumentForTPointer<T> @allocator) where T : unmanaged
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@allocator);
						
						var @handler = new global::ISurfaceCreateExpectations.Handler0<T>
						{
							@allocator = @allocator,
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					private global::ISurfaceCreateExpectations Expectations { get; }
				}
				
				internal global::ISurfaceCreateExpectations.MethodExpectations Methods { get; }
				
				internal ISurfaceCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::ISurface Instance()
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
					public interface IAdornmentsForISurface<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForISurface<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0<T>
						: global::Rocks.Adornments<AdornmentsForHandler0<T>, global::ISurfaceCreateExpectations.Handler0<T>, global::ISurfaceCreateExpectations.Projections.Callback_455733297849133658792680279988631751332086808415<T>>, IAdornmentsForISurface<AdornmentsForHandler0<T>> where T : unmanaged
					{
						public AdornmentsForHandler0(global::ISurfaceCreateExpectations.Handler0<T> handler)
							: base(handler) { }
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			""";

		var makeGeneratedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class ISurfaceMakeExpectations
			{
				internal global::ISurface Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::ISurface
				{
					public Mock()
					{
					}
					
					public unsafe void Create<T>(T* @allocator)
						where T : unmanaged
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
				("ISurface_Rock_Create.g.cs", createGeneratedCode),
				("ISurface_Rock_Make.g.cs", makeGeneratedCode)
			],
			[]);
	}
}