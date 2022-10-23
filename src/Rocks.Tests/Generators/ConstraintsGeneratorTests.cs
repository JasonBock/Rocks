using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ConstraintsGeneratorTests
{
	[Test]
	public static async Task CreateWithDefaultConstraintAsync()
	{
		var code =
			"""
			using Rocks;

			#nullable enable

			public class BaseStuff
			{
				public virtual T? GetService<T>(object[] args) where T : class => default!;
			}

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<BaseStuff>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfBaseStuffExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::BaseStuff> Methods(this global::Rocks.Expectations.Expectations<global::BaseStuff> @self) =>
					new(@self);
				
				internal static global::BaseStuff Instance(this global::Rocks.Expectations.Expectations<global::BaseStuff> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockBaseStuff(@self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockBaseStuff
					: global::BaseStuff
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockBaseStuff(global::Rocks.Expectations.Expectations<global::BaseStuff> @expectations) =>
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
					
					[global::Rocks.MemberIdentifier(3, "T? GetService<T>(object[] @args)")]
					public override T? GetService<T>(object[] @args)
						where T : class
					{
						if (this.handlers.TryGetValue(3, out var @methodHandlers))
						{
							foreach (var @methodHandler in @methodHandlers)
							{
								if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object[]>>(@methodHandler.Expectations[0]).IsValid(@args))
								{
									var @result = @methodHandler.Method is not null && @methodHandler.Method is global::System.Func<object[], T?> @methodReturn ?
										@methodReturn(@args) :
										@methodHandler is global::Rocks.HandlerInformation<T?> @returnValue ?
											@returnValue.ReturnValue :
											throw new global::Rocks.Exceptions.MockException($"No return value could be obtained for T of type {typeof(T).FullName}.");
									@methodHandler.IncrementCallCount();
									return @result!;
								}
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers match for T? GetService<T>(object[] @args)");
						}
						else
						{
							return base.GetService<T>(@args);
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfBaseStuffExtensions
			{
				internal static global::Rocks.MethodAdornments<global::BaseStuff, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::BaseStuff> @self, global::Rocks.Argument<object?> @obj)
				{
					global::System.ArgumentNullException.ThrowIfNull(@obj);
					return new global::Rocks.MethodAdornments<global::BaseStuff, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
				}
				internal static global::Rocks.MethodAdornments<global::BaseStuff, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::BaseStuff> @self) =>
					new global::Rocks.MethodAdornments<global::BaseStuff, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::BaseStuff, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::BaseStuff> @self) =>
					new global::Rocks.MethodAdornments<global::BaseStuff, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::BaseStuff, global::System.Func<object[], T?>, T?> GetService<T>(this global::Rocks.Expectations.MethodExpectations<global::BaseStuff> @self, global::Rocks.Argument<object[]> @args) where T : class
				{
					global::System.ArgumentNullException.ThrowIfNull(@args);
					return new global::Rocks.MethodAdornments<global::BaseStuff, global::System.Func<object[], T?>, T?>(@self.Add<T?>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @args }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "BaseStuff_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task CreateWithDelegateCreationAndConstraintsAsync()
	{
		var code =
			"""
			using Rocks;
			
			public interface IDot<T> { }

			public sealed class Frame<TDot>
				where TDot : unmanaged, IDot<TDot>
			{ }

			public interface INeedDelegate
			{				
				void Foo<T>(ref int a, Frame<T> frame) where T : unmanaged, IDot<T>;
			}
			
			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<INeedDelegate>();
				}
			}
			""";

		var generatedCode =
			"""
			using ProjectionsForINeedDelegate;
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			namespace ProjectionsForINeedDelegate
			{
				internal delegate void FooCallback_191327403400827159052686230025463041183626019740<T>(ref int @a, global::Frame<T> @frame) where T : unmanaged, global::IDot<T>;
			}
			
			internal static class CreateExpectationsOfINeedDelegateExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::INeedDelegate> Methods(this global::Rocks.Expectations.Expectations<global::INeedDelegate> @self) =>
					new(@self);
				
				internal static global::INeedDelegate Instance(this global::Rocks.Expectations.Expectations<global::INeedDelegate> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockINeedDelegate(@self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockINeedDelegate
					: global::INeedDelegate
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockINeedDelegate(global::Rocks.Expectations.Expectations<global::INeedDelegate> @expectations) =>
						this.handlers = @expectations.Handlers;
					
					[global::Rocks.MemberIdentifier(0, "void Foo<T>(ref int @a, global::Frame<T> @frame)")]
					public void Foo<T>(ref int @a, global::Frame<T> @frame)
						where T : unmanaged, global::IDot<T>
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
						{
							var @foundMatch = false;
							
							foreach (var @methodHandler in @methodHandlers)
							{
								if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<int>>(@methodHandler.Expectations[0]).IsValid(@a) &&
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<global::Frame<T>>>(@methodHandler.Expectations[1]).IsValid(@frame))
								{
									@foundMatch = true;
									
									if (@methodHandler.Method is not null && @methodHandler.Method is global::ProjectionsForINeedDelegate.FooCallback_191327403400827159052686230025463041183626019740<T> @method)
									{
										@method(ref @a, @frame);
									}
									
									@methodHandler.IncrementCallCount();
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void Foo<T>(ref int @a, global::Frame<T> @frame)");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Foo<T>(ref int @a, global::Frame<T> @frame)");
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfINeedDelegateExtensions
			{
				internal static global::Rocks.MethodAdornments<global::INeedDelegate, global::ProjectionsForINeedDelegate.FooCallback_191327403400827159052686230025463041183626019740<T>> Foo<T>(this global::Rocks.Expectations.MethodExpectations<global::INeedDelegate> @self, global::Rocks.Argument<int> @a, global::Rocks.Argument<global::Frame<T>> @frame) where T : unmanaged, global::IDot<T>
				{
					global::System.ArgumentNullException.ThrowIfNull(@a);
					global::System.ArgumentNullException.ThrowIfNull(@frame);
					return new global::Rocks.MethodAdornments<global::INeedDelegate, global::ProjectionsForINeedDelegate.FooCallback_191327403400827159052686230025463041183626019740<T>>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @a, @frame }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "INeedDelegate_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateCreateTargetingInterfaceAsync()
	{
		var code =
			"""
			#nullable enable

			using Rocks;

			public interface InterfaceConstraint { }
			public class ClassConstraint { }

			public interface ITypeConstraints
			{				
			   void HasUnmanaged<T>() where T : unmanaged;
			   void HasNotNull<T>() where T : notnull;
			   void HasClass<T>() where T : class;
			   void HasStruct<T>() where T : struct;
			   void HasClassTypeConstraint<T>() where T : ClassConstraint;
			   void HasInterfaceTypeConstraint<T>() where T : InterfaceConstraint;
			   void HasConstructorConstraint<T>() where T : new();
				TData? HasNullableValue<TData>(TData? data);
			}
			
			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<ITypeConstraints>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfITypeConstraintsExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::ITypeConstraints> Methods(this global::Rocks.Expectations.Expectations<global::ITypeConstraints> @self) =>
					new(@self);
				
				internal static global::ITypeConstraints Instance(this global::Rocks.Expectations.Expectations<global::ITypeConstraints> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockITypeConstraints(@self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockITypeConstraints
					: global::ITypeConstraints
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockITypeConstraints(global::Rocks.Expectations.Expectations<global::ITypeConstraints> @expectations) =>
						this.handlers = @expectations.Handlers;
					
					[global::Rocks.MemberIdentifier(0, "void HasUnmanaged<T>()")]
					public void HasUnmanaged<T>()
						where T : unmanaged
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void HasUnmanaged<T>()");
						}
					}
					
					[global::Rocks.MemberIdentifier(1, "void HasNotNull<T>()")]
					public void HasNotNull<T>()
						where T : notnull
					{
						if (this.handlers.TryGetValue(1, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void HasNotNull<T>()");
						}
					}
					
					[global::Rocks.MemberIdentifier(2, "void HasClass<T>()")]
					public void HasClass<T>()
						where T : class
					{
						if (this.handlers.TryGetValue(2, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void HasClass<T>()");
						}
					}
					
					[global::Rocks.MemberIdentifier(3, "void HasStruct<T>()")]
					public void HasStruct<T>()
						where T : struct
					{
						if (this.handlers.TryGetValue(3, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void HasStruct<T>()");
						}
					}
					
					[global::Rocks.MemberIdentifier(4, "void HasClassTypeConstraint<T>()")]
					public void HasClassTypeConstraint<T>()
						where T : global::ClassConstraint
					{
						if (this.handlers.TryGetValue(4, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void HasClassTypeConstraint<T>()");
						}
					}
					
					[global::Rocks.MemberIdentifier(5, "void HasInterfaceTypeConstraint<T>()")]
					public void HasInterfaceTypeConstraint<T>()
						where T : global::InterfaceConstraint
					{
						if (this.handlers.TryGetValue(5, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void HasInterfaceTypeConstraint<T>()");
						}
					}
					
					[global::Rocks.MemberIdentifier(6, "void HasConstructorConstraint<T>()")]
					public void HasConstructorConstraint<T>()
						where T : new()
					{
						if (this.handlers.TryGetValue(6, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void HasConstructorConstraint<T>()");
						}
					}
					
					[global::Rocks.MemberIdentifier(7, "TData? HasNullableValue<TData>(TData? @data)")]
					public TData? HasNullableValue<TData>(TData? @data)
					{
						if (this.handlers.TryGetValue(7, out var @methodHandlers))
						{
							foreach (var @methodHandler in @methodHandlers)
							{
								if (((@methodHandler.Expectations[0] as global::Rocks.Argument<TData?>)?.IsValid(@data) ?? false))
								{
									var @result = @methodHandler.Method is not null && @methodHandler.Method is global::System.Func<TData?, TData?> @methodReturn ?
										@methodReturn(@data) :
										@methodHandler is global::Rocks.HandlerInformation<TData?> @returnValue ?
											@returnValue.ReturnValue :
											throw new global::Rocks.Exceptions.MockException($"No return value could be obtained for TData of type {typeof(TData).FullName}.");
									@methodHandler.IncrementCallCount();
									return @result!;
								}
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers match for TData? HasNullableValue<TData>(TData? @data)");
						}
						
						throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for TData? HasNullableValue<TData>(TData? @data)");
					}
					
				}
			}
			
			internal static class MethodExpectationsOfITypeConstraintsExtensions
			{
				internal static global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action> HasUnmanaged<T>(this global::Rocks.Expectations.MethodExpectations<global::ITypeConstraints> @self) where T : unmanaged =>
					new global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action> HasNotNull<T>(this global::Rocks.Expectations.MethodExpectations<global::ITypeConstraints> @self) where T : notnull =>
					new global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action>(@self.Add(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action> HasClass<T>(this global::Rocks.Expectations.MethodExpectations<global::ITypeConstraints> @self) where T : class =>
					new global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action>(@self.Add(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action> HasStruct<T>(this global::Rocks.Expectations.MethodExpectations<global::ITypeConstraints> @self) where T : struct =>
					new global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action>(@self.Add(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action> HasClassTypeConstraint<T>(this global::Rocks.Expectations.MethodExpectations<global::ITypeConstraints> @self) where T : global::ClassConstraint =>
					new global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action>(@self.Add(4, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action> HasInterfaceTypeConstraint<T>(this global::Rocks.Expectations.MethodExpectations<global::ITypeConstraints> @self) where T : global::InterfaceConstraint =>
					new global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action>(@self.Add(5, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action> HasConstructorConstraint<T>(this global::Rocks.Expectations.MethodExpectations<global::ITypeConstraints> @self) where T : new() =>
					new global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Action>(@self.Add(6, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Func<TData?, TData?>, TData?> HasNullableValue<TData>(this global::Rocks.Expectations.MethodExpectations<global::ITypeConstraints> @self, global::Rocks.Argument<TData?> @data)
				{
					global::System.ArgumentNullException.ThrowIfNull(@data);
					return new global::Rocks.MethodAdornments<global::ITypeConstraints, global::System.Func<TData?, TData?>, TData?>(@self.Add<TData?>(7, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @data }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ITypeConstraints_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateMakeTargetingInterfaceAsync()
	{
		var code =
			"""
			#nullable enable
			
			using Rocks;

			public interface InterfaceConstraint { }
			public class ClassConstraint { }

			public interface ITypeConstraints
			{				
			   void HasUnmanaged<T>() where T : unmanaged;
			   void HasNotNull<T>() where T : notnull;
			   void HasClass<T>() where T : class;
			   void HasStruct<T>() where T : struct;
			   void HasClassTypeConstraint<T>() where T : ClassConstraint;
			   void HasInterfaceTypeConstraint<T>() where T : InterfaceConstraint;
			   void HasConstructorConstraint<T>() where T : new();
				TData? HasNullableValue<TData>(TData? data);
			}
			
			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Make<ITypeConstraints>();
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			internal static class MakeExpectationsOfITypeConstraintsExtensions
			{
				internal static global::ITypeConstraints Instance(this global::Rocks.MakeGeneration<global::ITypeConstraints> @self) =>
					new RockITypeConstraints();
				
				private sealed class RockITypeConstraints
					: global::ITypeConstraints
				{
					public RockITypeConstraints() { }
					
					public void HasUnmanaged<T>()
						where T : unmanaged
					{
					}
					public void HasNotNull<T>()
						where T : notnull
					{
					}
					public void HasClass<T>()
						where T : class
					{
					}
					public void HasStruct<T>()
						where T : struct
					{
					}
					public void HasClassTypeConstraint<T>()
						where T : global::ClassConstraint
					{
					}
					public void HasInterfaceTypeConstraint<T>()
						where T : global::InterfaceConstraint
					{
					}
					public void HasConstructorConstraint<T>()
						where T : new()
					{
					}
					public TData? HasNullableValue<TData>(TData? @data)
					{
						return default!;
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "ITypeConstraints_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateCreateTargetingClassAsync()
	{
		var code =
			"""
			#nullable enable
			
			using Rocks;

			public interface InterfaceConstraint { }
			public class ClassConstraint { }

			public class TypeConstraints
			{				
			   public virtual void HasUnmanaged<T>() where T : unmanaged { }
			   public virtual void HasNotNull<T>() where T : notnull { }
			   public virtual void HasClass<T>() where T : class { }
			   public virtual void HasStruct<T>() where T : struct { }
			   public virtual void HasClassTypeConstraint<T>() where T : ClassConstraint { }
			   public virtual void HasInterfaceTypeConstraint<T>() where T : InterfaceConstraint { }
			   public virtual void HasConstructorConstraint<T>() where T : new() { }
				public virtual TData? HasNullableValue<TData>(TData? data) => default!;
			}
			
			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<TypeConstraints>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfTypeConstraintsExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> Methods(this global::Rocks.Expectations.Expectations<global::TypeConstraints> @self) =>
					new(@self);
				
				internal static global::TypeConstraints Instance(this global::Rocks.Expectations.Expectations<global::TypeConstraints> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockTypeConstraints(@self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockTypeConstraints
					: global::TypeConstraints
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockTypeConstraints(global::Rocks.Expectations.Expectations<global::TypeConstraints> @expectations) =>
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
					
					[global::Rocks.MemberIdentifier(3, "void HasUnmanaged<T>()")]
					public override void HasUnmanaged<T>()
						where T : struct
					{
						if (this.handlers.TryGetValue(3, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							base.HasUnmanaged<T>();
						}
					}
					
					[global::Rocks.MemberIdentifier(4, "void HasNotNull<T>()")]
					public override void HasNotNull<T>()
					{
						if (this.handlers.TryGetValue(4, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							base.HasNotNull<T>();
						}
					}
					
					[global::Rocks.MemberIdentifier(5, "void HasClass<T>()")]
					public override void HasClass<T>()
						where T : class
					{
						if (this.handlers.TryGetValue(5, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							base.HasClass<T>();
						}
					}
					
					[global::Rocks.MemberIdentifier(6, "void HasStruct<T>()")]
					public override void HasStruct<T>()
						where T : struct
					{
						if (this.handlers.TryGetValue(6, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							base.HasStruct<T>();
						}
					}
					
					[global::Rocks.MemberIdentifier(7, "void HasClassTypeConstraint<T>()")]
					public override void HasClassTypeConstraint<T>()
					{
						if (this.handlers.TryGetValue(7, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							base.HasClassTypeConstraint<T>();
						}
					}
					
					[global::Rocks.MemberIdentifier(8, "void HasInterfaceTypeConstraint<T>()")]
					public override void HasInterfaceTypeConstraint<T>()
					{
						if (this.handlers.TryGetValue(8, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							base.HasInterfaceTypeConstraint<T>();
						}
					}
					
					[global::Rocks.MemberIdentifier(9, "void HasConstructorConstraint<T>()")]
					public override void HasConstructorConstraint<T>()
					{
						if (this.handlers.TryGetValue(9, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
							
							@methodHandler.IncrementCallCount();
						}
						else
						{
							base.HasConstructorConstraint<T>();
						}
					}
					
					[global::Rocks.MemberIdentifier(10, "TData? HasNullableValue<TData>(TData? @data)")]
					public override TData? HasNullableValue<TData>(TData? @data)
						where TData : default
					{
						if (this.handlers.TryGetValue(10, out var @methodHandlers))
						{
							foreach (var @methodHandler in @methodHandlers)
							{
								if (((@methodHandler.Expectations[0] as global::Rocks.Argument<TData?>)?.IsValid(@data) ?? false))
								{
									var @result = @methodHandler.Method is not null && @methodHandler.Method is global::System.Func<TData?, TData?> @methodReturn ?
										@methodReturn(@data) :
										@methodHandler is global::Rocks.HandlerInformation<TData?> @returnValue ?
											@returnValue.ReturnValue :
											throw new global::Rocks.Exceptions.MockException($"No return value could be obtained for TData of type {typeof(TData).FullName}.");
									@methodHandler.IncrementCallCount();
									return @result!;
								}
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers match for TData? HasNullableValue<TData>(TData? @data)");
						}
						else
						{
							return base.HasNullableValue<TData>(@data);
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfTypeConstraintsExtensions
			{
				internal static global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> @self, global::Rocks.Argument<object?> @obj)
				{
					global::System.ArgumentNullException.ThrowIfNull(@obj);
					return new global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
				}
				internal static global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> @self) =>
					new global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> @self) =>
					new global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action> HasUnmanaged<T>(this global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> @self) where T : unmanaged =>
					new global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action>(@self.Add(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action> HasNotNull<T>(this global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> @self) where T : notnull =>
					new global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action>(@self.Add(4, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action> HasClass<T>(this global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> @self) where T : class =>
					new global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action>(@self.Add(5, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action> HasStruct<T>(this global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> @self) where T : struct =>
					new global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action>(@self.Add(6, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action> HasClassTypeConstraint<T>(this global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> @self) where T : global::ClassConstraint =>
					new global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action>(@self.Add(7, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action> HasInterfaceTypeConstraint<T>(this global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> @self) where T : global::InterfaceConstraint =>
					new global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action>(@self.Add(8, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action> HasConstructorConstraint<T>(this global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> @self) where T : new() =>
					new global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Action>(@self.Add(9, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Func<TData?, TData?>, TData?> HasNullableValue<TData>(this global::Rocks.Expectations.MethodExpectations<global::TypeConstraints> @self, global::Rocks.Argument<TData?> @data)
				{
					global::System.ArgumentNullException.ThrowIfNull(@data);
					return new global::Rocks.MethodAdornments<global::TypeConstraints, global::System.Func<TData?, TData?>, TData?>(@self.Add<TData?>(10, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @data }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "TypeConstraints_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateMakeTargetingClassAsync()
	{
		var code =
			"""
			#nullable enable
			
			using Rocks;
			
			public interface InterfaceConstraint { }
			public class ClassConstraint { }
			
			public class TypeConstraints
			{				
			   public virtual void HasUnmanaged<T>() where T : unmanaged { }
			   public virtual void HasNotNull<T>() where T : notnull { }
			   public virtual void HasClass<T>() where T : class { }
			   public virtual void HasStruct<T>() where T : struct { }
			   public virtual void HasClassTypeConstraint<T>() where T : ClassConstraint { }
			   public virtual void HasInterfaceTypeConstraint<T>() where T : InterfaceConstraint { }
			   public virtual void HasConstructorConstraint<T>() where T : new() { }
				public virtual TData? HasNullableValue<TData>(TData? data) => default!;
			}
			
			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Make<TypeConstraints>();
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			internal static class MakeExpectationsOfTypeConstraintsExtensions
			{
				internal static global::TypeConstraints Instance(this global::Rocks.MakeGeneration<global::TypeConstraints> @self) =>
					new RockTypeConstraints();
				
				private sealed class RockTypeConstraints
					: global::TypeConstraints
				{
					public RockTypeConstraints() { }
					
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
					public override void HasUnmanaged<T>()
						where T : struct
					{
					}
					public override void HasNotNull<T>()
					{
					}
					public override void HasClass<T>()
						where T : class
					{
					}
					public override void HasStruct<T>()
						where T : struct
					{
					}
					public override void HasClassTypeConstraint<T>()
					{
					}
					public override void HasInterfaceTypeConstraint<T>()
					{
					}
					public override void HasConstructorConstraint<T>()
					{
					}
					public override TData? HasNullableValue<TData>(TData? @data)
						where TData : default
					{
						return default!;
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "TypeConstraints_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateProperNumberOfConstraintsAsync()
	{
		var code =
			"""
			using Rocks;
			
			public interface IValue<TValue>
			  where TValue : IValue<TValue> { }

			public class Value<TValue>
			  where TValue : unmanaged, IValue<TValue> { }

			public interface IUnmanagedValue
			{
			    void Use<TValue>(Value<TValue> value)
			        where TValue : unmanaged, IValue<TValue>;
			}

			public static class Test
			{
			  public static void Run()
			  {
			    var expectations = Rock.Create<IUnmanagedValue>();
			  }
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIUnmanagedValueExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::IUnmanagedValue> Methods(this global::Rocks.Expectations.Expectations<global::IUnmanagedValue> @self) =>
					new(@self);
				
				internal static global::IUnmanagedValue Instance(this global::Rocks.Expectations.Expectations<global::IUnmanagedValue> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockIUnmanagedValue(@self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIUnmanagedValue
					: global::IUnmanagedValue
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIUnmanagedValue(global::Rocks.Expectations.Expectations<global::IUnmanagedValue> @expectations) =>
						this.handlers = @expectations.Handlers;
					
					[global::Rocks.MemberIdentifier(0, "void Use<TValue>(global::Value<TValue> @value)")]
					public void Use<TValue>(global::Value<TValue> @value)
						where TValue : unmanaged, global::IValue<TValue>
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
						{
							var @foundMatch = false;
							
							foreach (var @methodHandler in @methodHandlers)
							{
								if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<global::Value<TValue>>>(@methodHandler.Expectations[0]).IsValid(@value))
								{
									@foundMatch = true;
									
									if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action<global::Value<TValue>> @method)
									{
										@method(@value);
									}
									
									@methodHandler.IncrementCallCount();
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void Use<TValue>(global::Value<TValue> @value)");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Use<TValue>(global::Value<TValue> @value)");
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfIUnmanagedValueExtensions
			{
				internal static global::Rocks.MethodAdornments<global::IUnmanagedValue, global::System.Action<global::Value<TValue>>> Use<TValue>(this global::Rocks.Expectations.MethodExpectations<global::IUnmanagedValue> @self, global::Rocks.Argument<global::Value<TValue>> @value) where TValue : unmanaged, global::IValue<TValue>
				{
					global::System.ArgumentNullException.ThrowIfNull(@value);
					return new global::Rocks.MethodAdornments<global::IUnmanagedValue, global::System.Action<global::Value<TValue>>>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IUnmanagedValue_Rock_Create.g.cs", generatedCode) },
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
				public class Thing { }

				public abstract class Thing<T>
					: Thing where T : class
				{
					public abstract Thing<TTarget> As<TTarget>() where TTarget : class;
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<Thing<string>>();
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
				internal static class CreateExpectationsOfThingOfstringExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.Thing<string>> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.Thing<string>> @self) =>
						new(@self);
					
					internal static global::MockTests.Thing<string> Instance(this global::Rocks.Expectations.Expectations<global::MockTests.Thing<string>> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							return new RockThingOfstring(@self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockThingOfstring
						: global::MockTests.Thing<string>
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockThingOfstring(global::Rocks.Expectations.Expectations<global::MockTests.Thing<string>> @expectations) =>
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
						
						[global::Rocks.MemberIdentifier(3, "global::MockTests.Thing<TTarget> As<TTarget>()")]
						public override global::MockTests.Thing<TTarget> As<TTarget>()
							where TTarget : class
						{
							if (this.handlers.TryGetValue(3, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<global::MockTests.Thing<TTarget>>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<global::MockTests.Thing<TTarget>>>(@methodHandler).ReturnValue;
								@methodHandler.IncrementCallCount();
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::MockTests.Thing<TTarget> As<TTarget>()");
						}
						
					}
				}
				
				internal static class MethodExpectationsOfThingOfstringExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Thing<string>> @self, global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						return new global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Thing<string>> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Thing<string>> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<global::MockTests.Thing<TTarget>>, global::MockTests.Thing<TTarget>> As<TTarget>(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Thing<string>> @self) where TTarget : class =>
						new global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<global::MockTests.Thing<TTarget>>, global::MockTests.Thing<TTarget>>(@self.Add<global::MockTests.Thing<TTarget>>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ThingOfstring_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}