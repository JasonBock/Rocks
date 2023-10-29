using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Diagnostics;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "ROCK10:Member Is Obsolete", Scope = "module")]

namespace Rocks.Tests.Generators;

public static class ObsoleteGeneratorTests
{
	[Test]
	public static async Task GenerateWhenAMethodAndItsPartsAreObsoleteAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Collections;
			using System.Collections.Generic;
			using System.Linq;
									
			#nullable enable

			namespace MockTests
			{
				[Obsolete("Do not use this type", true)]
				public interface IAmObsolete
				{
					void Execute();
				}

				public interface IAmAlsoObsolete
				{
					[Obsolete("Do not use this method", true)]
					void Use(IAmObsolete old);
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IAmAlsoObsolete>();
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			Enumerable.Empty<(Type, string, string)>(),
			new[]
			{
				new DiagnosticResult("ROCK10", DiagnosticSeverity.Error)
					.WithSpan(27, 15, 27, 45),
				new DiagnosticResult("ROCK9", DiagnosticSeverity.Error)
					.WithSpan(27, 15, 27, 45)
			},
			generalDiagnosticOption: ReportDiagnostic.Error,
			disabledDiagnostics: new List<string> { "CS1591" }).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenAPropertyAndItsTypeAreObsoleteAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Collections;
			using System.Collections.Generic;
			using System.Linq;
									
			#nullable enable

			namespace MockTests
			{
				[Obsolete("Do not use this type", true)]
				public interface IAmObsolete
				{
					void Execute();
				}

				public interface IAmAlsoObsolete
				{
					[Obsolete("Do not use this method", true)]
					IAmObsolete Use { get; }
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IAmAlsoObsolete>();
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			Enumerable.Empty<(Type, string, string)>(),
			new[]
			{
				new DiagnosticResult("ROCK10", DiagnosticSeverity.Error)
					.WithSpan(27, 15, 27, 45),
				new DiagnosticResult("ROCK9", DiagnosticSeverity.Error)
					.WithSpan(27, 15, 27, 45)
			},
			generalDiagnosticOption: ReportDiagnostic.Error,
			disabledDiagnostics: new List<string> { "CS1591" }).ConfigureAwait(false);
	}

	[Test]
	public static async Task CreateWhenGenericContainsObsoleteTypeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Collections;
			using System.Collections.Generic;
			using System.Linq;
									
			#nullable enable

			namespace MockTests
			{
				[Obsolete("Do not use this", true)]
				public interface IServerComponent
				{
					void Execute();
				}

				public abstract class JobStorage
				{
					public virtual IEnumerable<IServerComponent> GetComponents() =>
						Enumerable.Empty<IServerComponent>();
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<JobStorage>();
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			Enumerable.Empty<(Type, string, string)>(),
			new[] 
			{
				new DiagnosticResult("CS0619", DiagnosticSeverity.Error)
					.WithSpan(19, 30, 19, 46).WithArguments("MockTests.IServerComponent", "Do not use this"),
				new DiagnosticResult(MemberUsesObsoleteTypeDiagnostic.Id, DiagnosticSeverity.Error)
					.WithSpan(27, 15, 27, 40),
				new DiagnosticResult("CS0619", DiagnosticSeverity.Error)
					.WithSpan(20, 21, 20, 37).WithArguments("MockTests.IServerComponent", "Do not use this"),
			},
			generalDiagnosticOption: ReportDiagnostic.Error,
			disabledDiagnostics: new List<string> { "CS1591" }).ConfigureAwait(false);
	}

	[Test]
	public static async Task CreateWhenTargetHasObsoleteMembersAsWarningAndBuildIsErrorAndROCK10IsDisabledAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			#nullable enable

			namespace MockTests
			{
				public interface IHaveObsoleteMembers
				{
					[Obsolete("This method is not intended to be used directly by user code")]
					int GetObsoleteValue();
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IHaveObsoleteMembers>();
					}
				}
			}
			""";

		var generatedCode = "this isn't what it should be";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "MockTests.IHaveObsoleteMembers_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>(),
			generalDiagnosticOption: ReportDiagnostic.Error,
			disabledDiagnostics: new List<string> { "CS1591", "ROCK10" }).ConfigureAwait(false);
	}

	[Test]
	public static async Task CreateWhenTargetHasObsoleteMembersAsWarningsAndBuildIsErrorAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			#nullable enable

			namespace MockTests
			{
				public interface IPixelShader
				{
					[Obsolete("This method is not intended to be used directly by user code")]
					uint GetPixelOptions();

					[Obsolete("This property is not intended to be used directly by user code")]
					uint Values { get; }

					[Obsolete("This event is not intended to be used directly by user code")]
					event EventHandler ShadingOccurred;
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IPixelShader>();
					}
				}
			}
			""";

