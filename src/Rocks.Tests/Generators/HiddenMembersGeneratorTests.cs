﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class HiddenMembersGeneratorTests
{
	[Test]
	public static async Task GenerateWithSubclassHidingAndReturnTypeIsDifferentAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<MockTests.NewCopy>]

			#nullable enable

			namespace MockTests
			{
				public class Credentials { }

				public class NewCredentials : Credentials { }
			
				public class BaseCopy
				{
					public virtual void Ok() { }
					public virtual Credentials Copy() => new();
				}

				public class NewCopy : BaseCopy
				{
					public new NewCredentials Copy() => new();
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
				internal sealed class NewCopyCreateExpectations
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
					
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<string?>, string?>
					{ }
					
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Action>
					{ }
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.NewCopyCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.NewCopyCreateExpectations.Handler1> @handlers1 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.NewCopyCreateExpectations.Handler2> @handlers2 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.NewCopyCreateExpectations.Handler3> @handlers3 = new();
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							failures.AddRange(this.Verify(handlers0));
							failures.AddRange(this.Verify(handlers1));
							failures.AddRange(this.Verify(handlers2));
							failures.AddRange(this.Verify(handlers3));
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class RockNewCopy
						: global::MockTests.NewCopy
					{
						public RockNewCopy(global::MockTests.NewCopyCreateExpectations @expectations)
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
						
						[global::Rocks.MemberIdentifier(2, "string? ToString()")]
						public override string? ToString()
						{
							if (this.Expectations.handlers2.Count > 0)
							{
								var @handler = this.Expectations.handlers2[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							else
							{
								return base.ToString();
							}
						}
						
						[global::Rocks.MemberIdentifier(3, "void Ok()")]
						public override void Ok()
						{
							if (this.Expectations.handlers3.Count > 0)
							{
								var @handler = this.Expectations.handlers3[0];
								@handler.CallCount++;
								@handler.Callback?.Invoke();
							}
							else
							{
								base.Ok();
							}
						}
						
						private global::MockTests.NewCopyCreateExpectations Expectations { get; }
					}
					
					internal sealed class NewCopyMethodExpectations
					{
						internal NewCopyMethodExpectations(global::MockTests.NewCopyCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.NewCopyCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
						{
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var handler = new global::MockTests.NewCopyCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.NewCopyCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
						{
							var handler = new global::MockTests.NewCopyCreateExpectations.Handler1();
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.NewCopyCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
						{
							var handler = new global::MockTests.NewCopyCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.NewCopyCreateExpectations.Handler3, global::System.Action> Ok()
						{
							var handler = new global::MockTests.NewCopyCreateExpectations.Handler3();
							this.Expectations.handlers3.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.NewCopyCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.NewCopyCreateExpectations.NewCopyMethodExpectations Methods { get; }
					
					internal NewCopyCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.NewCopy Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockNewCopy(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.NewCopy_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithClassWhenMembersAreHiddenAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			[assembly: RockCreate<SubClass>]

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
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			internal sealed class SubClassCreateExpectations
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
				
				internal sealed class Handler2
					: global::Rocks.Handler<global::System.Func<string?>, string?>
				{ }
				
				internal sealed class Handler4
					: global::Rocks.Handler<global::System.Func<int>, int>
				{ }
				
				internal sealed class Handler8
					: global::Rocks.Handler<global::System.Func<int>, int>
				{ }
				
				internal sealed class Handler9
					: global::Rocks.Handler<global::System.Action<int>>
				{
					public global::Rocks.Argument<int> @value { get; set; }
				}
				
				internal sealed class Handler10
					: global::Rocks.Handler<global::System.Func<int, string, int>, int>
				{
					public global::Rocks.Argument<int> @a { get; set; }
					public global::Rocks.Argument<string> @b { get; set; }
				}
				
				#pragma warning restore CS8618
				
				private readonly global::System.Collections.Generic.List<global::SubClassCreateExpectations.Handler0> @handlers0 = new();
				private readonly global::System.Collections.Generic.List<global::SubClassCreateExpectations.Handler1> @handlers1 = new();
				private readonly global::System.Collections.Generic.List<global::SubClassCreateExpectations.Handler2> @handlers2 = new();
				private readonly global::System.Collections.Generic.List<global::SubClassCreateExpectations.Handler4> @handlers4 = new();
				private readonly global::System.Collections.Generic.List<global::SubClassCreateExpectations.Handler8> @handlers8 = new();
				private readonly global::System.Collections.Generic.List<global::SubClassCreateExpectations.Handler9> @handlers9 = new();
				private readonly global::System.Collections.Generic.List<global::SubClassCreateExpectations.Handler10> @handlers10 = new();
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						failures.AddRange(this.Verify(handlers0));
						failures.AddRange(this.Verify(handlers1));
						failures.AddRange(this.Verify(handlers2));
						failures.AddRange(this.Verify(handlers4));
						failures.AddRange(this.Verify(handlers8));
						failures.AddRange(this.Verify(handlers9));
						failures.AddRange(this.Verify(handlers10));
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class RockSubClass
					: global::SubClass
				{
					public RockSubClass(global::SubClassCreateExpectations @expectations)
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
					
					[global::Rocks.MemberIdentifier(2, "string? ToString()")]
					public override string? ToString()
					{
						if (this.Expectations.handlers2.Count > 0)
						{
							var @handler = this.Expectations.handlers2[0];
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
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
						if (this.Expectations.handlers4.Count > 0)
						{
							var @handler = this.Expectations.handlers4[0];
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						else
						{
							return base.Foo();
						}
					}
					
					[global::Rocks.MemberIdentifier(8, "get_Data()")]
					[global::Rocks.MemberIdentifier(9, "set_Data(value)")]
					public override int Data
					{
						get
						{
							if (this.Expectations.handlers8.Count > 0)
							{
								var @handler = this.Expectations.handlers8[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							else
							{
								return base.Data;
							}
						}
						set
						{
							if (this.Expectations.handlers9.Count > 0)
							{
								var @foundMatch = false;
								foreach (var @handler in this.Expectations.handlers9)
								{
									if (@handler.value.IsValid(value!))
									{
										@handler.CallCount++;
										@foundMatch = true;
										@handler.Callback?.Invoke(value!);
										
										if (!@foundMatch)
										{
											throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_Data(value)");
										}
										
										break;
									}
								}
							}
							else
							{
								base.Data = value!;
							}
						}
					}
					
					[global::Rocks.MemberIdentifier(10, "this[int @a, string @b]")]
					public override int this[int @a, string @b]
					{
						get
						{
							if (this.Expectations.handlers10.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers10)
								{
									if (@handler.@a.IsValid(@a!) &&
										@handler.@b.IsValid(@b!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@a!, @b!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[int @a, string @b]");
							}
							else
							{
								return base[a: @a!, b: @b!];
							}
						}
					}
					
					private global::SubClassCreateExpectations Expectations { get; }
				}
				
				internal sealed class SubClassMethodExpectations
				{
					internal SubClassMethodExpectations(global::SubClassCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::Rocks.Adornments<global::SubClassCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						
						var handler = new global::SubClassCreateExpectations.Handler0
						{
							@obj = @obj,
						};
						
						this.Expectations.handlers0.Add(handler);
						return new(handler);
					}
					
					internal new global::Rocks.Adornments<global::SubClassCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
					{
						var handler = new global::SubClassCreateExpectations.Handler1();
						this.Expectations.handlers1.Add(handler);
						return new(handler);
					}
					
					internal new global::Rocks.Adornments<global::SubClassCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
					{
						var handler = new global::SubClassCreateExpectations.Handler2();
						this.Expectations.handlers2.Add(handler);
						return new(handler);
					}
					
					internal global::Rocks.Adornments<global::SubClassCreateExpectations.Handler4, global::System.Func<int>, int> Foo()
					{
						var handler = new global::SubClassCreateExpectations.Handler4();
						this.Expectations.handlers4.Add(handler);
						return new(handler);
					}
					
					private global::SubClassCreateExpectations Expectations { get; }
				}
				
				internal sealed class SubClassPropertyExpectations
				{
					internal sealed class SubClassPropertyGetterExpectations
					{
						internal SubClassPropertyGetterExpectations(global::SubClassCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::SubClassCreateExpectations.Handler8, global::System.Func<int>, int> Data()
						{
							var handler = new global::SubClassCreateExpectations.Handler8();
							this.Expectations.handlers8.Add(handler);
							return new(handler);
						}
						private global::SubClassCreateExpectations Expectations { get; }
					}
					
					internal sealed class SubClassPropertySetterExpectations
					{
						internal SubClassPropertySetterExpectations(global::SubClassCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::SubClassCreateExpectations.Handler9, global::System.Action<int>> Data(global::Rocks.Argument<int> @value)
						{
							var handler = new global::SubClassCreateExpectations.Handler9
							{
								value = @value,
							};
						
							this.Expectations.handlers9.Add(handler);
							return new(handler);
						}
						private global::SubClassCreateExpectations Expectations { get; }
					}
					
					internal SubClassPropertyExpectations(global::SubClassCreateExpectations expectations) =>
						(this.Getters, this.Setters) = (new(expectations), new(expectations));
					
					internal global::SubClassCreateExpectations.SubClassPropertyExpectations.SubClassPropertyGetterExpectations Getters { get; }
					internal global::SubClassCreateExpectations.SubClassPropertyExpectations.SubClassPropertySetterExpectations Setters { get; }
				}
				internal sealed class SubClassIndexerExpectations
				{
					internal sealed class SubClassIndexerGetterExpectations
					{
						internal SubClassIndexerGetterExpectations(global::SubClassCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::SubClassCreateExpectations.Handler10, global::System.Func<int, string, int>, int> This(global::Rocks.Argument<int> @a, global::Rocks.Argument<string> @b)
						{
							global::System.ArgumentNullException.ThrowIfNull(@a);
							global::System.ArgumentNullException.ThrowIfNull(@b);
							
							var handler = new global::SubClassCreateExpectations.Handler10
							{
								@a = @a,
								@b = @b,
							};
							
							this.Expectations.handlers10.Add(handler);
							return new(handler);
						}
						private global::SubClassCreateExpectations Expectations { get; }
					}
					
					
					internal SubClassIndexerExpectations(global::SubClassCreateExpectations expectations) =>
						(this.Getters) = (new(expectations));
					
					internal global::SubClassCreateExpectations.SubClassIndexerExpectations.SubClassIndexerGetterExpectations Getters { get; }
				}
				
				internal global::SubClassCreateExpectations.SubClassMethodExpectations Methods { get; }
				internal global::SubClassCreateExpectations.SubClassPropertyExpectations Properties { get; }
				internal global::SubClassCreateExpectations.SubClassIndexerExpectations Indexers { get; }
				
				internal SubClassCreateExpectations() =>
					(this.Methods, this.Properties, this.Indexers) = (new(this), new(this), new(this));
				
				internal global::SubClass Instance()
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockSubClass(this);
						this.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "SubClass_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithInterfaceWhenMembersAreHiddenAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<ISub>]

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
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			internal sealed class ISubCreateExpectations
				: global::Rocks.Expectations
			{
				#pragma warning disable CS8618
				
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Func<int>, int>
				{ }
				
				internal sealed class Handler1
					: global::Rocks.Handler<global::System.Action>
				{ }
				
				internal sealed class Handler2
					: global::Rocks.Handler<global::System.Func<int>, int>
				{ }
				
				internal sealed class Handler3
					: global::Rocks.Handler<global::System.Action<int>>
				{
					public global::Rocks.Argument<int> @value { get; set; }
				}
				
				internal sealed class Handler4
					: global::Rocks.Handler<global::System.Func<int, string, int>, int>
				{
					public global::Rocks.Argument<int> @a { get; set; }
					public global::Rocks.Argument<string> @b { get; set; }
				}
				
				internal sealed class Handler5
					: global::Rocks.Handler<global::System.Func<string>, string>
				{ }
				
				internal sealed class Handler6
					: global::Rocks.Handler<global::System.Action<string>>
				{
					public global::Rocks.Argument<string> @value { get; set; }
				}
				
				internal sealed class Handler7
					: global::Rocks.Handler<global::System.Func<int, string, string>, string>
				{
					public global::Rocks.Argument<int> @a { get; set; }
					public global::Rocks.Argument<string> @b { get; set; }
				}
				
				#pragma warning restore CS8618
				
				private readonly global::System.Collections.Generic.List<global::ISubCreateExpectations.Handler0> @handlers0 = new();
				private readonly global::System.Collections.Generic.List<global::ISubCreateExpectations.Handler1> @handlers1 = new();
				private readonly global::System.Collections.Generic.List<global::ISubCreateExpectations.Handler2> @handlers2 = new();
				private readonly global::System.Collections.Generic.List<global::ISubCreateExpectations.Handler3> @handlers3 = new();
				private readonly global::System.Collections.Generic.List<global::ISubCreateExpectations.Handler4> @handlers4 = new();
				private readonly global::System.Collections.Generic.List<global::ISubCreateExpectations.Handler5> @handlers5 = new();
				private readonly global::System.Collections.Generic.List<global::ISubCreateExpectations.Handler6> @handlers6 = new();
				private readonly global::System.Collections.Generic.List<global::ISubCreateExpectations.Handler7> @handlers7 = new();
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						failures.AddRange(this.Verify(handlers0));
						failures.AddRange(this.Verify(handlers1));
						failures.AddRange(this.Verify(handlers2));
						failures.AddRange(this.Verify(handlers3));
						failures.AddRange(this.Verify(handlers4));
						failures.AddRange(this.Verify(handlers5));
						failures.AddRange(this.Verify(handlers6));
						failures.AddRange(this.Verify(handlers7));
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class RockISub
					: global::ISub
				{
					public RockISub(global::ISubCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0, "int Foo()")]
					public int Foo()
					{
						if (this.Expectations.handlers0.Count > 0)
						{
							var @handler = this.Expectations.handlers0[0];
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int Foo()");
					}
					
					[global::Rocks.MemberIdentifier(1, "void global::IBase.Foo()")]
					void global::IBase.Foo()
					{
						if (this.Expectations.handlers1.Count > 0)
						{
							var @handler = this.Expectations.handlers1[0];
							@handler.CallCount++;
							@handler.Callback?.Invoke();
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void global::IBase.Foo()");
						}
					}
					
					[global::Rocks.MemberIdentifier(2, "get_Data()")]
					[global::Rocks.MemberIdentifier(3, "set_Data(value)")]
					public int Data
					{
						get
						{
							if (this.Expectations.handlers2.Count > 0)
							{
								var @handler = this.Expectations.handlers2[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_Data())");
						}
						set
						{
							if (this.Expectations.handlers3.Count > 0)
							{
								var @foundMatch = false;
								foreach (var @handler in this.Expectations.handlers3)
								{
									if (@handler.value.IsValid(value!))
									{
										@handler.CallCount++;
										@foundMatch = true;
										@handler.Callback?.Invoke(value!);
										
										if (!@foundMatch)
										{
											throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_Data(value)");
										}
										
										break;
									}
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for set_Data(value)");
							}
						}
					}
					[global::Rocks.MemberIdentifier(5, "global::IBase.get_Data()")]
					[global::Rocks.MemberIdentifier(6, "global::IBase.set_Data(value)")]
					string global::IBase.Data
					{
						get
						{
							if (this.Expectations.handlers5.Count > 0)
							{
								var @handler = this.Expectations.handlers5[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::IBase.get_Data())");
						}
						set
						{
							if (this.Expectations.handlers6.Count > 0)
							{
								var @foundMatch = false;
								foreach (var @handler in this.Expectations.handlers6)
								{
									if (@handler.value.IsValid(value!))
									{
										@handler.CallCount++;
										@foundMatch = true;
										@handler.Callback?.Invoke(value!);
										
										if (!@foundMatch)
										{
											throw new global::Rocks.Exceptions.ExpectationException("No handlers match for global::IBase.set_Data(value)");
										}
										
										break;
									}
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::IBase.set_Data(value)");
							}
						}
					}
					
					[global::Rocks.MemberIdentifier(4, "this[int @a, string @b]")]
					public int this[int @a, string @b]
					{
						get
						{
							if (this.Expectations.handlers4.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers4)
								{
									if (@handler.@a.IsValid(@a!) &&
										@handler.@b.IsValid(@b!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@a!, @b!) : @handler.ReturnValue;
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
							if (this.Expectations.handlers7.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers7)
								{
									if (@handler.@a.IsValid(@a!) &&
										@handler.@b.IsValid(@b!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@a!, @b!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[int @a, string @b]");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[int @a, string @b])");
						}
					}
					
					private global::ISubCreateExpectations Expectations { get; }
				}
				
				internal sealed class ISubMethodExpectations
				{
					internal ISubMethodExpectations(global::ISubCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::Rocks.Adornments<global::ISubCreateExpectations.Handler0, global::System.Func<int>, int> Foo()
					{
						var handler = new global::ISubCreateExpectations.Handler0();
						this.Expectations.handlers0.Add(handler);
						return new(handler);
					}
					
					private global::ISubCreateExpectations Expectations { get; }
				}
				internal sealed class ISubExplicitMethodExpectationsForIBase
				{
					internal ISubExplicitMethodExpectationsForIBase(global::ISubCreateExpectations expectations) =>
						this.Expectations = expectations;
				
					internal global::Rocks.Adornments<global::ISubCreateExpectations.Handler1, global::System.Action> Foo()
					{
						var handler = new global::ISubCreateExpectations.Handler1();
						this.Expectations.handlers1.Add(handler);
						return new(handler);
					}
					
					private global::ISubCreateExpectations Expectations { get; }
				}
				
				internal sealed class ISubPropertyExpectations
				{
					internal sealed class ISubPropertyGetterExpectations
					{
						internal ISubPropertyGetterExpectations(global::ISubCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::ISubCreateExpectations.Handler2, global::System.Func<int>, int> Data()
						{
							var handler = new global::ISubCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						private global::ISubCreateExpectations Expectations { get; }
					}
					
					internal sealed class ISubPropertySetterExpectations
					{
						internal ISubPropertySetterExpectations(global::ISubCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::ISubCreateExpectations.Handler3, global::System.Action<int>> Data(global::Rocks.Argument<int> @value)
						{
							var handler = new global::ISubCreateExpectations.Handler3
							{
								value = @value,
							};
						
							this.Expectations.handlers3.Add(handler);
							return new(handler);
						}
						private global::ISubCreateExpectations Expectations { get; }
					}
					
					internal ISubPropertyExpectations(global::ISubCreateExpectations expectations) =>
						(this.Getters, this.Setters) = (new(expectations), new(expectations));
					
					internal global::ISubCreateExpectations.ISubPropertyExpectations.ISubPropertyGetterExpectations Getters { get; }
					internal global::ISubCreateExpectations.ISubPropertyExpectations.ISubPropertySetterExpectations Setters { get; }
				}
				internal sealed class ISubIndexerExpectations
				{
					internal sealed class ISubIndexerGetterExpectations
					{
						internal ISubIndexerGetterExpectations(global::ISubCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::ISubCreateExpectations.Handler4, global::System.Func<int, string, int>, int> This(global::Rocks.Argument<int> @a, global::Rocks.Argument<string> @b)
						{
							global::System.ArgumentNullException.ThrowIfNull(@a);
							global::System.ArgumentNullException.ThrowIfNull(@b);
							
							var handler = new global::ISubCreateExpectations.Handler4
							{
								@a = @a,
								@b = @b,
							};
							
							this.Expectations.handlers4.Add(handler);
							return new(handler);
						}
						private global::ISubCreateExpectations Expectations { get; }
					}
					
					
					internal ISubIndexerExpectations(global::ISubCreateExpectations expectations) =>
						(this.Getters) = (new(expectations));
					
					internal global::ISubCreateExpectations.ISubIndexerExpectations.ISubIndexerGetterExpectations Getters { get; }
				}
				internal sealed class ISubExplicitPropertyExpectationsForIBase
				{
					internal sealed class ISubExplicitPropertyGetterExpectationsForIBase
					{
						internal ISubExplicitPropertyGetterExpectationsForIBase(global::ISubCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::ISubCreateExpectations.Handler5, global::System.Func<string>, string> Data()
						{
							var handler = new global::ISubCreateExpectations.Handler5();
							this.Expectations.handlers5.Add(handler);
							return new(handler);
						}
						private global::ISubCreateExpectations Expectations { get; }
					}
					internal sealed class ISubExplicitPropertySetterExpectationsForIBase
					{
						internal ISubExplicitPropertySetterExpectationsForIBase(global::ISubCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::ISubCreateExpectations.Handler6, global::System.Action<string>> Data(global::Rocks.Argument<string> @value)
						{
							var handler = new global::ISubCreateExpectations.Handler6
							{
								value = @value,
							};
						
							this.Expectations.handlers6.Add(handler);
							return new(handler);
						}
						private global::ISubCreateExpectations Expectations { get; }
					}
					
					internal ISubExplicitPropertyExpectationsForIBase(global::ISubCreateExpectations expectations) =>
						(this.Getters, this.Setters) = (new(expectations), new(expectations));
					
					internal global::ISubCreateExpectations.ISubExplicitPropertyExpectationsForIBase.ISubExplicitPropertyGetterExpectationsForIBase Getters { get; }
					internal global::ISubCreateExpectations.ISubExplicitPropertyExpectationsForIBase.ISubExplicitPropertySetterExpectationsForIBase Setters { get; }
				}
				internal sealed class ISubExplicitIndexerExpectationsForIBase
				{
					internal sealed class ISubExplicitIndexerGetterExpectationsForIBase
					{
						internal ISubExplicitIndexerGetterExpectationsForIBase(global::ISubCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::ISubCreateExpectations.Handler7, global::System.Func<int, string, string>, string> This(global::Rocks.Argument<int> @a, global::Rocks.Argument<string> @b)
						{
							global::System.ArgumentNullException.ThrowIfNull(@a);
							global::System.ArgumentNullException.ThrowIfNull(@b);
							
							var handler = new global::ISubCreateExpectations.Handler7
							{
								@a = @a,
								@b = @b,
							};
							
							this.Expectations.handlers7.Add(handler);
							return new(handler);
						}
						private global::ISubCreateExpectations Expectations { get; }
					}
					
					internal ISubExplicitIndexerExpectationsForIBase(global::ISubCreateExpectations expectations) =>
						(this.Getters) = (new(expectations));
					
					internal global::ISubCreateExpectations.ISubExplicitIndexerExpectationsForIBase.ISubExplicitIndexerGetterExpectationsForIBase Getters { get; }
				}
				
				internal global::ISubCreateExpectations.ISubMethodExpectations Methods { get; }
				internal global::ISubCreateExpectations.ISubExplicitMethodExpectationsForIBase ExplicitMethodsForIBase { get; }
				internal global::ISubCreateExpectations.ISubPropertyExpectations Properties { get; }
				internal global::ISubCreateExpectations.ISubIndexerExpectations Indexers { get; }
				internal global::ISubCreateExpectations.ISubExplicitPropertyExpectationsForIBase ExplicitPropertiesForIBase { get; }
				internal global::ISubCreateExpectations.ISubExplicitIndexerExpectationsForIBase ExplicitIndexersForIBase { get; }
				
				internal ISubCreateExpectations() =>
					(this.Methods, this.ExplicitMethodsForIBase, this.Properties, this.Indexers, this.ExplicitPropertiesForIBase, this.ExplicitIndexersForIBase) = (new(this), new(this), new(this), new(this), new(this), new(this));
				
				internal global::ISub Instance()
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockISub(this);
						this.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "ISub_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}
}