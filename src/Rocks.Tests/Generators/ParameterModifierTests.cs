﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ParameterModifierTests
{
	[Test]
	public static async Task GenerateCreateWithInParameterAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<IParameterModifier>]

			public interface IParameterModifier
			{
			    void Modify(in string value);   
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
			
			internal sealed class IParameterModifierCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Action<string>>
				{
					public global::Rocks.Argument<string> @value { get; set; }
				}
				private global::Rocks.Handlers<global::IParameterModifierCreateExpectations.Handler0>? @handlers0;
				
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
					: global::IParameterModifier
				{
					public Mock(global::IParameterModifierCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void Modify(in string @value)
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
					
					private global::IParameterModifierCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IParameterModifierCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::IParameterModifierCreateExpectations.Adornments.AdornmentsForHandler0 Modify(global::Rocks.Argument<string> @value)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@value);
						
						var @handler = new global::IParameterModifierCreateExpectations.Handler0
						{
							@value = @value,
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					private global::IParameterModifierCreateExpectations Expectations { get; }
				}
				
				internal global::IParameterModifierCreateExpectations.MethodExpectations Methods { get; }
				
				internal IParameterModifierCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IParameterModifier Instance()
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
					public interface IAdornmentsForIParameterModifier<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIParameterModifier<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::IParameterModifierCreateExpectations.Handler0, global::System.Action<string>>, IAdornmentsForIParameterModifier<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::IParameterModifierCreateExpectations.Handler0 handler)
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
			[(typeof(RockAttributeGenerator), "IParameterModifier_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateMakeWithInParameterAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<IParameterModifier>]

			public interface IParameterModifier
			{
			    void Modify(in string value);   
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
			
			internal sealed class IParameterModifierMakeExpectations
			{
				internal global::IParameterModifier Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::IParameterModifier
				{
					public Mock()
					{
					}
					
					public void Modify(in string @value)
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
			[(typeof(RockAttributeGenerator), "IParameterModifier_Rock_Make.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateCreateWithOutParameterAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<IParameterModifier>]

			public interface IParameterModifier
			{
			    void Modify(out string value);   
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
			
			internal sealed class IParameterModifierCreateExpectations
				: global::Rocks.Expectations
			{
				internal static class Projections
				{
					internal delegate void Callback_193235261019447779478409340058228437220444154875(out string @value);
				}
				
				internal sealed class Handler0
					: global::Rocks.Handler<global::IParameterModifierCreateExpectations.Projections.Callback_193235261019447779478409340058228437220444154875>
				{
					public global::Rocks.Argument<string> @value { get; set; }
				}
				private global::Rocks.Handlers<global::IParameterModifierCreateExpectations.Handler0>? @handlers0;
				
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
					: global::IParameterModifier
				{
					public Mock(global::IParameterModifierCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void Modify(out string @value)
					{
						value = default!;
						if (this.Expectations.handlers0 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @handler in this.Expectations.handlers0)
							{
								if (@handler.@value.IsValid(@value!))
								{
									@foundMatch = true;
									@handler.CallCount++;
									@handler.Callback?.Invoke(out @value!);
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
					
					private global::IParameterModifierCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IParameterModifierCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::IParameterModifierCreateExpectations.Adornments.AdornmentsForHandler0 Modify(global::Rocks.Argument<string> @value)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@value);
						
						var @handler = new global::IParameterModifierCreateExpectations.Handler0
						{
							@value = global::Rocks.Arg.Any<string>(),
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					private global::IParameterModifierCreateExpectations Expectations { get; }
				}
				
				internal global::IParameterModifierCreateExpectations.MethodExpectations Methods { get; }
				
				internal IParameterModifierCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IParameterModifier Instance()
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
					public interface IAdornmentsForIParameterModifier<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIParameterModifier<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::IParameterModifierCreateExpectations.Handler0, global::IParameterModifierCreateExpectations.Projections.Callback_193235261019447779478409340058228437220444154875>, IAdornmentsForIParameterModifier<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::IParameterModifierCreateExpectations.Handler0 handler)
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
			[(typeof(RockAttributeGenerator), "IParameterModifier_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateMakeWithOutParameterAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<IParameterModifier>]

			public interface IParameterModifier
			{
			    void Modify(out string value);   
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
			
			internal sealed class IParameterModifierMakeExpectations
			{
				internal global::IParameterModifier Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::IParameterModifier
				{
					public Mock()
					{
					}
					
					public void Modify(out string @value)
					{
						value = default!;
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "IParameterModifier_Rock_Make.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateCreateWithRefParameterAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<IParameterModifier>]

			public interface IParameterModifier
			{
			    void Modify(ref string value);   
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
			
			internal sealed class IParameterModifierCreateExpectations
				: global::Rocks.Expectations
			{
				internal static class Projections
				{
					internal delegate void Callback_117490457623372471697910661720505969856490442481(ref string @value);
				}
				
				internal sealed class Handler0
					: global::Rocks.Handler<global::IParameterModifierCreateExpectations.Projections.Callback_117490457623372471697910661720505969856490442481>
				{
					public global::Rocks.Argument<string> @value { get; set; }
				}
				private global::Rocks.Handlers<global::IParameterModifierCreateExpectations.Handler0>? @handlers0;
				
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
					: global::IParameterModifier
				{
					public Mock(global::IParameterModifierCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void Modify(ref string @value)
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
									@handler.Callback?.Invoke(ref @value!);
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
					
					private global::IParameterModifierCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IParameterModifierCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::IParameterModifierCreateExpectations.Adornments.AdornmentsForHandler0 Modify(global::Rocks.Argument<string> @value)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@value);
						
						var @handler = new global::IParameterModifierCreateExpectations.Handler0
						{
							@value = @value,
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					private global::IParameterModifierCreateExpectations Expectations { get; }
				}
				
				internal global::IParameterModifierCreateExpectations.MethodExpectations Methods { get; }
				
				internal IParameterModifierCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IParameterModifier Instance()
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
					public interface IAdornmentsForIParameterModifier<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIParameterModifier<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::IParameterModifierCreateExpectations.Handler0, global::IParameterModifierCreateExpectations.Projections.Callback_117490457623372471697910661720505969856490442481>, IAdornmentsForIParameterModifier<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::IParameterModifierCreateExpectations.Handler0 handler)
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
			[(typeof(RockAttributeGenerator), "IParameterModifier_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateMakeWithRefParameterAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<IParameterModifier>]

			public interface IParameterModifier
			{
			    void Modify(ref string value);   
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
			
			internal sealed class IParameterModifierMakeExpectations
			{
				internal global::IParameterModifier Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::IParameterModifier
				{
					public Mock()
					{
					}
					
					public void Modify(ref string @value)
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
			[(typeof(RockAttributeGenerator), "IParameterModifier_Rock_Make.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateCreateWithRefReadonlyParameterAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<IParameterModifier>]

			public interface IParameterModifier
			{
			    void Modify(ref readonly string value);   
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
			
			internal sealed class IParameterModifierCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Action<string>>
				{
					public global::Rocks.Argument<string> @value { get; set; }
				}
				private global::Rocks.Handlers<global::IParameterModifierCreateExpectations.Handler0>? @handlers0;
				
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
					: global::IParameterModifier
				{
					public Mock(global::IParameterModifierCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void Modify(ref readonly string @value)
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
					
					private global::IParameterModifierCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IParameterModifierCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::IParameterModifierCreateExpectations.Adornments.AdornmentsForHandler0 Modify(global::Rocks.Argument<string> @value)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@value);
						
						var @handler = new global::IParameterModifierCreateExpectations.Handler0
						{
							@value = @value,
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					private global::IParameterModifierCreateExpectations Expectations { get; }
				}
				
				internal global::IParameterModifierCreateExpectations.MethodExpectations Methods { get; }
				
				internal IParameterModifierCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IParameterModifier Instance()
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
					public interface IAdornmentsForIParameterModifier<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIParameterModifier<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::IParameterModifierCreateExpectations.Handler0, global::System.Action<string>>, IAdornmentsForIParameterModifier<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::IParameterModifierCreateExpectations.Handler0 handler)
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
			[(typeof(RockAttributeGenerator), "IParameterModifier_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateMakeWithRefReadonlyParameterAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<IParameterModifier>]

			public interface IParameterModifier
			{
			    void Modify(ref readonly string value);   
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
			
			internal sealed class IParameterModifierMakeExpectations
			{
				internal global::IParameterModifier Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::IParameterModifier
				{
					public Mock()
					{
					}
					
					public void Modify(ref readonly string @value)
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
			[(typeof(RockAttributeGenerator), "IParameterModifier_Rock_Make.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateCreateWithMixtureAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<IParameterModifier>]

			public interface IParameterModifier
			{
				void RefArgument(ref int a);
				void RefArgumentsWithGenerics<T1, T2>(T1 a, ref T2 b);
				void OutArgument(out int a);
				void OutArgumentsWithGenerics<T1, T2>(T1 a, out T2 b);
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
			
			internal sealed class IParameterModifierCreateExpectations
				: global::Rocks.Expectations
			{
				internal static class Projections
				{
					internal delegate void Callback_640457111802933967433135011802939136029252304196(ref int @a);
					internal delegate void Callback_595359563624402850359397691594068887527805892984<T1, T2>(T1 @a, ref T2 @b);
					internal delegate void Callback_360038979746784102455204459622175388488035293924(out int @a);
					internal delegate void Callback_376795512182245354180779374814988822523603172187<T1, T2>(T1 @a, out T2 @b);
				}
				
				internal sealed class Handler0
					: global::Rocks.Handler<global::IParameterModifierCreateExpectations.Projections.Callback_640457111802933967433135011802939136029252304196>
				{
					public global::Rocks.Argument<int> @a { get; set; }
				}
				private global::Rocks.Handlers<global::IParameterModifierCreateExpectations.Handler0>? @handlers0;
				internal sealed class Handler1<T1, T2>
					: global::Rocks.Handler<global::IParameterModifierCreateExpectations.Projections.Callback_595359563624402850359397691594068887527805892984<T1, T2>>
				{
					public global::Rocks.Argument<T1> @a { get; set; }
					public global::Rocks.Argument<T2> @b { get; set; }
				}
				private global::Rocks.Handlers<global::Rocks.Handler>? @handlers1;
				internal sealed class Handler2
					: global::Rocks.Handler<global::IParameterModifierCreateExpectations.Projections.Callback_360038979746784102455204459622175388488035293924>
				{
					public global::Rocks.Argument<int> @a { get; set; }
				}
				private global::Rocks.Handlers<global::IParameterModifierCreateExpectations.Handler2>? @handlers2;
				internal sealed class Handler3<T1, T2>
					: global::Rocks.Handler<global::IParameterModifierCreateExpectations.Projections.Callback_376795512182245354180779374814988822523603172187<T1, T2>>
				{
					public global::Rocks.Argument<T1> @a { get; set; }
					public global::Rocks.Argument<T2> @b { get; set; }
				}
				private global::Rocks.Handlers<global::Rocks.Handler>? @handlers3;
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						if (this.handlers0 is not null) { failures.AddRange(this.Verify(this.handlers0, 0)); }
						if (this.handlers1 is not null) { failures.AddRange(this.Verify(this.handlers1, 1)); }
						if (this.handlers2 is not null) { failures.AddRange(this.Verify(this.handlers2, 2)); }
						if (this.handlers3 is not null) { failures.AddRange(this.Verify(this.handlers3, 3)); }
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class Mock
					: global::IParameterModifier
				{
					public Mock(global::IParameterModifierCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void RefArgument(ref int @a)
					{
						if (this.Expectations.handlers0 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @handler in this.Expectations.handlers0)
							{
								if (@handler.@a.IsValid(@a!))
								{
									@foundMatch = true;
									@handler.CallCount++;
									@handler.Callback?.Invoke(ref @a!);
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
					
					[global::Rocks.MemberIdentifier(1)]
					public void RefArgumentsWithGenerics<T1, T2>(T1 @a, ref T2 @b)
					{
						if (this.Expectations.handlers1 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @genericHandler in this.Expectations.handlers1)
							{
								if (@genericHandler is global::IParameterModifierCreateExpectations.Handler1<T1, T2> @handler)
								{
									if (@handler.@a.IsValid(@a!) &&
										@handler.@b.IsValid(@b!))
									{
										@foundMatch = true;
										@handler.CallCount++;
										@handler.Callback?.Invoke(@a!, ref @b!);
										break;
									}
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(1)}");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(1)}");
						}
					}
					
					[global::Rocks.MemberIdentifier(2)]
					public void OutArgument(out int @a)
					{
						a = default!;
						if (this.Expectations.handlers2 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @handler in this.Expectations.handlers2)
							{
								if (@handler.@a.IsValid(@a!))
								{
									@foundMatch = true;
									@handler.CallCount++;
									@handler.Callback?.Invoke(out @a!);
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(2)}");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(2)}");
						}
					}
					
					[global::Rocks.MemberIdentifier(3)]
					public void OutArgumentsWithGenerics<T1, T2>(T1 @a, out T2 @b)
					{
						b = default!;
						if (this.Expectations.handlers3 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @genericHandler in this.Expectations.handlers3)
							{
								if (@genericHandler is global::IParameterModifierCreateExpectations.Handler3<T1, T2> @handler)
								{
									if (@handler.@a.IsValid(@a!) &&
										@handler.@b.IsValid(@b!))
									{
										@foundMatch = true;
										@handler.CallCount++;
										@handler.Callback?.Invoke(@a!, out @b!);
										break;
									}
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(3)}");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(3)}");
						}
					}
					
					private global::IParameterModifierCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IParameterModifierCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::IParameterModifierCreateExpectations.Adornments.AdornmentsForHandler0 RefArgument(global::Rocks.Argument<int> @a)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@a);
						
						var @handler = new global::IParameterModifierCreateExpectations.Handler0
						{
							@a = @a,
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					internal global::IParameterModifierCreateExpectations.Adornments.AdornmentsForHandler1<T1, T2> RefArgumentsWithGenerics<T1, T2>(global::Rocks.Argument<T1> @a, global::Rocks.Argument<T2> @b)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@a);
						global::System.ArgumentNullException.ThrowIfNull(@b);
						
						var @handler = new global::IParameterModifierCreateExpectations.Handler1<T1, T2>
						{
							@a = @a,
							@b = @b,
						};
						
						if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(@handler); }
						else { this.Expectations.handlers1.Add(@handler); }
						return new(@handler);
					}
					
					internal global::IParameterModifierCreateExpectations.Adornments.AdornmentsForHandler2 OutArgument(global::Rocks.Argument<int> @a)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@a);
						
						var @handler = new global::IParameterModifierCreateExpectations.Handler2
						{
							@a = global::Rocks.Arg.Any<int>(),
						};
						
						if (this.Expectations.handlers2 is null) { this.Expectations.handlers2 = new(@handler); }
						else { this.Expectations.handlers2.Add(@handler); }
						return new(@handler);
					}
					
					internal global::IParameterModifierCreateExpectations.Adornments.AdornmentsForHandler3<T1, T2> OutArgumentsWithGenerics<T1, T2>(global::Rocks.Argument<T1> @a, global::Rocks.Argument<T2> @b)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@a);
						global::System.ArgumentNullException.ThrowIfNull(@b);
						
						var @handler = new global::IParameterModifierCreateExpectations.Handler3<T1, T2>
						{
							@a = @a,
							@b = global::Rocks.Arg.Any<T2>(),
						};
						
						if (this.Expectations.handlers3 is null) { this.Expectations.handlers3 = new(@handler); }
						else { this.Expectations.handlers3.Add(@handler); }
						return new(@handler);
					}
					
					private global::IParameterModifierCreateExpectations Expectations { get; }
				}
				
				internal global::IParameterModifierCreateExpectations.MethodExpectations Methods { get; }
				
				internal IParameterModifierCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IParameterModifier Instance()
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
					public interface IAdornmentsForIParameterModifier<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIParameterModifier<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::IParameterModifierCreateExpectations.Handler0, global::IParameterModifierCreateExpectations.Projections.Callback_640457111802933967433135011802939136029252304196>, IAdornmentsForIParameterModifier<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::IParameterModifierCreateExpectations.Handler0 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler1<T1, T2>
						: global::Rocks.Adornments<AdornmentsForHandler1<T1, T2>, global::IParameterModifierCreateExpectations.Handler1<T1, T2>, global::IParameterModifierCreateExpectations.Projections.Callback_595359563624402850359397691594068887527805892984<T1, T2>>, IAdornmentsForIParameterModifier<AdornmentsForHandler1<T1, T2>>
					{
						public AdornmentsForHandler1(global::IParameterModifierCreateExpectations.Handler1<T1, T2> handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler2
						: global::Rocks.Adornments<AdornmentsForHandler2, global::IParameterModifierCreateExpectations.Handler2, global::IParameterModifierCreateExpectations.Projections.Callback_360038979746784102455204459622175388488035293924>, IAdornmentsForIParameterModifier<AdornmentsForHandler2>
					{
						public AdornmentsForHandler2(global::IParameterModifierCreateExpectations.Handler2 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler3<T1, T2>
						: global::Rocks.Adornments<AdornmentsForHandler3<T1, T2>, global::IParameterModifierCreateExpectations.Handler3<T1, T2>, global::IParameterModifierCreateExpectations.Projections.Callback_376795512182245354180779374814988822523603172187<T1, T2>>, IAdornmentsForIParameterModifier<AdornmentsForHandler3<T1, T2>>
					{
						public AdornmentsForHandler3(global::IParameterModifierCreateExpectations.Handler3<T1, T2> handler)
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
			[(typeof(RockAttributeGenerator), "IParameterModifier_Rock_Create.g.cs", generatedCode)],
			[]);
	}
}