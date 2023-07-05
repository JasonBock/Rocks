using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class GenericsGeneratorTests
{
	[Test]
	public static async Task GenerateWhenClosedGenericCreatesMethodMatchAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Threading.Tasks;
			
			namespace MockTests
			{
				public interface IRequest<T>
					where T : class
				{
					Task<T> Send(Guid requestId, object values);
					Task Send(Guid requestId, T message);
				}
			
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IRequest<object>>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfIRequestOfobjectExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.IRequest<object>> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.IRequest<object>> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IRequest<object>, global::MockTests.IRequest<object>> ExplicitMethodsForIRequestOfobject(this global::Rocks.Expectations.Expectations<global::MockTests.IRequest<object>> @self) =>
						new(@self);
					
					internal static global::MockTests.IRequest<object> Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IRequest<object>> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockIRequestOfobject(@self);
							@self.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIRequestOfobject
						: global::MockTests.IRequest<object>
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIRequestOfobject(global::Rocks.Expectations.Expectations<global::MockTests.IRequest<object>> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::Rocks.MemberIdentifier(0, "global::System.Threading.Tasks.Task<object> Send(global::System.Guid @requestId, object @values)")]
						public global::System.Threading.Tasks.Task<object> Send(global::System.Guid @requestId, object @values)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((global::Rocks.Argument<global::System.Guid>)@methodHandler.Expectations[0]).IsValid(@requestId) &&
										((global::Rocks.Argument<object>)@methodHandler.Expectations[1]).IsValid(@values))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											((global::System.Func<global::System.Guid, object, global::System.Threading.Tasks.Task<object>>)@methodHandler.Method)(@requestId, @values) :
											((global::Rocks.HandlerInformation<global::System.Threading.Tasks.Task<object>>)@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for global::System.Threading.Tasks.Task<object> Send(global::System.Guid @requestId, object @values)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::System.Threading.Tasks.Task<object> Send(global::System.Guid @requestId, object @values)");
						}
						
						[global::Rocks.MemberIdentifier(1, "global::System.Threading.Tasks.Task global::MockTests.IRequest<object>.Send(global::System.Guid @requestId, object @message)")]
						global::System.Threading.Tasks.Task global::MockTests.IRequest<object>.Send(global::System.Guid @requestId, object @message)
						{
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((global::Rocks.Argument<global::System.Guid>)@methodHandler.Expectations[0]).IsValid(@requestId) &&
										((global::Rocks.Argument<object>)@methodHandler.Expectations[1]).IsValid(@message))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											((global::System.Func<global::System.Guid, object, global::System.Threading.Tasks.Task>)@methodHandler.Method)(@requestId, @message) :
											((global::Rocks.HandlerInformation<global::System.Threading.Tasks.Task>)@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for global::System.Threading.Tasks.Task global::MockTests.IRequest<object>.Send(global::System.Guid @requestId, object @message)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::System.Threading.Tasks.Task global::MockTests.IRequest<object>.Send(global::System.Guid @requestId, object @message)");
						}
						
					}
				}
				
				internal static class MethodExpectationsOfIRequestOfobjectExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IRequest<object>, global::System.Func<global::System.Guid, object, global::System.Threading.Tasks.Task<object>>, global::System.Threading.Tasks.Task<object>> Send(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IRequest<object>> @self, global::Rocks.Argument<global::System.Guid> @requestId, global::Rocks.Argument<object> @values)
					{
						global::System.ArgumentNullException.ThrowIfNull(@requestId);
						global::System.ArgumentNullException.ThrowIfNull(@values);
						return new global::Rocks.MethodAdornments<global::MockTests.IRequest<object>, global::System.Func<global::System.Guid, object, global::System.Threading.Tasks.Task<object>>, global::System.Threading.Tasks.Task<object>>(@self.Add<global::System.Threading.Tasks.Task<object>>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @requestId, @values }));
					}
				}
				internal static class ExplicitMethodExpectationsOfIRequestOfobjectForIRequestOfobjectExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IRequest<object>, global::System.Func<global::System.Guid, object, global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task> Send(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IRequest<object>, global::MockTests.IRequest<object>> @self, global::Rocks.Argument<global::System.Guid> @requestId, global::Rocks.Argument<object> @message)
					{
						global::System.ArgumentNullException.ThrowIfNull(@requestId);
						global::System.ArgumentNullException.ThrowIfNull(@message);
						return new global::Rocks.MethodAdornments<global::MockTests.IRequest<object>, global::System.Func<global::System.Guid, object, global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(@self.Add<global::System.Threading.Tasks.Task>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @requestId, @message }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IRequestOfobject_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}