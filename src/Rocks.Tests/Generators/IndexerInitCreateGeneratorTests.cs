using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class IndexerInitCreateGeneratorTests
{
	[Test]
	public static async Task CreateWhenTypeHasMultipleIndexersWithInitAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			public interface IHaveIndexersWithInit
			{
				double this[uint a] { init; }
				string this[int a, string b] { init; }
				int this[string a, int b, Guid c] { init; }
			}

			public static class Test
			{
				public static void Generate()
				{
					var rock = Rock.Create<IHaveIndexersWithInit>();
				}
			}			
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIHaveIndexersWithInitExtensions
			{
				internal static global::Rocks.Expectations.IndexerExpectations<global::IHaveIndexersWithInit> Indexers(this global::Rocks.Expectations.Expectations<global::IHaveIndexersWithInit> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.IndexerSetterExpectations<global::IHaveIndexersWithInit> Setters(this global::Rocks.Expectations.IndexerExpectations<global::IHaveIndexersWithInit> @self) =>
					new(@self);
				
				internal sealed class ConstructorProperties
					: global::System.Collections.Generic.IEnumerable<uint>, global::System.Collections.Generic.IEnumerable<(int, string)>, global::System.Collections.Generic.IEnumerable<(string, int, global::System.Guid)>
				{
					private readonly global::System.Collections.Generic.Dictionary<uint, double> i0 = new();
					private readonly global::System.Collections.Generic.Dictionary<(int, string), string> i1 = new();
					private readonly global::System.Collections.Generic.Dictionary<(string, int, global::System.Guid), int> i2 = new();
					
					global::System.Collections.Generic.IEnumerator<uint> global::System.Collections.Generic.IEnumerable<uint>.GetEnumerator()
					{
					    foreach(var key in this.i0.Keys)
					    {
					        yield return key;
					    }
					}
					global::System.Collections.Generic.IEnumerator<(int, string)> global::System.Collections.Generic.IEnumerable<(int, string)>.GetEnumerator()
					{
					    foreach(var key in this.i1.Keys)
					    {
					        yield return key;
					    }
					}
					global::System.Collections.Generic.IEnumerator<(string, int, global::System.Guid)> global::System.Collections.Generic.IEnumerable<(string, int, global::System.Guid)>.GetEnumerator()
					{
					    foreach(var key in this.i2.Keys)
					    {
					        yield return key;
					    }
					}
					
					global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() => throw new global::System.NotImplementedException();
					
					internal double this[uint a]
					{
					    get => this.i0[a];
					    init => this.i0[a] = value;
					}
					internal string this[int a, string b]
					{
					    get => this.i1[(a, b)];
					    init => this.i1[(a, b)] = value;
					}
					internal int this[string a, int b, global::System.Guid c]
					{
					    get => this.i2[(a, b, c)];
					    init => this.i2[(a, b, c)] = value;
					}
				}
				
				internal static global::IHaveIndexersWithInit Instance(this global::Rocks.Expectations.Expectations<global::IHaveIndexersWithInit> @self, ConstructorProperties? @constructorProperties)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockIHaveIndexersWithInit(@self, @constructorProperties);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIHaveIndexersWithInit
					: global::IHaveIndexersWithInit
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIHaveIndexersWithInit(global::Rocks.Expectations.Expectations<global::IHaveIndexersWithInit> @expectations, ConstructorProperties? @constructorProperties)
					{
						this.handlers = @expectations.Handlers;
						if (@constructorProperties is not null)
						{
							foreach(var a in global::System.Runtime.CompilerServices.Unsafe.As<global::System.Collections.Generic.IEnumerable<uint>>(@constructorProperties))
							{
								 this[a] = constructorProperties[a];
							}
							foreach((var a, var b) in global::System.Runtime.CompilerServices.Unsafe.As<global::System.Collections.Generic.IEnumerable<(int, string)>>(@constructorProperties))
							{
								 this[a, b] = constructorProperties[a, b];
							}
							foreach((var a, var b, var c) in global::System.Runtime.CompilerServices.Unsafe.As<global::System.Collections.Generic.IEnumerable<(string, int, global::System.Guid)>>(@constructorProperties))
							{
								 this[a, b, c] = constructorProperties[a, b, c];
							}
						}
					}
					
					[global::Rocks.MemberIdentifier(0, "this[uint @a]")]
					public double this[uint @a]
					{
						init
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<uint>>(@methodHandler.Expectations[0]).IsValid(@a) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<double>>(@methodHandler.Expectations[1]).IsValid(@value))
									{
										if (@methodHandler.Method is not null)
										{
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<uint, double>>(@methodHandler.Method)(@a, @value);
										}
										
										@methodHandler.IncrementCallCount();
										return;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[uint @a]");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[uint @a])");
						}
					}
					[global::Rocks.MemberIdentifier(1, "this[int @a, string @b]")]
					public string this[int @a, string @b]
					{
						init
						{
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<int>>(@methodHandler.Expectations[0]).IsValid(@a) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[1]).IsValid(@b) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[2]).IsValid(@value))
									{
										if (@methodHandler.Method is not null)
										{
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<int, string, string>>(@methodHandler.Method)(@a, @b, @value);
										}
										
										@methodHandler.IncrementCallCount();
										return;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[int @a, string @b]");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[int @a, string @b])");
						}
					}
					[global::Rocks.MemberIdentifier(2, "this[string @a, int @b, global::System.Guid @c]")]
					public int this[string @a, int @b, global::System.Guid @c]
					{
						init
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[0]).IsValid(@a) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<int>>(@methodHandler.Expectations[1]).IsValid(@b) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<global::System.Guid>>(@methodHandler.Expectations[2]).IsValid(@c) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<int>>(@methodHandler.Expectations[3]).IsValid(@value))
									{
										if (@methodHandler.Method is not null)
										{
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<string, int, global::System.Guid, int>>(@methodHandler.Method)(@a, @b, @c, @value);
										}
										
										@methodHandler.IncrementCallCount();
										return;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[string @a, int @b, global::System.Guid @c]");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[string @a, int @b, global::System.Guid @c])");
						}
					}
				}
			}
			
			internal static class IndexerSetterExpectationsOfIHaveIndexersWithInitExtensions
			{
				internal static global::Rocks.IndexerAdornments<global::IHaveIndexersWithInit, global::System.Action<uint, double>> This(this global::Rocks.Expectations.IndexerSetterExpectations<global::IHaveIndexersWithInit> @self, global::Rocks.Argument<uint> @a, global::Rocks.Argument<double> @value)
				{
					global::System.ArgumentNullException.ThrowIfNull(@a);
					global::System.ArgumentNullException.ThrowIfNull(@value);
					return new global::Rocks.IndexerAdornments<global::IHaveIndexersWithInit, global::System.Action<uint, double>>(self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @a, @value }));
				}
				internal static global::Rocks.IndexerAdornments<global::IHaveIndexersWithInit, global::System.Action<int, string, string>> This(this global::Rocks.Expectations.IndexerSetterExpectations<global::IHaveIndexersWithInit> @self, global::Rocks.Argument<int> @a, global::Rocks.Argument<string> @b, global::Rocks.Argument<string> @value)
				{
					global::System.ArgumentNullException.ThrowIfNull(@a);
					global::System.ArgumentNullException.ThrowIfNull(@b);
					global::System.ArgumentNullException.ThrowIfNull(@value);
					return new global::Rocks.IndexerAdornments<global::IHaveIndexersWithInit, global::System.Action<int, string, string>>(self.Add(1, new global::System.Collections.Generic.List<global::Rocks.Argument>(3) { @a, @b, @value }));
				}
				internal static global::Rocks.IndexerAdornments<global::IHaveIndexersWithInit, global::System.Action<string, int, global::System.Guid, int>> This(this global::Rocks.Expectations.IndexerSetterExpectations<global::IHaveIndexersWithInit> @self, global::Rocks.Argument<string> @a, global::Rocks.Argument<int> @b, global::Rocks.Argument<global::System.Guid> @c, global::Rocks.Argument<int> @value)
				{
					global::System.ArgumentNullException.ThrowIfNull(@a);
					global::System.ArgumentNullException.ThrowIfNull(@b);
					global::System.ArgumentNullException.ThrowIfNull(@c);
					global::System.ArgumentNullException.ThrowIfNull(@value);
					return new global::Rocks.IndexerAdornments<global::IHaveIndexersWithInit, global::System.Action<string, int, global::System.Guid, int>>(self.Add(2, new global::System.Collections.Generic.List<global::Rocks.Argument>(4) { @a, @b, @c, @value }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveIndexersWithInit_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task MakeWhenTypeHasMultipleIndexersWithInitAsync()
	{
		var code =
			"""
			using System;

			public interface IHaveIndexersWithInit
			{
				double this[uint a] { init; }
				string this[int a, string b] { init; }
				int this[string a, int b, Guid c] { init; }
			}

			public static class Test
			{
				public static void Generate()
				{
					var rock = Rock.Make<IHaveIndexersWithInit>();
				}
			}			
			""";

		var generatedCode = "";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "IHaveIndexersWithInit_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public abstract class Target
				{
					public abstract int this[int a] { get; init; }
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<Target>();
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
				internal static class CreateExpectationsOfTargetExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.Target> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.Target> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.IndexerExpectations<global::MockTests.Target> Indexers(this global::Rocks.Expectations.Expectations<global::MockTests.Target> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.IndexerGetterExpectations<global::MockTests.Target> Getters(this global::Rocks.Expectations.IndexerExpectations<global::MockTests.Target> @self) =>
						new(@self);
					
					internal static global::MockTests.Target Instance(this global::Rocks.Expectations.Expectations<global::MockTests.Target> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							return new RockTarget(@self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockTarget
						: global::MockTests.Target
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockTarget(global::Rocks.Expectations.Expectations<global::MockTests.Target> @expectations) =>
							this.handlers = @expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "bool Equals(object? @obj)")]
						public override bool Equals(object? @obj)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(@methodHandler.Expectations[0]).IsValid(@obj))
									{
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(@methodHandler.Method)(@obj) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(@methodHandler).ReturnValue;
										@methodHandler.IncrementCallCount();
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool Equals(object? @obj)");
							}
							else
							{
								return base.Equals(@obj);
							}
						}
						
						[global::Rocks.MemberIdentifier(1, "int GetHashCode()")]
						public override int GetHashCode()
						{
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
								@methodHandler.IncrementCallCount();
								return @result!;
							}
							else
							{
								return base.GetHashCode();
							}
						}
						
						[global::Rocks.MemberIdentifier(2, "string? ToString()")]
						public override string? ToString()
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(@methodHandler).ReturnValue;
								@methodHandler.IncrementCallCount();
								return @result!;
							}
							else
							{
								return base.ToString();
							}
						}
						
						[global::Rocks.MemberIdentifier(3, "this[int @a]")]
						[global::Rocks.MemberIdentifier(4, "this[int @a]")]
						public override int this[int @a]
						{
							get
							{
								if (this.handlers.TryGetValue(3, out var @methodHandlers))
								{
									foreach (var @methodHandler in @methodHandlers)
									{
										if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<int>>(@methodHandler.Expectations[0]).IsValid(@a))
										{
											var @result = @methodHandler.Method is not null ?
												global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int, int>>(@methodHandler.Method)(@a) :
												global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
											@methodHandler.IncrementCallCount();
											return @result!;
										}
									}
									
									throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[int @a]");
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[int @a])");
							}
							init { }
						}
					}
				}
				
				internal static class MethodExpectationsOfTargetExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Target> @self, global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						return new global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Target> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Target> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				
				internal static class IndexerGetterExpectationsOfTargetExtensions
				{
					internal static global::Rocks.IndexerAdornments<global::MockTests.Target, global::System.Func<int, int>, int> This(this global::Rocks.Expectations.IndexerGetterExpectations<global::MockTests.Target> @self, global::Rocks.Argument<int> @a)
					{
						global::System.ArgumentNullException.ThrowIfNull(@a);
						return new global::Rocks.IndexerAdornments<global::MockTests.Target, global::System.Func<int, int>, int>(@self.Add<int>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @a }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Target_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}