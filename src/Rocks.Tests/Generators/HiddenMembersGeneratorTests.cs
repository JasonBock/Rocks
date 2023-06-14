using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class HiddenMembersGeneratorTests
{
	[Test]
	public static async Task GenerateWithClassWhenMembersAreHiddenAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			#nullable enable

			public class BaseClass
			{
			    public virtual void Foo() { }   

			    public virtual string? Data { get; set; }

			    public virtual string this[int a, string b] { get => "2"; }
			}

			public class SubClass 
				: BaseClass
			{
			    public new virtual int Foo() => 2;  

			    public new virtual int Data { get; set; }

			    public new virtual int this[int a, string b] { get => 3; }
			}

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<SubClass>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfSubClassExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::SubClass> Methods(this global::Rocks.Expectations.Expectations<global::SubClass> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyExpectations<global::SubClass> Properties(this global::Rocks.Expectations.Expectations<global::SubClass> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyGetterExpectations<global::SubClass> Getters(this global::Rocks.Expectations.PropertyExpectations<global::SubClass> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertySetterExpectations<global::SubClass> Setters(this global::Rocks.Expectations.PropertyExpectations<global::SubClass> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.IndexerExpectations<global::SubClass> Indexers(this global::Rocks.Expectations.Expectations<global::SubClass> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.IndexerGetterExpectations<global::SubClass> Getters(this global::Rocks.Expectations.IndexerExpectations<global::SubClass> @self) =>
					new(@self);
				
				internal static global::SubClass Instance(this global::Rocks.Expectations.Expectations<global::SubClass> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						var @mock = new RockSubClass(@self);
						@self.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockSubClass
					: global::SubClass
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockSubClass(global::Rocks.Expectations.Expectations<global::SubClass> @expectations)
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
								if (((global::Rocks.Argument<object?>)@methodHandler.Expectations[0]).IsValid(@obj))
								{
									@methodHandler.IncrementCallCount();
									var @result = @methodHandler.Method is not null ?
										((global::System.Func<object?, bool>)@methodHandler.Method)(@obj) :
										((global::Rocks.HandlerInformation<bool>)@methodHandler).ReturnValue;
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
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								((global::System.Func<int>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
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
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								((global::System.Func<string?>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<string?>)@methodHandler).ReturnValue;
							return @result!;
						}
						else
						{
							return base.ToString();
						}
					}
					
					[global::Rocks.MemberIdentifier(4, "int Foo()")]
					public override int Foo()
					{
						if (this.handlers.TryGetValue(4, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								((global::System.Func<int>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
							return @result!;
						}
						else
						{
							return base.Foo();
						}
					}
					
					[global::Rocks.MemberIdentifier(8, "get_Data()")]
					[global::Rocks.MemberIdentifier(9, "set_Data(@value)")]
					public override int Data
					{
						get
						{
							if (this.handlers.TryGetValue(8, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									((global::System.Func<int>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
								return @result!;
							}
							else
							{
								return base.Data;
							}
						}
						set
						{
							if (this.handlers.TryGetValue(9, out var @methodHandlers))
							{
								var @foundMatch = false;
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((global::Rocks.Argument<int>)@methodHandler.Expectations[0]).IsValid(@value))
									{
										@methodHandler.IncrementCallCount();
										@foundMatch = true;
										
										if (@methodHandler.Method is not null)
										{
											((global::System.Action<int>)@methodHandler.Method)(@value);
										}
										
										if (!@foundMatch)
										{
											throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_Data(@value)");
										}
										
										break;
									}
								}
							}
							else
							{
								base.Data = @value;
							}
						}
					}
					[global::Rocks.MemberIdentifier(10, "this[int @a, string @b]")]
					public override int this[int @a, string @b]
					{
						get
						{
							if (this.handlers.TryGetValue(10, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((global::Rocks.Argument<int>)@methodHandler.Expectations[0]).IsValid(@a) &&
										((global::Rocks.Argument<string>)@methodHandler.Expectations[1]).IsValid(@b))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											((global::System.Func<int, string, int>)@methodHandler.Method)(@a, @b) :
											((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[int @a, string @b]");
							}
							else
							{
								return base[@a, @b];
							}
						}
					}
				}
			}
			
			internal static class MethodExpectationsOfSubClassExtensions
			{
				internal static global::Rocks.MethodAdornments<global::SubClass, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::SubClass> @self, global::Rocks.Argument<object?> @obj)
				{
					global::System.ArgumentNullException.ThrowIfNull(@obj);
					return new global::Rocks.MethodAdornments<global::SubClass, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
				}
				internal static global::Rocks.MethodAdornments<global::SubClass, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::SubClass> @self) =>
					new global::Rocks.MethodAdornments<global::SubClass, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::SubClass, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::SubClass> @self) =>
					new global::Rocks.MethodAdornments<global::SubClass, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::SubClass, global::System.Func<int>, int> Foo(this global::Rocks.Expectations.MethodExpectations<global::SubClass> @self) =>
					new global::Rocks.MethodAdornments<global::SubClass, global::System.Func<int>, int>(@self.Add<int>(4, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			internal static class PropertyGetterExpectationsOfSubClassExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::SubClass, global::System.Func<int>, int> Data(this global::Rocks.Expectations.PropertyGetterExpectations<global::SubClass> @self) =>
					new global::Rocks.PropertyAdornments<global::SubClass, global::System.Func<int>, int>(@self.Add<int>(8, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			internal static class PropertySetterExpectationsOfSubClassExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::SubClass, global::System.Action<int>> Data(this global::Rocks.Expectations.PropertySetterExpectations<global::SubClass> @self, global::Rocks.Argument<int> @value) =>
					new global::Rocks.PropertyAdornments<global::SubClass, global::System.Action<int>>(@self.Add(9, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
			}
			internal static class IndexerGetterExpectationsOfSubClassExtensions
			{
				internal static global::Rocks.IndexerAdornments<global::SubClass, global::System.Func<int, string, int>, int> This(this global::Rocks.Expectations.IndexerGetterExpectations<global::SubClass> @self, global::Rocks.Argument<int> @a, global::Rocks.Argument<string> @b)
				{
					global::System.ArgumentNullException.ThrowIfNull(@a);
					global::System.ArgumentNullException.ThrowIfNull(@b);
					return new global::Rocks.IndexerAdornments<global::SubClass, global::System.Func<int, string, int>, int>(@self.Add<int>(10, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @a, @b }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGeneratorV3>(code,
			new[] { (typeof(RockCreateGeneratorV3), "SubClass_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithInterfaceWhenMembersAreHiddenAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			#nullable enable

			public interface IBase
			{
			   void Foo();

			   string Data { get; set; }

				string this[int a, string b] { get; }
			}

			public interface ISub : IBase
			{
			   new int Foo();

			   new int Data { get; set; }

				new int this[int a, string b] { get; }
			}

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<ISub>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfISubExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::ISub> Methods(this global::Rocks.Expectations.Expectations<global::ISub> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitMethodExpectations<global::ISub, global::IBase> ExplicitMethodsForIBase(this global::Rocks.Expectations.Expectations<global::ISub> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyExpectations<global::ISub> Properties(this global::Rocks.Expectations.Expectations<global::ISub> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyGetterExpectations<global::ISub> Getters(this global::Rocks.Expectations.PropertyExpectations<global::ISub> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertySetterExpectations<global::ISub> Setters(this global::Rocks.Expectations.PropertyExpectations<global::ISub> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitPropertyExpectations<global::ISub, global::IBase> ExplicitPropertiesForIBase(this global::Rocks.Expectations.Expectations<global::ISub> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitPropertyGetterExpectations<global::ISub, global::IBase> Getters(this global::Rocks.Expectations.ExplicitPropertyExpectations<global::ISub, global::IBase> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitPropertySetterExpectations<global::ISub, global::IBase> Setters(this global::Rocks.Expectations.ExplicitPropertyExpectations<global::ISub, global::IBase> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.IndexerExpectations<global::ISub> Indexers(this global::Rocks.Expectations.Expectations<global::ISub> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.IndexerGetterExpectations<global::ISub> Getters(this global::Rocks.Expectations.IndexerExpectations<global::ISub> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitIndexerExpectations<global::ISub, global::IBase> ExplicitIndexersForIBase(this global::Rocks.Expectations.Expectations<global::ISub> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitIndexerGetterExpectations<global::ISub, global::IBase> Getters(this global::Rocks.Expectations.ExplicitIndexerExpectations<global::ISub, global::IBase> @self) =>
					new(@self);
				
				internal static global::ISub Instance(this global::Rocks.Expectations.Expectations<global::ISub> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						var @mock = new RockISub(@self);
						@self.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockISub
					: global::ISub
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockISub(global::Rocks.Expectations.Expectations<global::ISub> @expectations)
					{
						this.handlers = @expectations.Handlers;
					}
					
					[global::Rocks.MemberIdentifier(0, "int Foo()")]
					public int Foo()
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								((global::System.Func<int>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int Foo()");
					}
					
					[global::Rocks.MemberIdentifier(1, "void global::IBase.Foo()")]
					void global::IBase.Foo()
					{
						if (this.handlers.TryGetValue(1, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							@methodHandler.IncrementCallCount();
							if (@methodHandler.Method is not null)
							{
								((global::System.Action)@methodHandler.Method)();
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void global::IBase.Foo()");
						}
					}
					
					[global::Rocks.MemberIdentifier(2, "get_Data()")]
					[global::Rocks.MemberIdentifier(3, "set_Data(@value)")]
					public int Data
					{
						get
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									((global::System.Func<int>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_Data())");
						}
						set
						{
							if (this.handlers.TryGetValue(3, out var @methodHandlers))
							{
								var @foundMatch = false;
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((global::Rocks.Argument<int>)@methodHandler.Expectations[0]).IsValid(@value))
									{
										@methodHandler.IncrementCallCount();
										@foundMatch = true;
										
										if (@methodHandler.Method is not null)
										{
											((global::System.Action<int>)@methodHandler.Method)(@value);
										}
										
										if (!@foundMatch)
										{
											throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_Data(@value)");
										}
										
										break;
									}
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for set_Data(@value)");
							}
						}
					}
					[global::Rocks.MemberIdentifier(5, "global::IBase.get_Data()")]
					[global::Rocks.MemberIdentifier(6, "global::IBase.set_Data(@value)")]
					string global::IBase.Data
					{
						get
						{
							if (this.handlers.TryGetValue(5, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									((global::System.Func<string>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<string>)@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::IBase.get_Data())");
						}
						set
						{
							if (this.handlers.TryGetValue(6, out var @methodHandlers))
							{
								var @foundMatch = false;
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((global::Rocks.Argument<string>)@methodHandler.Expectations[0]).IsValid(@value))
									{
										@methodHandler.IncrementCallCount();
										@foundMatch = true;
										
										if (@methodHandler.Method is not null)
										{
											((global::System.Action<string>)@methodHandler.Method)(@value);
										}
										
										if (!@foundMatch)
										{
											throw new global::Rocks.Exceptions.ExpectationException("No handlers match for global::IBase.set_Data(@value)");
										}
										
										break;
									}
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::IBase.set_Data(@value)");
							}
						}
					}
					[global::Rocks.MemberIdentifier(4, "this[int @a, string @b]")]
					public int this[int @a, string @b]
					{
						get
						{
							if (this.handlers.TryGetValue(4, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((global::Rocks.Argument<int>)@methodHandler.Expectations[0]).IsValid(@a) &&
										((global::Rocks.Argument<string>)@methodHandler.Expectations[1]).IsValid(@b))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											((global::System.Func<int, string, int>)@methodHandler.Method)(@a, @b) :
											((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[int @a, string @b]");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[int @a, string @b])");
						}
					}
					[global::Rocks.MemberIdentifier(7, "global::IBase.this[int @a, string @b]")]
					string global::IBase.this[int @a, string @b]
					{
						get
						{
							if (this.handlers.TryGetValue(7, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((global::Rocks.Argument<int>)@methodHandler.Expectations[0]).IsValid(@a) &&
										((global::Rocks.Argument<string>)@methodHandler.Expectations[1]).IsValid(@b))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											((global::System.Func<int, string, string>)@methodHandler.Method)(@a, @b) :
											((global::Rocks.HandlerInformation<string>)@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[int @a, string @b]");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[int @a, string @b])");
						}
					}
				}
			}
			
			internal static class MethodExpectationsOfISubExtensions
			{
				internal static global::Rocks.MethodAdornments<global::ISub, global::System.Func<int>, int> Foo(this global::Rocks.Expectations.MethodExpectations<global::ISub> @self) =>
					new global::Rocks.MethodAdornments<global::ISub, global::System.Func<int>, int>(@self.Add<int>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			internal static class ExplicitMethodExpectationsOfISubForIBaseExtensions
			{
				internal static global::Rocks.MethodAdornments<global::ISub, global::System.Action> Foo(this global::Rocks.Expectations.ExplicitMethodExpectations<global::ISub, global::IBase> @self) =>
					new global::Rocks.MethodAdornments<global::ISub, global::System.Action>(@self.Add(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			internal static class PropertyGetterExpectationsOfISubExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::ISub, global::System.Func<int>, int> Data(this global::Rocks.Expectations.PropertyGetterExpectations<global::ISub> @self) =>
					new global::Rocks.PropertyAdornments<global::ISub, global::System.Func<int>, int>(@self.Add<int>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			internal static class PropertySetterExpectationsOfISubExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::ISub, global::System.Action<int>> Data(this global::Rocks.Expectations.PropertySetterExpectations<global::ISub> @self, global::Rocks.Argument<int> @value) =>
					new global::Rocks.PropertyAdornments<global::ISub, global::System.Action<int>>(@self.Add(3, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
			}
			internal static class IndexerGetterExpectationsOfISubExtensions
			{
				internal static global::Rocks.IndexerAdornments<global::ISub, global::System.Func<int, string, int>, int> This(this global::Rocks.Expectations.IndexerGetterExpectations<global::ISub> @self, global::Rocks.Argument<int> @a, global::Rocks.Argument<string> @b)
				{
					global::System.ArgumentNullException.ThrowIfNull(@a);
					global::System.ArgumentNullException.ThrowIfNull(@b);
					return new global::Rocks.IndexerAdornments<global::ISub, global::System.Func<int, string, int>, int>(@self.Add<int>(4, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @a, @b }));
				}
			}
			internal static class ExplicitPropertyGetterExpectationsOfISubForIBaseExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::ISub, global::System.Func<string>, string> Data(this global::Rocks.Expectations.ExplicitPropertyGetterExpectations<global::ISub, global::IBase> @self) =>
					new global::Rocks.PropertyAdornments<global::ISub, global::System.Func<string>, string>(@self.Add<string>(5, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			internal static class ExplicitPropertySetterExpectationsOfISubForIBaseExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::ISub, global::System.Action<string>> Data(this global::Rocks.Expectations.ExplicitPropertySetterExpectations<global::ISub, global::IBase> @self, global::Rocks.Argument<string> value) =>
					new global::Rocks.PropertyAdornments<global::ISub, global::System.Action<string>>(@self.Add(6, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { value }));
			}
			internal static class ExplicitIndexerGetterExpectationsOfISubForIBaseExtensions
			{
				internal static global::Rocks.IndexerAdornments<global::ISub, global::System.Func<int, string, string>, string> This(this global::Rocks.Expectations.ExplicitIndexerGetterExpectations<global::ISub, global::IBase> @self, global::Rocks.Argument<int> @a, global::Rocks.Argument<string> @b)
				{
					global::System.ArgumentNullException.ThrowIfNull(@a);
					global::System.ArgumentNullException.ThrowIfNull(@b);
					return new global::Rocks.IndexerAdornments<global::ISub, global::System.Func<int, string, string>, string>(@self.Add<string>(7, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @a, @b }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGeneratorV3>(code,
			new[] { (typeof(RockCreateGeneratorV3), "ISub_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}