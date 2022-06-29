using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class AllowNullGeneratorTests
{
	[Test]
	public static async Task GenerateAbstractCreateAsync()
	{
		var code =
@"using Rocks;
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
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using Rocks.Expectations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfIAllowExtensions
	{
		internal static PropertyExpectations<IAllow> Properties(this Expectations<IAllow> self) =>
			new(self);
		
		internal static PropertyGetterExpectations<IAllow> Getters(this PropertyExpectations<IAllow> self) =>
			new(self.Expectations);
		
		internal static PropertySetterExpectations<IAllow> Setters(this PropertyExpectations<IAllow> self) =>
			new(self.Expectations);
		
		internal static IAllow Instance(this Expectations<IAllow> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockIAllow(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockIAllow
			: IAllow, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockIAllow(Expectations<IAllow> expectations) =>
				this.handlers = expectations.Handlers;
			
			[AllowNull]
			[MemberIdentifier(0, ""get_NewLine()"")]
			[MemberIdentifier(1, ""set_NewLine(value)"")]
			public string NewLine
			{
				get
				{
					if (this.handlers.TryGetValue(0, out var methodHandlers))
					{
						var methodHandler = methodHandlers[0];
						var result = methodHandler.Method is not null ?
							((Func<string>)methodHandler.Method)() :
							((HandlerInformation<string>)methodHandler).ReturnValue;
						methodHandler.IncrementCallCount();
						return result!;
					}
					
					throw new ExpectationException(""No handlers were found for get_NewLine())"");
				}
				set
				{
					if (this.handlers.TryGetValue(1, out var methodHandlers))
					{
						var foundMatch = false;
						foreach (var methodHandler in methodHandlers)
						{
							if ((methodHandler.Expectations[0] as Argument<string>)?.IsValid(value!) ?? false)
							{
								foundMatch = true;
								
								if (methodHandler.Method is not null)
								{
									((Action<string>)methodHandler.Method)(value!);
								}
								
								if (!foundMatch)
								{
									throw new ExpectationException(""No handlers match for set_NewLine(value)"");
								}
								
								methodHandler.IncrementCallCount();
								break;
							}
						}
					}
					else
					{
						throw new ExpectationException(""No handlers were found for set_NewLine(value)"");
					}
				}
			}
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class PropertyGetterExpectationsOfIAllowExtensions
	{
		internal static PropertyAdornments<IAllow, Func<string>, string> NewLine(this PropertyGetterExpectations<IAllow> self) =>
			new PropertyAdornments<IAllow, Func<string>, string>(self.Add<string>(0, new List<Argument>()));
	}
	internal static class PropertySetterExpectationsOfIAllowExtensions
	{
		internal static PropertyAdornments<IAllow, Action<string>> NewLine(this PropertySetterExpectations<IAllow> self, Argument<string> value) =>
			new PropertyAdornments<IAllow, Action<string>>(self.Add(1, new List<Argument>(1) { value }));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IAllow_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateAbstractMakeAsync()
	{
		var code =
@"using Rocks;
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
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfIAllowExtensions
	{
		internal static IAllow Instance(this MakeGeneration<IAllow> self) =>
			new RockIAllow();
		
		private sealed class RockIAllow
			: IAllow
		{
			public RockIAllow() { }
			
			[AllowNull]
			public string NewLine
			{
				get => default!;
				set { }
			}
		}
	}
}
";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "IAllow_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateNonAbstractCreateAsync()
	{
		var code =
@"using Rocks;
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
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using Rocks.Expectations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfAllowExtensions
	{
		internal static MethodExpectations<Allow> Methods(this Expectations<Allow> self) =>
			new(self);
		
		internal static PropertyExpectations<Allow> Properties(this Expectations<Allow> self) =>
			new(self);
		
		internal static PropertyGetterExpectations<Allow> Getters(this PropertyExpectations<Allow> self) =>
			new(self.Expectations);
		
		internal static PropertySetterExpectations<Allow> Setters(this PropertyExpectations<Allow> self) =>
			new(self.Expectations);
		
		internal static Allow Instance(this Expectations<Allow> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockAllow(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockAllow
			: Allow, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockAllow(Expectations<Allow> expectations) =>
				this.handlers = expectations.Handlers;
			
			[MemberIdentifier(0, ""bool Equals(object? obj)"")]
			public override bool Equals(object? obj)
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					foreach (var methodHandler in methodHandlers)
					{
						if (((methodHandler.Expectations[0] as Argument<object?>)?.IsValid(obj) ?? false))
						{
							var result = methodHandler.Method is not null ?
								((Func<object?, bool>)methodHandler.Method)(obj) :
								((HandlerInformation<bool>)methodHandler).ReturnValue;
							methodHandler.IncrementCallCount();
							return result!;
						}
					}
					
					throw new ExpectationException(""No handlers match for bool Equals(object? obj)"");
				}
				else
				{
					return base.Equals(obj);
				}
			}
			
			[MemberIdentifier(1, ""int GetHashCode()"")]
			public override int GetHashCode()
			{
				if (this.handlers.TryGetValue(1, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<int>)methodHandler.Method)() :
						((HandlerInformation<int>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					return result!;
				}
				else
				{
					return base.GetHashCode();
				}
			}
			
			[MemberIdentifier(2, ""string? ToString()"")]
			public override string? ToString()
			{
				if (this.handlers.TryGetValue(2, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<string?>)methodHandler.Method)() :
						((HandlerInformation<string?>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					return result!;
				}
				else
				{
					return base.ToString();
				}
			}
			
			[AllowNull]
			[MemberIdentifier(3, ""get_NewLine()"")]
			[MemberIdentifier(4, ""set_NewLine(value)"")]
			public override string NewLine
			{
				get
				{
					if (this.handlers.TryGetValue(3, out var methodHandlers))
					{
						var methodHandler = methodHandlers[0];
						var result = methodHandler.Method is not null ?
							((Func<string>)methodHandler.Method)() :
							((HandlerInformation<string>)methodHandler).ReturnValue;
						methodHandler.IncrementCallCount();
						return result!;
					}
					else
					{
						return base.NewLine;
					}
				}
				set
				{
					if (this.handlers.TryGetValue(4, out var methodHandlers))
					{
						var foundMatch = false;
						foreach (var methodHandler in methodHandlers)
						{
							if ((methodHandler.Expectations[0] as Argument<string>)?.IsValid(value!) ?? false)
							{
								foundMatch = true;
								
								if (methodHandler.Method is not null)
								{
									((Action<string>)methodHandler.Method)(value!);
								}
								
								if (!foundMatch)
								{
									throw new ExpectationException(""No handlers match for set_NewLine(value)"");
								}
								
								methodHandler.IncrementCallCount();
								break;
							}
						}
					}
					else
					{
						base.NewLine = value;
					}
				}
			}
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfAllowExtensions
	{
		internal static MethodAdornments<Allow, Func<object?, bool>, bool> Equals(this MethodExpectations<Allow> self, Argument<object?> obj) =>
			new MethodAdornments<Allow, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<Allow, Func<int>, int> GetHashCode(this MethodExpectations<Allow> self) =>
			new MethodAdornments<Allow, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<Allow, Func<string?>, string?> ToString(this MethodExpectations<Allow> self) =>
			new MethodAdornments<Allow, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
	}
	
	internal static class PropertyGetterExpectationsOfAllowExtensions
	{
		internal static PropertyAdornments<Allow, Func<string>, string> NewLine(this PropertyGetterExpectations<Allow> self) =>
			new PropertyAdornments<Allow, Func<string>, string>(self.Add<string>(3, new List<Argument>()));
	}
	internal static class PropertySetterExpectationsOfAllowExtensions
	{
		internal static PropertyAdornments<Allow, Action<string>> NewLine(this PropertySetterExpectations<Allow> self, Argument<string> value) =>
			new PropertyAdornments<Allow, Action<string>>(self.Add(4, new List<Argument>(1) { value }));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Allow_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateNonAbstractMakeAsync()
	{
		var code =
@"using Rocks;
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
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfAllowExtensions
	{
		internal static Allow Instance(this MakeGeneration<Allow> self) =>
			new RockAllow();
		
		private sealed class RockAllow
			: Allow
		{
			public RockAllow() { }
			
			public override bool Equals(object? obj)
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
			[AllowNull]
			public override string NewLine
			{
				get => default!;
				set { }
			}
		}
	}
}
";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "Allow_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}