		var methodDiagnostic = new DiagnosticResult(MemberIsObsoleteDiagnostic.Id, DiagnosticSeverity.Error)
			.WithSpan(24, 15, 24, 42);
		var propertyDiagnostic = new DiagnosticResult(MemberIsObsoleteDiagnostic.Id, DiagnosticSeverity.Error)
			.WithSpan(24, 15, 24, 42);
		var eventDiagnostic = new DiagnosticResult(MemberIsObsoleteDiagnostic.Id, DiagnosticSeverity.Error)
			.WithSpan(24, 15, 24, 42);
		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			Enumerable.Empty<(Type, string, string)>(),
			new[] { methodDiagnostic, propertyDiagnostic, eventDiagnostic }, 
			generalDiagnosticOption: ReportDiagnostic.Error,
			disabledDiagnostics: new List<string> { "CS1591" }).ConfigureAwait(false);
	}

	[Test]
	public static async Task CreateWhenTargetHasObsoleteMembersAsWarningsAndBuildIsDefaultAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			#nullable enable
			
			namespace MockTests
			{
				public interface IPixelShader
				{
					[Obsolete("This method is not intended to be used directly by user code")]
					uint GetPixelOptions();
			
					[Obsolete("This property is not intended to be used directly by user code")]
					uint Values { get; }
			
					[Obsolete("This event is not intended to be used directly by user code")]
					event EventHandler ShadingOccurred;
				}
			
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IPixelShader>();
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
				internal static class CreateExpectationsOfIPixelShaderExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.IPixelShader> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.IPixelShader> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertyExpectations<global::MockTests.IPixelShader> Properties(this global::Rocks.Expectations.Expectations<global::MockTests.IPixelShader> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.IPixelShader> Getters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.IPixelShader> @self) =>
						new(@self);
					
					internal static global::MockTests.IPixelShader Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IPixelShader> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockIPixelShader(@self);
							@self.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIPixelShader
						: global::MockTests.IPixelShader, global::Rocks.IRaiseEvents
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIPixelShader(global::Rocks.Expectations.Expectations<global::MockTests.IPixelShader> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::System.ObsoleteAttribute("This method is not intended to be used directly by user code")]
						[global::Rocks.MemberIdentifier(0, "uint GetPixelOptions()")]
						public uint GetPixelOptions()
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									((global::System.Func<uint>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<uint>)@methodHandler).ReturnValue;
								@methodHandler.RaiseEvents(this);
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for uint GetPixelOptions()");
						}
						
						[global::System.ObsoleteAttribute("This property is not intended to be used directly by user code")]
						[global::Rocks.MemberIdentifier(1, "get_Values()")]
						public uint Values
						{
							get
							{
								if (this.handlers.TryGetValue(1, out var @methodHandlers))
								{
									var @methodHandler = @methodHandlers[0];
									@methodHandler.IncrementCallCount();
									var @result = @methodHandler.Method is not null ?
										((global::System.Func<uint>)@methodHandler.Method)() :
										((global::Rocks.HandlerInformation<uint>)@methodHandler).ReturnValue;
									@methodHandler.RaiseEvents(this);
									return @result!;
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_Values())");
							}
						}
						
						#pragma warning disable CS0067
						[global::System.ObsoleteAttribute("This event is not intended to be used directly by user code")]
						public event global::System.EventHandler? ShadingOccurred;
						#pragma warning restore CS0067
						
						void global::Rocks.IRaiseEvents.Raise(string @fieldName, object @args)
						{
							var @thisType = this.GetType();
							var @eventDelegate = (global::System.MulticastDelegate)thisType.GetField(@fieldName, 
								global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.NonPublic)!.GetValue(this)!;
							
							if (@eventDelegate is not null)
							{
								foreach (var @handler in @eventDelegate.GetInvocationList())
								{
									@handler.Method.Invoke(@handler.Target, new object[]{this, @args});
								}
							}
						}
					}
				}
				
				internal static class MethodExpectationsOfIPixelShaderExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IPixelShader, global::System.Func<uint>, uint> GetPixelOptions(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IPixelShader> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.IPixelShader, global::System.Func<uint>, uint>(@self.Add<uint>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				
				internal static class PropertyGetterExpectationsOfIPixelShaderExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.IPixelShader, global::System.Func<uint>, uint> Values(this global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.IPixelShader> @self) =>
						new global::Rocks.PropertyAdornments<global::MockTests.IPixelShader, global::System.Func<uint>, uint>(@self.Add<uint>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				
				internal static class MethodAdornmentsOfIPixelShaderExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IPixelShader, TCallback, TReturn> RaisesShadingOccurred<TCallback, TReturn>(this global::Rocks.MethodAdornments<global::MockTests.IPixelShader, TCallback, TReturn> @self, global::System.EventArgs @args)
						where TCallback : global::System.Delegate
					{
						@self.Handler.AddRaiseEvent(new("ShadingOccurred", @args));
						return @self;
					}
				}
				
				internal static class PropertyAdornmentsOfIPixelShaderExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.IPixelShader, TCallback, TReturn> RaisesShadingOccurred<TCallback, TReturn>(this global::Rocks.PropertyAdornments<global::MockTests.IPixelShader, TCallback, TReturn> @self, global::System.EventArgs @args)
						where TCallback : global::System.Delegate
					{
						@self.Handler.AddRaiseEvent(new("ShadingOccurred", @args));
						return @self;
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "MockTests.IPixelShader_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}