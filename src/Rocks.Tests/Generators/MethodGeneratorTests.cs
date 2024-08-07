﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class MethodGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
			"""
			using Rocks;
			
			[assembly: RockCreate<MockTests.IMember>]

			namespace MockTests
			{
				public interface IMember
				{
					void Exists();
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
				internal sealed class IMemberCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Action>
					{ }
					private global::Rocks.Handlers<global::MockTests.IMemberCreateExpectations.Handler0>? @handlers0;
					
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
						: global::MockTests.IMember
					{
						public Mock(global::MockTests.IMemberCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public void Exists()
						{
							if (this.Expectations.handlers0 is not null)
							{
								var @handler = this.Expectations.handlers0.First;
								@handler.CallCount++;
								@handler.Callback?.Invoke();
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
							}
						}
						
						private global::MockTests.IMemberCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.IMemberCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.IMemberCreateExpectations.Adornments.AdornmentsForHandler0 Exists()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.IMemberCreateExpectations.Handler0();
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(handler); }
							else { this.Expectations.handlers0.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.IMemberCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IMemberCreateExpectations.MethodExpectations Methods { get; }
					
					internal IMemberCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IMember Instance()
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
						public interface IAdornmentsForIMember<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIMember<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.IMemberCreateExpectations.Handler0, global::System.Action>, IAdornmentsForIMember<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.IMemberCreateExpectations.Handler0 handler)
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

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[(typeof(RockGenerator), "MockTests.IMember_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateWithOptionalParametersAndParamsAsync()
	{
		var code =
			"""
			using Rocks;
			using System.Collections.Generic;
			using System;
			using System.Linq;
			using System.Linq.Expressions;
			
			[assembly: RockCreate<MockTests.IMapper>]

			namespace MockTests
			{
				public interface IMapper
				{
					IQueryable<TDestination> ProjectTo<TDestination>(
						IQueryable source, object parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand);
					IQueryable<TDestination> ProjectTo<TDestination>(
						IQueryable source, IDictionary<string, object> parameters, params string[] membersToExpand);
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
				internal sealed class IMapperCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0<TDestination>
						: global::Rocks.Handler<global::System.Func<global::System.Linq.IQueryable, object?, global::System.Linq.Expressions.Expression<global::System.Func<TDestination, object>>[], global::System.Linq.IQueryable<TDestination>>, global::System.Linq.IQueryable<TDestination>>
					{
						public global::Rocks.Argument<global::System.Linq.IQueryable> @source { get; set; }
						public global::Rocks.Argument<object?> @parameters { get; set; }
						public global::Rocks.Argument<global::System.Linq.Expressions.Expression<global::System.Func<TDestination, object>>[]> @membersToExpand { get; set; }
					}
					private global::Rocks.Handlers<global::Rocks.Handler>? @handlers0;
					internal sealed class Handler1<TDestination>
						: global::Rocks.Handler<global::System.Func<global::System.Linq.IQueryable, global::System.Collections.Generic.IDictionary<string, object>, string[], global::System.Linq.IQueryable<TDestination>>, global::System.Linq.IQueryable<TDestination>>
					{
						public global::Rocks.Argument<global::System.Linq.IQueryable> @source { get; set; }
						public global::Rocks.Argument<global::System.Collections.Generic.IDictionary<string, object>> @parameters { get; set; }
						public global::Rocks.Argument<string[]> @membersToExpand { get; set; }
					}
					private global::Rocks.Handlers<global::Rocks.Handler>? @handlers1;
					
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
						: global::MockTests.IMapper
					{
						public Mock(global::MockTests.IMapperCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public global::System.Linq.IQueryable<TDestination> ProjectTo<TDestination>(global::System.Linq.IQueryable @source, object? @parameters = null, params global::System.Linq.Expressions.Expression<global::System.Func<TDestination, object>>[] @membersToExpand)
						{
							if (this.Expectations.handlers0 is not null)
							{
								foreach (var @genericHandler in this.Expectations.handlers0)
								{
									if (@genericHandler is global::MockTests.IMapperCreateExpectations.Handler0<TDestination> @handler)
									{
										if (@handler.@source.IsValid(@source!) &&
											@handler.@parameters.IsValid(@parameters!) &&
											@handler.@membersToExpand.IsValid(@membersToExpand!))
										{
											@handler.CallCount++;
											var @result = @handler.Callback is not null ?
												@handler.Callback(@source!, @parameters!, @membersToExpand!) : @handler.ReturnValue;
											return @result!;
										}
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(0)}");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
						}
						
						[global::Rocks.MemberIdentifier(1)]
						public global::System.Linq.IQueryable<TDestination> ProjectTo<TDestination>(global::System.Linq.IQueryable @source, global::System.Collections.Generic.IDictionary<string, object> @parameters, params string[] @membersToExpand)
						{
							if (this.Expectations.handlers1 is not null)
							{
								foreach (var @genericHandler in this.Expectations.handlers1)
								{
									if (@genericHandler is global::MockTests.IMapperCreateExpectations.Handler1<TDestination> @handler)
									{
										if (@handler.@source.IsValid(@source!) &&
											@handler.@parameters.IsValid(@parameters!) &&
											@handler.@membersToExpand.IsValid(@membersToExpand!))
										{
											@handler.CallCount++;
											var @result = @handler.Callback is not null ?
												@handler.Callback(@source!, @parameters!, @membersToExpand!) : @handler.ReturnValue;
											return @result!;
										}
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(1)}");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(1)}");
						}
						
						private global::MockTests.IMapperCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.IMapperCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.IMapperCreateExpectations.Adornments.AdornmentsForHandler0<TDestination> ProjectTo<TDestination>(global::Rocks.Argument<global::System.Linq.IQueryable> @source, global::Rocks.Argument<object?> @parameters, global::Rocks.Argument<global::System.Linq.Expressions.Expression<global::System.Func<TDestination, object>>[]> @membersToExpand)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@source);
							global::System.ArgumentNullException.ThrowIfNull(@parameters);
							global::System.ArgumentNullException.ThrowIfNull(@membersToExpand);
							
							var @handler = new global::MockTests.IMapperCreateExpectations.Handler0<TDestination>
							{
								@source = @source,
								@parameters = @parameters.Transform(null),
								@membersToExpand = @membersToExpand,
							};
							
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						internal global::MockTests.IMapperCreateExpectations.Adornments.AdornmentsForHandler0<TDestination> ProjectTo<TDestination>(global::Rocks.Argument<global::System.Linq.IQueryable> @source, object? @parameters = null, params global::System.Linq.Expressions.Expression<global::System.Func<TDestination, object>>[] @membersToExpand) =>
							this.ProjectTo<TDestination>(@source, global::Rocks.Arg.Is(@parameters), global::Rocks.Arg.Is(@membersToExpand));
						
						internal global::MockTests.IMapperCreateExpectations.Adornments.AdornmentsForHandler1<TDestination> ProjectTo<TDestination>(global::Rocks.Argument<global::System.Linq.IQueryable> @source, global::Rocks.Argument<global::System.Collections.Generic.IDictionary<string, object>> @parameters, global::Rocks.Argument<string[]> @membersToExpand)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@source);
							global::System.ArgumentNullException.ThrowIfNull(@parameters);
							global::System.ArgumentNullException.ThrowIfNull(@membersToExpand);
							
							var @handler = new global::MockTests.IMapperCreateExpectations.Handler1<TDestination>
							{
								@source = @source,
								@parameters = @parameters,
								@membersToExpand = @membersToExpand,
							};
							
							if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(@handler); }
							else { this.Expectations.handlers1.Add(@handler); }
							return new(@handler);
						}
						
						private global::MockTests.IMapperCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IMapperCreateExpectations.MethodExpectations Methods { get; }
					
					internal IMapperCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IMapper Instance()
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
						public interface IAdornmentsForIMapper<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIMapper<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0<TDestination>
							: global::Rocks.Adornments<AdornmentsForHandler0<TDestination>, global::MockTests.IMapperCreateExpectations.Handler0<TDestination>, global::System.Func<global::System.Linq.IQueryable, object?, global::System.Linq.Expressions.Expression<global::System.Func<TDestination, object>>[], global::System.Linq.IQueryable<TDestination>>, global::System.Linq.IQueryable<TDestination>>, IAdornmentsForIMapper<AdornmentsForHandler0<TDestination>>
						{
							public AdornmentsForHandler0(global::MockTests.IMapperCreateExpectations.Handler0<TDestination> handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler1<TDestination>
							: global::Rocks.Adornments<AdornmentsForHandler1<TDestination>, global::MockTests.IMapperCreateExpectations.Handler1<TDestination>, global::System.Func<global::System.Linq.IQueryable, global::System.Collections.Generic.IDictionary<string, object>, string[], global::System.Linq.IQueryable<TDestination>>, global::System.Linq.IQueryable<TDestination>>, IAdornmentsForIMapper<AdornmentsForHandler1<TDestination>>
						{
							public AdornmentsForHandler1(global::MockTests.IMapperCreateExpectations.Handler1<TDestination> handler)
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

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[(typeof(RockGenerator), "MockTests.IMapper_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateWhenOptionalArgumentsExistAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<MockTests.IHaveOptionalArguments>]

			namespace MockTests
			{
				public interface IHaveOptionalArguments
				{
					void Foo(int a, string b = "b", double c = 3.2);
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
				internal sealed class IHaveOptionalArgumentsCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Action<int, string, double>>
					{
						public global::Rocks.Argument<int> @a { get; set; }
						public global::Rocks.Argument<string> @b { get; set; }
						public global::Rocks.Argument<double> @c { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IHaveOptionalArgumentsCreateExpectations.Handler0>? @handlers0;
					
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
						: global::MockTests.IHaveOptionalArguments
					{
						public Mock(global::MockTests.IHaveOptionalArgumentsCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public void Foo(int @a, string @b = "b", double @c = 3.2)
						{
							if (this.Expectations.handlers0 is not null)
							{
								var @foundMatch = false;
								
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@a.IsValid(@a!) &&
										@handler.@b.IsValid(@b!) &&
										@handler.@c.IsValid(@c!))
									{
										@foundMatch = true;
										@handler.CallCount++;
										@handler.Callback?.Invoke(@a!, @b!, @c!);
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
						
						private global::MockTests.IHaveOptionalArgumentsCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.IHaveOptionalArgumentsCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.IHaveOptionalArgumentsCreateExpectations.Adornments.AdornmentsForHandler0 Foo(global::Rocks.Argument<int> @a, global::Rocks.Argument<string> @b, global::Rocks.Argument<double> @c)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@a);
							global::System.ArgumentNullException.ThrowIfNull(@b);
							global::System.ArgumentNullException.ThrowIfNull(@c);
							
							var @handler = new global::MockTests.IHaveOptionalArgumentsCreateExpectations.Handler0
							{
								@a = @a,
								@b = @b.Transform("b"),
								@c = @c.Transform(3.2),
							};
							
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						internal global::MockTests.IHaveOptionalArgumentsCreateExpectations.Adornments.AdornmentsForHandler0 Foo(global::Rocks.Argument<int> @a, string @b = "b", double @c = 3.2) =>
							this.Foo(@a, global::Rocks.Arg.Is(@b), global::Rocks.Arg.Is(@c));
						
						private global::MockTests.IHaveOptionalArgumentsCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IHaveOptionalArgumentsCreateExpectations.MethodExpectations Methods { get; }
					
					internal IHaveOptionalArgumentsCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IHaveOptionalArguments Instance()
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
						public interface IAdornmentsForIHaveOptionalArguments<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIHaveOptionalArguments<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.IHaveOptionalArgumentsCreateExpectations.Handler0, global::System.Action<int, string, double>>, IAdornmentsForIHaveOptionalArguments<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.IHaveOptionalArgumentsCreateExpectations.Handler0 handler)
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

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[(typeof(RockGenerator), "MockTests.IHaveOptionalArguments_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateWhenOptionalArgumentsAndParamsExistAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockCreate<MockTests.IProjection>]

			namespace MockTests
			{
				public interface IProjection
				{
					 void Project(string a, int b = 22, params Guid[] values);
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
				internal sealed class IProjectionCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Action<string, int, global::System.Guid[]>>
					{
						public global::Rocks.Argument<string> @a { get; set; }
						public global::Rocks.Argument<int> @b { get; set; }
						public global::Rocks.Argument<global::System.Guid[]> @values { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IProjectionCreateExpectations.Handler0>? @handlers0;
					
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
						: global::MockTests.IProjection
					{
						public Mock(global::MockTests.IProjectionCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public void Project(string @a, int @b = 22, params global::System.Guid[] @values)
						{
							if (this.Expectations.handlers0 is not null)
							{
								var @foundMatch = false;
								
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@a.IsValid(@a!) &&
										@handler.@b.IsValid(@b!) &&
										@handler.@values.IsValid(@values!))
									{
										@foundMatch = true;
										@handler.CallCount++;
										@handler.Callback?.Invoke(@a!, @b!, @values!);
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
						
						private global::MockTests.IProjectionCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.IProjectionCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.IProjectionCreateExpectations.Adornments.AdornmentsForHandler0 Project(global::Rocks.Argument<string> @a, global::Rocks.Argument<int> @b, global::Rocks.Argument<global::System.Guid[]> @values)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@a);
							global::System.ArgumentNullException.ThrowIfNull(@b);
							global::System.ArgumentNullException.ThrowIfNull(@values);
							
							var @handler = new global::MockTests.IProjectionCreateExpectations.Handler0
							{
								@a = @a,
								@b = @b.Transform(22),
								@values = @values,
							};
							
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						internal global::MockTests.IProjectionCreateExpectations.Adornments.AdornmentsForHandler0 Project(global::Rocks.Argument<string> @a, int @b = 22, params global::System.Guid[] @values) =>
							this.Project(@a, global::Rocks.Arg.Is(@b), global::Rocks.Arg.Is(@values));
						
						private global::MockTests.IProjectionCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IProjectionCreateExpectations.MethodExpectations Methods { get; }
					
					internal IProjectionCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IProjection Instance()
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
						public interface IAdornmentsForIProjection<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIProjection<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.IProjectionCreateExpectations.Handler0, global::System.Action<string, int, global::System.Guid[]>>, IAdornmentsForIProjection<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.IProjectionCreateExpectations.Handler0 handler)
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

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[(typeof(RockGenerator), "MockTests.IProjection_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateWhenMethodHasOver16ParametersAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<MockTests.IHaveTooMuch>]

			namespace MockTests
			{
				public interface IHaveTooMuch
				{
					int AddProperty(
						int i0, int i1, int i2, int i3, int i4,
						int i5, int i6, int i7, int i8, int i9,
						int i10, int i11, int i12, int i13, int i14,
						int i15, int i16, int i17, int i18, int i19);
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
				internal sealed class IHaveTooMuchCreateExpectations
					: global::Rocks.Expectations
				{
					internal static class Projections
					{
						internal delegate int Callback_383140697323744298072430331353344056628280456971(int @i0, int @i1, int @i2, int @i3, int @i4, int @i5, int @i6, int @i7, int @i8, int @i9, int @i10, int @i11, int @i12, int @i13, int @i14, int @i15, int @i16, int @i17, int @i18, int @i19);
					}
					
					internal sealed class Handler0
						: global::Rocks.Handler<global::MockTests.IHaveTooMuchCreateExpectations.Projections.Callback_383140697323744298072430331353344056628280456971, int>
					{
						public global::Rocks.Argument<int> @i0 { get; set; }
						public global::Rocks.Argument<int> @i1 { get; set; }
						public global::Rocks.Argument<int> @i2 { get; set; }
						public global::Rocks.Argument<int> @i3 { get; set; }
						public global::Rocks.Argument<int> @i4 { get; set; }
						public global::Rocks.Argument<int> @i5 { get; set; }
						public global::Rocks.Argument<int> @i6 { get; set; }
						public global::Rocks.Argument<int> @i7 { get; set; }
						public global::Rocks.Argument<int> @i8 { get; set; }
						public global::Rocks.Argument<int> @i9 { get; set; }
						public global::Rocks.Argument<int> @i10 { get; set; }
						public global::Rocks.Argument<int> @i11 { get; set; }
						public global::Rocks.Argument<int> @i12 { get; set; }
						public global::Rocks.Argument<int> @i13 { get; set; }
						public global::Rocks.Argument<int> @i14 { get; set; }
						public global::Rocks.Argument<int> @i15 { get; set; }
						public global::Rocks.Argument<int> @i16 { get; set; }
						public global::Rocks.Argument<int> @i17 { get; set; }
						public global::Rocks.Argument<int> @i18 { get; set; }
						public global::Rocks.Argument<int> @i19 { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IHaveTooMuchCreateExpectations.Handler0>? @handlers0;
					
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
						: global::MockTests.IHaveTooMuch
					{
						public Mock(global::MockTests.IHaveTooMuchCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public int AddProperty(int @i0, int @i1, int @i2, int @i3, int @i4, int @i5, int @i6, int @i7, int @i8, int @i9, int @i10, int @i11, int @i12, int @i13, int @i14, int @i15, int @i16, int @i17, int @i18, int @i19)
						{
							if (this.Expectations.handlers0 is not null)
							{
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@i0.IsValid(@i0!) &&
										@handler.@i1.IsValid(@i1!) &&
										@handler.@i2.IsValid(@i2!) &&
										@handler.@i3.IsValid(@i3!) &&
										@handler.@i4.IsValid(@i4!) &&
										@handler.@i5.IsValid(@i5!) &&
										@handler.@i6.IsValid(@i6!) &&
										@handler.@i7.IsValid(@i7!) &&
										@handler.@i8.IsValid(@i8!) &&
										@handler.@i9.IsValid(@i9!) &&
										@handler.@i10.IsValid(@i10!) &&
										@handler.@i11.IsValid(@i11!) &&
										@handler.@i12.IsValid(@i12!) &&
										@handler.@i13.IsValid(@i13!) &&
										@handler.@i14.IsValid(@i14!) &&
										@handler.@i15.IsValid(@i15!) &&
										@handler.@i16.IsValid(@i16!) &&
										@handler.@i17.IsValid(@i17!) &&
										@handler.@i18.IsValid(@i18!) &&
										@handler.@i19.IsValid(@i19!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@i0!, @i1!, @i2!, @i3!, @i4!, @i5!, @i6!, @i7!, @i8!, @i9!, @i10!, @i11!, @i12!, @i13!, @i14!, @i15!, @i16!, @i17!, @i18!, @i19!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(0)}");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
						}
						
						private global::MockTests.IHaveTooMuchCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.IHaveTooMuchCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.IHaveTooMuchCreateExpectations.Adornments.AdornmentsForHandler0 AddProperty(global::Rocks.Argument<int> @i0, global::Rocks.Argument<int> @i1, global::Rocks.Argument<int> @i2, global::Rocks.Argument<int> @i3, global::Rocks.Argument<int> @i4, global::Rocks.Argument<int> @i5, global::Rocks.Argument<int> @i6, global::Rocks.Argument<int> @i7, global::Rocks.Argument<int> @i8, global::Rocks.Argument<int> @i9, global::Rocks.Argument<int> @i10, global::Rocks.Argument<int> @i11, global::Rocks.Argument<int> @i12, global::Rocks.Argument<int> @i13, global::Rocks.Argument<int> @i14, global::Rocks.Argument<int> @i15, global::Rocks.Argument<int> @i16, global::Rocks.Argument<int> @i17, global::Rocks.Argument<int> @i18, global::Rocks.Argument<int> @i19)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@i0);
							global::System.ArgumentNullException.ThrowIfNull(@i1);
							global::System.ArgumentNullException.ThrowIfNull(@i2);
							global::System.ArgumentNullException.ThrowIfNull(@i3);
							global::System.ArgumentNullException.ThrowIfNull(@i4);
							global::System.ArgumentNullException.ThrowIfNull(@i5);
							global::System.ArgumentNullException.ThrowIfNull(@i6);
							global::System.ArgumentNullException.ThrowIfNull(@i7);
							global::System.ArgumentNullException.ThrowIfNull(@i8);
							global::System.ArgumentNullException.ThrowIfNull(@i9);
							global::System.ArgumentNullException.ThrowIfNull(@i10);
							global::System.ArgumentNullException.ThrowIfNull(@i11);
							global::System.ArgumentNullException.ThrowIfNull(@i12);
							global::System.ArgumentNullException.ThrowIfNull(@i13);
							global::System.ArgumentNullException.ThrowIfNull(@i14);
							global::System.ArgumentNullException.ThrowIfNull(@i15);
							global::System.ArgumentNullException.ThrowIfNull(@i16);
							global::System.ArgumentNullException.ThrowIfNull(@i17);
							global::System.ArgumentNullException.ThrowIfNull(@i18);
							global::System.ArgumentNullException.ThrowIfNull(@i19);
							
							var @handler = new global::MockTests.IHaveTooMuchCreateExpectations.Handler0
							{
								@i0 = @i0,
								@i1 = @i1,
								@i2 = @i2,
								@i3 = @i3,
								@i4 = @i4,
								@i5 = @i5,
								@i6 = @i6,
								@i7 = @i7,
								@i8 = @i8,
								@i9 = @i9,
								@i10 = @i10,
								@i11 = @i11,
								@i12 = @i12,
								@i13 = @i13,
								@i14 = @i14,
								@i15 = @i15,
								@i16 = @i16,
								@i17 = @i17,
								@i18 = @i18,
								@i19 = @i19,
							};
							
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						private global::MockTests.IHaveTooMuchCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IHaveTooMuchCreateExpectations.MethodExpectations Methods { get; }
					
					internal IHaveTooMuchCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IHaveTooMuch Instance()
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
						public interface IAdornmentsForIHaveTooMuch<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIHaveTooMuch<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.IHaveTooMuchCreateExpectations.Handler0, global::MockTests.IHaveTooMuchCreateExpectations.Projections.Callback_383140697323744298072430331353344056628280456971, int>, IAdornmentsForIHaveTooMuch<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.IHaveTooMuchCreateExpectations.Handler0 handler)
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

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[(typeof(RockGenerator), "MockTests.IHaveTooMuch_Rock_Create.g.cs", generatedCode)],
			[]);
	}
}