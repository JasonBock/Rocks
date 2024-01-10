﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ObjectMethodsAndInterfacesGeneratorTests
{
	[Test]
	public static async Task CreateWhenObjectMethodIsHiddenAsStaticAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<MockTests.StaticToString>]

			#nullable enable

			namespace MockTests
			{
				public class StaticToString
				{
					protected static new string ToString() => "c";   
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
				internal sealed class StaticToStringCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @obj { get; set; }
					}
					
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.StaticToStringCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.StaticToStringCreateExpectations.Handler1> @handlers1 = new();
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							failures.AddRange(this.Verify(handlers0));
							failures.AddRange(this.Verify(handlers1));
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class RockStaticToString
						: global::MockTests.StaticToString
					{
						public RockStaticToString(global::MockTests.StaticToStringCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "bool Equals(object? @obj)")]
						public override bool Equals(object? @obj)
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@obj.IsValid(@obj!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@obj!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool Equals(object? @obj)");
							}
							else
							{
								return base.Equals(obj: @obj!);
							}
						}
						
						[global::Rocks.MemberIdentifier(1, "int GetHashCode()")]
						public override int GetHashCode()
						{
							if (this.Expectations.handlers1.Count > 0)
							{
								var @handler = this.Expectations.handlers1[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							else
							{
								return base.GetHashCode();
							}
						}
						
						private global::MockTests.StaticToStringCreateExpectations Expectations { get; }
					}
					
					internal sealed class StaticToStringMethodExpectations
					{
						internal StaticToStringMethodExpectations(global::MockTests.StaticToStringCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.StaticToStringCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
						{
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var handler = new global::MockTests.StaticToStringCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.StaticToStringCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
						{
							var handler = new global::MockTests.StaticToStringCreateExpectations.Handler1();
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.StaticToStringCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.StaticToStringCreateExpectations.StaticToStringMethodExpectations Methods { get; }
					
					internal StaticToStringCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.StaticToString Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockStaticToString(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.StaticToString_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task MakeWhenObjectMethodIsHiddenAsStaticAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<MockTests.StaticToString>]

			#nullable enable

			namespace MockTests
			{
				public class StaticToString
				{
					protected static new string ToString() => "c";   
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class StaticToStringMakeExpectations
				{
					internal global::MockTests.StaticToString Instance()
					{
						return new RockStaticToString();
					}
					
					private sealed class RockStaticToString
						: global::MockTests.StaticToString
					{
						public RockStaticToString()
						{
						}
						
						public override bool Equals(object? @obj)
						{
							return default!;
						}
						public override int GetHashCode()
						{
							return default!;
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.StaticToString_Rock_Make.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithExactMatchesCreateAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<MockTests.IMatchObject<object>>]

			#nullable enable

			namespace MockTests
			{
				public interface IMatchObject<T>
				{
					bool Equals(T? other);
					bool ReferenceEquals(T? objA, T? objB);
					T MemberwiseClone();
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
				internal sealed class IMatchObjectOfobjectCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @other { get; set; }
					}
					
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<object?, object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @objA { get; set; }
						public global::Rocks.Argument<object?> @objB { get; set; }
					}
					
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<object>, object>
					{ }
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1> @handlers1 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2> @handlers2 = new();
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							failures.AddRange(this.Verify(handlers0));
							failures.AddRange(this.Verify(handlers1));
							failures.AddRange(this.Verify(handlers2));
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class RockIMatchObjectOfobject
						: global::MockTests.IMatchObject<object>
					{
						public RockIMatchObjectOfobject(global::MockTests.IMatchObjectOfobjectCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "bool global::MockTests.IMatchObject<object>.Equals(object? @other)")]
						bool global::MockTests.IMatchObject<object>.Equals(object? @other)
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@other.IsValid(@other!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@other!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool global::MockTests.IMatchObject<object>.Equals(object? @other)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.Equals(object? @other)");
						}
						
						[global::Rocks.MemberIdentifier(1, "bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)")]
						bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
						{
							if (this.Expectations.handlers1.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers1)
								{
									if (@handler.@objA.IsValid(@objA!) &&
										@handler.@objB.IsValid(@objB!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@objA!, @objB!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
						}
						
						[global::Rocks.MemberIdentifier(2, "object global::MockTests.IMatchObject<object>.MemberwiseClone()")]
						object global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							if (this.Expectations.handlers2.Count > 0)
							{
								var @handler = this.Expectations.handlers2[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for object global::MockTests.IMatchObject<object>.MemberwiseClone()");
						}
						
						private global::MockTests.IMatchObjectOfobjectCreateExpectations Expectations { get; }
					}
					
					internal sealed class IMatchObjectOfobjectExplicitMethodExpectationsForIMatchObjectOfobject
					{
						internal IMatchObjectOfobjectExplicitMethodExpectationsForIMatchObjectOfobject(global::MockTests.IMatchObjectOfobjectCreateExpectations expectations) =>
							this.Expectations = expectations;
					
						internal global::Rocks.Adornments<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @other)
						{
							global::System.ArgumentNullException.ThrowIfNull(@other);
							
							var handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0
							{
								@other = @other,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1, global::System.Func<object?, object?, bool>, bool> ReferenceEquals(global::Rocks.Argument<object?> @objA, global::Rocks.Argument<object?> @objB)
						{
							global::System.ArgumentNullException.ThrowIfNull(@objA);
							global::System.ArgumentNullException.ThrowIfNull(@objB);
							
							var handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1
							{
								@objA = @objA,
								@objB = @objB,
							};
							
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2, global::System.Func<object>, object> MemberwiseClone()
						{
							var handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.IMatchObjectOfobjectCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IMatchObjectOfobjectCreateExpectations.IMatchObjectOfobjectExplicitMethodExpectationsForIMatchObjectOfobject ExplicitMethodsForIMatchObjectOfobject { get; }
					
					internal IMatchObjectOfobjectCreateExpectations() =>
						(this.ExplicitMethodsForIMatchObjectOfobject) = (new(this));
					
					internal global::MockTests.IMatchObject<object> Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockIMatchObjectOfobject(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.IMatchObjectobject_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithExactMatchesMakeAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<MockTests.IMatchObject<object>>]

			#nullable enable

			namespace MockTests
			{
				public interface IMatchObject<T>
				{
					bool Equals(T? other);
					bool ReferenceEquals(T? objA, T? objB);
					T MemberwiseClone();
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class IMatchObjectOfobjectMakeExpectations
				{
					internal global::MockTests.IMatchObject<object> Instance()
					{
						return new RockIMatchObjectOfobject();
					}
					
					private sealed class RockIMatchObjectOfobject
						: global::MockTests.IMatchObject<object>
					{
						public RockIMatchObjectOfobject()
						{
						}
						
						bool global::MockTests.IMatchObject<object>.Equals(object? @other)
						{
							return default!;
						}
						bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
						{
							return default!;
						}
						object global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							return default!;
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.IMatchObjectobject_Rock_Make.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithDiffByReturnTypeMatchesCreateAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<MockTests.IMatchObject<object>>]

			#nullable enable

			namespace MockTests
			{
				public interface IMatchObject<T>
				{
					string Equals(T? other);
					int ReferenceEquals(T? objA, T? objB);
					bool MemberwiseClone();
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
				internal sealed class IMatchObjectOfobjectCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, string>, string>
					{
						public global::Rocks.Argument<object?> @other { get; set; }
					}
					
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<object?, object?, int>, int>
					{
						public global::Rocks.Argument<object?> @objA { get; set; }
						public global::Rocks.Argument<object?> @objB { get; set; }
					}
					
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<bool>, bool>
					{ }
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1> @handlers1 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2> @handlers2 = new();
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							failures.AddRange(this.Verify(handlers0));
							failures.AddRange(this.Verify(handlers1));
							failures.AddRange(this.Verify(handlers2));
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class RockIMatchObjectOfobject
						: global::MockTests.IMatchObject<object>
					{
						public RockIMatchObjectOfobject(global::MockTests.IMatchObjectOfobjectCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "string global::MockTests.IMatchObject<object>.Equals(object? @other)")]
						string global::MockTests.IMatchObject<object>.Equals(object? @other)
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@other.IsValid(@other!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@other!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for string global::MockTests.IMatchObject<object>.Equals(object? @other)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for string global::MockTests.IMatchObject<object>.Equals(object? @other)");
						}
						
						[global::Rocks.MemberIdentifier(1, "int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)")]
						int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
						{
							if (this.Expectations.handlers1.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers1)
								{
									if (@handler.@objA.IsValid(@objA!) &&
										@handler.@objB.IsValid(@objB!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@objA!, @objB!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
						}
						
						[global::Rocks.MemberIdentifier(2, "bool global::MockTests.IMatchObject<object>.MemberwiseClone()")]
						bool global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							if (this.Expectations.handlers2.Count > 0)
							{
								var @handler = this.Expectations.handlers2[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.MemberwiseClone()");
						}
						
						private global::MockTests.IMatchObjectOfobjectCreateExpectations Expectations { get; }
					}
					
					internal sealed class IMatchObjectOfobjectExplicitMethodExpectationsForIMatchObjectOfobject
					{
						internal IMatchObjectOfobjectExplicitMethodExpectationsForIMatchObjectOfobject(global::MockTests.IMatchObjectOfobjectCreateExpectations expectations) =>
							this.Expectations = expectations;
					
						internal global::Rocks.Adornments<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0, global::System.Func<object?, string>, string> Equals(global::Rocks.Argument<object?> @other)
						{
							global::System.ArgumentNullException.ThrowIfNull(@other);
							
							var handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0
							{
								@other = @other,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1, global::System.Func<object?, object?, int>, int> ReferenceEquals(global::Rocks.Argument<object?> @objA, global::Rocks.Argument<object?> @objB)
						{
							global::System.ArgumentNullException.ThrowIfNull(@objA);
							global::System.ArgumentNullException.ThrowIfNull(@objB);
							
							var handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1
							{
								@objA = @objA,
								@objB = @objB,
							};
							
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2, global::System.Func<bool>, bool> MemberwiseClone()
						{
							var handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.IMatchObjectOfobjectCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IMatchObjectOfobjectCreateExpectations.IMatchObjectOfobjectExplicitMethodExpectationsForIMatchObjectOfobject ExplicitMethodsForIMatchObjectOfobject { get; }
					
					internal IMatchObjectOfobjectCreateExpectations() =>
						(this.ExplicitMethodsForIMatchObjectOfobject) = (new(this));
					
					internal global::MockTests.IMatchObject<object> Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockIMatchObjectOfobject(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.IMatchObjectobject_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithDiffByReturnTypeMatchesMakeAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<MockTests.IMatchObject<object>>]

			#nullable enable

			namespace MockTests
			{
				public interface IMatchObject<T>
				{
					string Equals(T? other);
					int ReferenceEquals(T? objA, T? objB);
					bool MemberwiseClone();
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class IMatchObjectOfobjectMakeExpectations
				{
					internal global::MockTests.IMatchObject<object> Instance()
					{
						return new RockIMatchObjectOfobject();
					}
					
					private sealed class RockIMatchObjectOfobject
						: global::MockTests.IMatchObject<object>
					{
						public RockIMatchObjectOfobject()
						{
						}
						
						string global::MockTests.IMatchObject<object>.Equals(object? @other)
						{
							return default!;
						}
						int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
						{
							return default!;
						}
						bool global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							return default!;
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.IMatchObjectobject_Rock_Make.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}
}