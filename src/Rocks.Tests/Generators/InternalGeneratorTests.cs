﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class InternalGeneratorTests
{
	[Test]
	public static async Task GenerateWhenTypeParameterIsPublicAsync()
	{
		var source =
			"""
			using System.Runtime.CompilerServices;

			[assembly: InternalsVisibleTo("TestProject")]
			
			public sealed record DataStuff;

			internal interface IDoStuff
			{
			  void Do(DataStuff dataStuff);
			}
			""";
		var sourceReferences = Shared.References.Value
			.Cast<MetadataReference>()
			.ToList();
		var sourceSyntaxTree = CSharpSyntaxTree.ParseText(source);
		var sourceCompilation = CSharpCompilation.Create("Source", [sourceSyntaxTree],
			sourceReferences,
			new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var sourceReference = sourceCompilation.ToMetadataReference()!;
		sourceReferences.Add(sourceReference);

		var code =
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IDoStuff), BuildType.Create | BuildType.Make)]
			""";

		var createGeneratedCode =
			""""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class IDoStuffCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Action<global::DataStuff>>
				{
					public global::Rocks.Argument<global::DataStuff> @dataStuff { get; set; }
				}
				private global::Rocks.Handlers<global::IDoStuffCreateExpectations.Handler0>? @handlers0;
				
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
					: global::IDoStuff
				{
					public Mock(global::IDoStuffCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void Do(global::DataStuff @dataStuff)
					{
						if (this.Expectations.handlers0 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @handler in this.Expectations.handlers0)
							{
								if (@handler.@dataStuff.IsValid(@dataStuff!))
								{
									@foundMatch = true;
									@handler.CallCount++;
									@handler.Callback?.Invoke(@dataStuff!);
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException(
									$"""
									No handlers match for {this.GetType().GetMemberDescription(0)}
										dataStuff: {@dataStuff.FormatValue()}
									""");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException(
								$"""
								No handlers were found for {this.GetType().GetMemberDescription(0)}
									dataStuff: {@dataStuff.FormatValue()}
								""");
						}
					}
					
					private global::IDoStuffCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IDoStuffCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::IDoStuffCreateExpectations.Adornments.AdornmentsForHandler0 Do(global::Rocks.Argument<global::DataStuff> @dataStuff)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@dataStuff);
						
						var @handler = new global::IDoStuffCreateExpectations.Handler0
						{
							@dataStuff = @dataStuff,
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					private global::IDoStuffCreateExpectations Expectations { get; }
				}
				
				internal global::IDoStuffCreateExpectations.MethodExpectations Methods { get; }
				
				internal IDoStuffCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IDoStuff Instance()
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
					public interface IAdornmentsForIDoStuff<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIDoStuff<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::IDoStuffCreateExpectations.Handler0, global::System.Action<global::DataStuff>>, IAdornmentsForIDoStuff<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::IDoStuffCreateExpectations.Handler0 handler)
							: base(handler) { }
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			"""";
		
		var makeGeneratedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class IDoStuffMakeExpectations
			{
				internal global::IDoStuff Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::IDoStuff
				{
					public Mock()
					{
					}
					
					public void Do(global::DataStuff @dataStuff)
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
				("IDoStuff_Rock_Create.g.cs", createGeneratedCode),
				("IDoStuff_Rock_Make.g.cs", makeGeneratedCode)
			],
			[],
			additionalReferences: sourceReferences);
	}

	[Test]
	public static async Task GenerateWhenTypeParameterIsInternalAsync()
	{
		var source =
			"""
			using System.Runtime.CompilerServices;

			[assembly: InternalsVisibleTo("TestProject")]
			
			internal sealed record DataStuff;

			internal interface IDoStuff
			{
			  void Do(DataStuff dataStuff);
			}
			""";
		var sourceReferences = Shared.References.Value
			.Cast<MetadataReference>()
			.ToList();
		var sourceSyntaxTree = CSharpSyntaxTree.ParseText(source);
		var sourceCompilation = CSharpCompilation.Create("Source", [sourceSyntaxTree],
			sourceReferences,
			new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var sourceReference = sourceCompilation.ToMetadataReference()!;
		sourceReferences.Add(sourceReference);

		var code =
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IDoStuff), BuildType.Create | BuildType.Make)]
			""";

		var createGeneratedCode =
			""""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class IDoStuffCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Action<global::DataStuff>>
				{
					public global::Rocks.Argument<global::DataStuff> @dataStuff { get; set; }
				}
				private global::Rocks.Handlers<global::IDoStuffCreateExpectations.Handler0>? @handlers0;
				
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
					: global::IDoStuff
				{
					public Mock(global::IDoStuffCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void Do(global::DataStuff @dataStuff)
					{
						if (this.Expectations.handlers0 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @handler in this.Expectations.handlers0)
							{
								if (@handler.@dataStuff.IsValid(@dataStuff!))
								{
									@foundMatch = true;
									@handler.CallCount++;
									@handler.Callback?.Invoke(@dataStuff!);
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException(
									$"""
									No handlers match for {this.GetType().GetMemberDescription(0)}
										dataStuff: {@dataStuff.FormatValue()}
									""");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException(
								$"""
								No handlers were found for {this.GetType().GetMemberDescription(0)}
									dataStuff: {@dataStuff.FormatValue()}
								""");
						}
					}
					
					private global::IDoStuffCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IDoStuffCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::IDoStuffCreateExpectations.Adornments.AdornmentsForHandler0 Do(global::Rocks.Argument<global::DataStuff> @dataStuff)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@dataStuff);
						
						var @handler = new global::IDoStuffCreateExpectations.Handler0
						{
							@dataStuff = @dataStuff,
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					private global::IDoStuffCreateExpectations Expectations { get; }
				}
				
				internal global::IDoStuffCreateExpectations.MethodExpectations Methods { get; }
				
				internal IDoStuffCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IDoStuff Instance()
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
					public interface IAdornmentsForIDoStuff<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIDoStuff<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::IDoStuffCreateExpectations.Handler0, global::System.Action<global::DataStuff>>, IAdornmentsForIDoStuff<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::IDoStuffCreateExpectations.Handler0 handler)
							: base(handler) { }
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			"""";
		
		var makeGeneratedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class IDoStuffMakeExpectations
			{
				internal global::IDoStuff Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::IDoStuff
				{
					public Mock()
					{
					}
					
					public void Do(global::DataStuff @dataStuff)
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
				("IDoStuff_Rock_Create.g.cs", createGeneratedCode),
				("IDoStuff_Rock_Make.g.cs", makeGeneratedCode)
			],
			[],
			additionalReferences: sourceReferences);
	}
}