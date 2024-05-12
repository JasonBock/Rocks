﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class DefaultValuesGeneratorTests
{
	[Test]
	public static async Task CreateWhenExplicitImplementationHasDefaultValuesAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Threading.Tasks;

			[assembly: RockCreate<MockTests.IRequest<object>>]

			namespace MockTests
			{
				public struct SomeStruct { }

				public interface IRequest<T>
					where T : class
				{
					Task<T> Send(object values, SomeStruct someStruct = default);
					Task Send(T message, SomeStruct someStruct = default);
				}	
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
			
			namespace MockTests
			{
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
				internal sealed class IRequestOfobjectCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object, global::MockTests.SomeStruct, global::System.Threading.Tasks.Task<object>>, global::System.Threading.Tasks.Task<object>>
					{
						public global::Rocks.Argument<object> @values { get; set; }
						public global::Rocks.Argument<global::MockTests.SomeStruct> @someStruct { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IRequestOfobjectCreateExpectations.Handler0>? @handlers0;
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<object, global::MockTests.SomeStruct, global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>
					{
						public global::Rocks.Argument<object> @message { get; set; }
						public global::Rocks.Argument<global::MockTests.SomeStruct> @someStruct { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IRequestOfobjectCreateExpectations.Handler1>? @handlers1;
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							if (this.handlers0 is not null) { failures.AddRange(this.Verify(this.handlers0, 0)); }
							if (this.handlers1 is not null) { failures.AddRange(this.Verify(this.handlers1, 1)); }
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class Mock
						: global::MockTests.IRequest<object>
					{
						public Mock(global::MockTests.IRequestOfobjectCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public global::System.Threading.Tasks.Task<object> Send(object @values, global::MockTests.SomeStruct @someStruct = default)
						{
							if (this.Expectations.handlers0 is not null)
							{
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@values.IsValid(@values!) &&
										@handler.@someStruct.IsValid(@someStruct!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@values!, @someStruct!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(0)}");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
						}
						
						[global::Rocks.MemberIdentifier(1)]
						global::System.Threading.Tasks.Task global::MockTests.IRequest<object>.Send(object @message, global::MockTests.SomeStruct @someStruct)
						{
							if (this.Expectations.handlers1 is not null)
							{
								foreach (var @handler in this.Expectations.handlers1)
								{
									if (@handler.@message.IsValid(@message!) &&
										@handler.@someStruct.IsValid(@someStruct!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@message!, @someStruct!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(1)}");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(1)}");
						}
						
						private global::MockTests.IRequestOfobjectCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.IRequestOfobjectCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.IRequestOfobjectCreateExpectations.Adornments.AdornmentsForHandler0 Send(global::Rocks.Argument<object> @values, global::Rocks.Argument<global::MockTests.SomeStruct> @someStruct)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@values);
							global::System.ArgumentNullException.ThrowIfNull(@someStruct);
							
							var @handler = new global::MockTests.IRequestOfobjectCreateExpectations.Handler0
							{
								@values = @values,
								@someStruct = @someStruct.Transform(default),
							};
							
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						internal global::MockTests.IRequestOfobjectCreateExpectations.Adornments.AdornmentsForHandler0 Send(global::Rocks.Argument<object> @values, global::MockTests.SomeStruct @someStruct = default) =>
							this.Send(@values, global::Rocks.Arg.Is(@someStruct));
						
						private global::MockTests.IRequestOfobjectCreateExpectations Expectations { get; }
					}
					internal sealed class ExplicitMethodExpectationsForIRequestOfObject
					{
						internal ExplicitMethodExpectationsForIRequestOfObject(global::MockTests.IRequestOfobjectCreateExpectations expectations) =>
							this.Expectations = expectations;
					
						internal global::MockTests.IRequestOfobjectCreateExpectations.Adornments.AdornmentsForHandler1 Send(global::Rocks.Argument<object> @message, global::Rocks.Argument<global::MockTests.SomeStruct> @someStruct)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@message);
							global::System.ArgumentNullException.ThrowIfNull(@someStruct);
							
							var @handler = new global::MockTests.IRequestOfobjectCreateExpectations.Handler1
							{
								@message = @message,
								@someStruct = @someStruct,
							};
							
							if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(@handler); }
							else { this.Expectations.handlers1.Add(@handler); }
							return new(@handler);
						}
						internal global::MockTests.IRequestOfobjectCreateExpectations.Adornments.AdornmentsForHandler1 Send(global::Rocks.Argument<object> @message, global::MockTests.SomeStruct @someStruct = default) =>
							this.Send(@message, global::Rocks.Arg.Is(@someStruct));
						
						private global::MockTests.IRequestOfobjectCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IRequestOfobjectCreateExpectations.MethodExpectations Methods { get; }
					internal global::MockTests.IRequestOfobjectCreateExpectations.ExplicitMethodExpectationsForIRequestOfObject ExplicitMethodsForIRequestOfObject { get; }
					
					internal IRequestOfobjectCreateExpectations() =>
						(this.Methods, this.ExplicitMethodsForIRequestOfObject) = (new(this), new(this));
					
					internal global::MockTests.IRequest<object> Instance()
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
						public interface IAdornmentsForIRequestOfobject<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIRequestOfobject<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.IRequestOfobjectCreateExpectations.Handler0, global::System.Func<object, global::MockTests.SomeStruct, global::System.Threading.Tasks.Task<object>>, global::System.Threading.Tasks.Task<object>>, IAdornmentsForIRequestOfobject<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.IRequestOfobjectCreateExpectations.Handler0 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler1
							: global::Rocks.Adornments<AdornmentsForHandler1, global::MockTests.IRequestOfobjectCreateExpectations.Handler1, global::System.Func<object, global::MockTests.SomeStruct, global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>, IAdornmentsForIRequestOfobject<AdornmentsForHandler1>
						{
							public AdornmentsForHandler1(global::MockTests.IRequestOfobjectCreateExpectations.Handler1 handler)
								: base(handler) { }
						}
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.IRequestobject_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task CreateWhenGenericParameterHasOptionalDefaultValueAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockCreate<IGenericDefault>]

			public interface IGenericDefault
			{
				void Setup<T>(T initialValue = default(T));
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
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class IGenericDefaultCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0<T>
					: global::Rocks.Handler<global::System.Action<T>>
				{
					public global::Rocks.Argument<T> @initialValue { get; set; }
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
					: global::IGenericDefault
				{
					public Mock(global::IGenericDefaultCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void Setup<T>(T @initialValue = default!)
					{
						if (this.Expectations.handlers0 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @genericHandler in this.Expectations.handlers0)
							{
								if (@genericHandler is global::IGenericDefaultCreateExpectations.Handler0<T> @handler)
								{
									if (@handler.@initialValue.IsValid(@initialValue!))
									{
										@foundMatch = true;
										@handler.CallCount++;
										@handler.Callback?.Invoke(@initialValue!);
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
					
					private global::IGenericDefaultCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IGenericDefaultCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::IGenericDefaultCreateExpectations.Adornments.AdornmentsForHandler0<T> Setup<T>(global::Rocks.Argument<T> @initialValue)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@initialValue);
						
						var @handler = new global::IGenericDefaultCreateExpectations.Handler0<T>
						{
							@initialValue = @initialValue.Transform(default!),
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					internal global::IGenericDefaultCreateExpectations.Adornments.AdornmentsForHandler0<T> Setup<T>(T @initialValue = default!) =>
						this.Setup<T>(global::Rocks.Arg.Is(@initialValue));
					
					private global::IGenericDefaultCreateExpectations Expectations { get; }
				}
				
				internal global::IGenericDefaultCreateExpectations.MethodExpectations Methods { get; }
				
				internal IGenericDefaultCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IGenericDefault Instance()
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
					public interface IAdornmentsForIGenericDefault<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIGenericDefault<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0<T>
						: global::Rocks.Adornments<AdornmentsForHandler0<T>, global::IGenericDefaultCreateExpectations.Handler0<T>, global::System.Action<T>>, IAdornmentsForIGenericDefault<AdornmentsForHandler0<T>>
					{
						public AdornmentsForHandler0(global::IGenericDefaultCreateExpectations.Handler0<T> handler)
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
			[(typeof(RockAttributeGenerator), "IGenericDefault_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task MakeWhenGenericParameterHasOptionalDefaultValueAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockMake<IGenericDefault>]

			public interface IGenericDefault
			{
				void Setup<T>(T initialValue = default(T));
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
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class IGenericDefaultMakeExpectations
			{
				internal global::IGenericDefault Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::IGenericDefault
				{
					public Mock()
					{
					}
					
					public void Setup<T>(T @initialValue = default!)
					{
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "IGenericDefault_Rock_Make.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task CreateWithPositiveInfinityAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<IUseInfinity>]

			public interface IUseInfinity
			{
				void Use(double value = double.PositiveInfinity);
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
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class IUseInfinityCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Action<double>>
				{
					public global::Rocks.Argument<double> @value { get; set; }
				}
				private global::Rocks.Handlers<global::IUseInfinityCreateExpectations.Handler0>? @handlers0;
				
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
					: global::IUseInfinity
				{
					public Mock(global::IUseInfinityCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void Use(double @value = double.PositiveInfinity)
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
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(0)}");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
						}
					}
					
					private global::IUseInfinityCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IUseInfinityCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::IUseInfinityCreateExpectations.Adornments.AdornmentsForHandler0 Use(global::Rocks.Argument<double> @value)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@value);
						
						var @handler = new global::IUseInfinityCreateExpectations.Handler0
						{
							@value = @value.Transform(double.PositiveInfinity),
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					internal global::IUseInfinityCreateExpectations.Adornments.AdornmentsForHandler0 Use(double @value = double.PositiveInfinity) =>
						this.Use(global::Rocks.Arg.Is(@value));
					
					private global::IUseInfinityCreateExpectations Expectations { get; }
				}
				
				internal global::IUseInfinityCreateExpectations.MethodExpectations Methods { get; }
				
				internal IUseInfinityCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IUseInfinity Instance()
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
					public interface IAdornmentsForIUseInfinity<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIUseInfinity<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::IUseInfinityCreateExpectations.Handler0, global::System.Action<double>>, IAdornmentsForIUseInfinity<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::IUseInfinityCreateExpectations.Handler0 handler)
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
			[(typeof(RockAttributeGenerator), "IUseInfinity_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task MakeWithPositiveInfinityAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockMake<IUseInfinity>]

			public interface IUseInfinity
			{
			  void Use(double value = double.PositiveInfinity);
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
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class IUseInfinityMakeExpectations
			{
				internal global::IUseInfinity Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::IUseInfinity
				{
					public Mock()
					{
					}
					
					public void Use(double @value = double.PositiveInfinity)
					{
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "IUseInfinity_Rock_Make.g.cs", generatedCode)],
			[]);
	}
}