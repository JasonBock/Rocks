using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class AllowNullGeneratorTests
{
	[Test]
	public static async Task GenerateAbstractCreateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			namespace MockTests
			{
				public interface IAllow
				{
					 [AllowNull]
					 string NewLine { get; set; }
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IAllow>();
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
				internal static class CreateExpectationsOfIAllowExtensions
				{
					internal static global::Rocks.Expectations.PropertyExpectations<global::MockTests.IAllow> Properties(this global::Rocks.Expectations.Expectations<global::MockTests.IAllow> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.IAllow> Getters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.IAllow> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertySetterExpectations<global::MockTests.IAllow> Setters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.IAllow> @self) =>
						new(@self);
					
					internal static global::MockTests.IAllow Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IAllow> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockIAllow(@self);
							@self.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIAllow
						: global::MockTests.IAllow
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIAllow(global::Rocks.Expectations.Expectations<global::MockTests.IAllow> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]
						[global::Rocks.MemberIdentifier(0, "get_NewLine()")]
						[global::Rocks.MemberIdentifier(1, "set_NewLine(@value)")]
						public string NewLine
						{
							get
							{
								if (this.handlers.TryGetValue(0, out var @methodHandlers))
								{
									var @methodHandler = @methodHandlers[0];
									var @result = @methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string>>(@methodHandler.Method)() :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string>>(@methodHandler).ReturnValue;
									@methodHandler.IncrementCallCount();
									return @result!;
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_NewLine())");
							}
							set
							{
								if (this.handlers.TryGetValue(1, out var @methodHandlers))
								{
									var @foundMatch = false;
									foreach (var @methodHandler in @methodHandlers)
									{
										if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[0]).IsValid(@value!))
										{
											@foundMatch = true;
											
											if (@methodHandler.Method is not null)
											{
												global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<string>>(@methodHandler.Method)(@value!);
											}
											
											if (!@foundMatch)
											{
												throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_NewLine(@value)");
											}
											
											@methodHandler.IncrementCallCount();
											break;
										}
									}
								}
								else
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for set_NewLine(@value)");
								}
							}
						}
					}
				}
				
				internal static class PropertyGetterExpectationsOfIAllowExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.IAllow, global::System.Func<string>, string> NewLine(this global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.IAllow> @self) =>
						new global::Rocks.PropertyAdornments<global::MockTests.IAllow, global::System.Func<string>, string>(@self.Add<string>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				internal static class PropertySetterExpectationsOfIAllowExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.IAllow, global::System.Action<string>> NewLine(this global::Rocks.Expectations.PropertySetterExpectations<global::MockTests.IAllow> @self, global::Rocks.Argument<string> @value) =>
						new global::Rocks.PropertyAdornments<global::MockTests.IAllow, global::System.Action<string>>(@self.Add(1, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IAllow_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateAbstractMakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			namespace MockTests
			{
				public interface IAllow
				{
					 [AllowNull]
					 string NewLine { get; set; }
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Make<IAllow>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class MakeExpectationsOfIAllowExtensions
				{
					internal static global::MockTests.IAllow Instance(this global::Rocks.MakeGeneration<global::MockTests.IAllow> @self)
					{
						return new RockIAllow();
					}
					
					private sealed class RockIAllow
						: global::MockTests.IAllow
					{
						public RockIAllow()
						{
						}
						
						[global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]
						public string NewLine
						{
							get => default!;
							set { }
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "IAllow_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateNonAbstractCreateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			namespace MockTests
			{
				public class Allow
				{
					 [AllowNull]
					 public virtual string NewLine { get; set; }
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<Allow>();
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
				internal static class CreateExpectationsOfAllowExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.Allow> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.Allow> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertyExpectations<global::MockTests.Allow> Properties(this global::Rocks.Expectations.Expectations<global::MockTests.Allow> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.Allow> Getters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.Allow> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertySetterExpectations<global::MockTests.Allow> Setters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.Allow> @self) =>
						new(@self);
					
					internal static global::MockTests.Allow Instance(this global::Rocks.Expectations.Expectations<global::MockTests.Allow> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockAllow(@self);
							@self.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockAllow
						: global::MockTests.Allow
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockAllow(global::Rocks.Expectations.Expectations<global::MockTests.Allow> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
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
						
						[global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]
						[global::Rocks.MemberIdentifier(3, "get_NewLine()")]
						[global::Rocks.MemberIdentifier(4, "set_NewLine(@value)")]
						public override string NewLine
						{
							get
							{
								if (this.handlers.TryGetValue(3, out var @methodHandlers))
								{
									var @methodHandler = @methodHandlers[0];
									var @result = @methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string>>(@methodHandler.Method)() :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string>>(@methodHandler).ReturnValue;
									@methodHandler.IncrementCallCount();
									return @result!;
								}
								else
								{
									return base.NewLine;
								}
							}
							set
							{
								if (this.handlers.TryGetValue(4, out var @methodHandlers))
								{
									var @foundMatch = false;
									foreach (var @methodHandler in @methodHandlers)
									{
										if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[0]).IsValid(@value!))
										{
											@foundMatch = true;
											
											if (@methodHandler.Method is not null)
											{
												global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<string>>(@methodHandler.Method)(@value!);
											}
											
											if (!@foundMatch)
											{
												throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_NewLine(@value)");
											}
											
											@methodHandler.IncrementCallCount();
											break;
										}
									}
								}
								else
								{
									base.NewLine = @value;
								}
							}
						}
					}
				}
				
				internal static class MethodExpectationsOfAllowExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Allow> @self, global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						return new global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Allow> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Allow> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Allow, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				
				internal static class PropertyGetterExpectationsOfAllowExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.Allow, global::System.Func<string>, string> NewLine(this global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.Allow> @self) =>
						new global::Rocks.PropertyAdornments<global::MockTests.Allow, global::System.Func<string>, string>(@self.Add<string>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				internal static class PropertySetterExpectationsOfAllowExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.Allow, global::System.Action<string>> NewLine(this global::Rocks.Expectations.PropertySetterExpectations<global::MockTests.Allow> @self, global::Rocks.Argument<string> @value) =>
						new global::Rocks.PropertyAdornments<global::MockTests.Allow, global::System.Action<string>>(@self.Add(4, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Allow_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateNonAbstractMakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			namespace MockTests
			{
				public class Allow
				{
					 [AllowNull]
					 public virtual string NewLine { get; set; }
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Make<Allow>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class MakeExpectationsOfAllowExtensions
				{
					internal static global::MockTests.Allow Instance(this global::Rocks.MakeGeneration<global::MockTests.Allow> @self)
					{
						return new RockAllow();
					}
					
					private sealed class RockAllow
						: global::MockTests.Allow
					{
						public RockAllow()
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
						public override string? ToString()
						{
							return default!;
						}
						[global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]
						public override string NewLine
						{
							get => default!;
							set { }
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "Allow_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}