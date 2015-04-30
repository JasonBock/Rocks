using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Exceptions;
using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static Rocks.Extensions.ConstructorInfoExtensions;
using static Rocks.Extensions.MethodBaseExtensions;
using static Rocks.Extensions.MethodInfoExtensions;
using static Rocks.Extensions.PropertyInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction
{
	internal abstract class Builder
	{
		internal Builder(Type baseType,
			ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, Options options, NameGenerator generator)
		{
			this.BaseType = baseType;
			this.IsUnsafe = this.BaseType.IsUnsafeToMock();
			this.Handlers = handlers;
			this.Namespaces = namespaces;
			this.Options = options;
			this.NameGenerator = generator;
		}

		internal virtual void Build()
		{
			this.Tree = this.MakeTree();
		}

		private List<string> GetGeneratedConstructors()
		{
			var generatedConstructors = new List<string>();
			var constructorName = this.GetTypeNameWithNoGenerics();

			if (this.BaseType.IsInterface)
			{
				generatedConstructors.Add(CodeTemplates.GetConstructorTemplate(
					constructorName, string.Empty, string.Empty));
			}
			else
			{
				foreach (var constructor in this.BaseType.GetConstructors(ReflectionValues.PublicNonPublicInstance)
					.Where(_ => !_.IsPrivate))
				{
					if (constructor.CanBeSeenByMockAssembly(this.NameGenerator))
					{
						var parameters = constructor.GetParameters(this.Namespaces);

						if (!string.IsNullOrWhiteSpace(parameters))
						{
							parameters = $", {parameters}";
						}

						generatedConstructors.Add(CodeTemplates.GetConstructorTemplate(
							constructorName, constructor.GetArgumentNameList(), parameters));
						this.IsUnsafe |= constructor.IsUnsafeToMock();
					}
				}
			}

			return generatedConstructors;
		}

		private List<string> GetGeneratedMethods()
		{
			var generatedMethods = new List<string>();

			foreach (var method in this.BaseType.GetMockableMethods(this.NameGenerator))
			{
				var methodInformation = this.GetMethodInformation(method);
				var baseMethod = method.Value;
				var argumentNameList = baseMethod.GetArgumentNameList();
				var outInitializers = !methodInformation.ContainsRefAndOrOutParameters ? string.Empty : baseMethod.GetOutInitializers();

				if (baseMethod.IsPublic)
				{
					var visibility = method.RequiresExplicitInterfaceImplementation ? string.Empty : CodeTemplates.Public;

					// Either the base method contains no refs/outs, or the user specified a delegate
					// to use to handle that method (remember, types with methods with refs/outs are gen'd
					// each time, and that's the only reason the handlers are passed in).
					if (!methodInformation.ContainsRefAndOrOutParameters || !string.IsNullOrWhiteSpace(methodInformation.DelegateCast))
					{
						if (!methodInformation.ContainsRefAndOrOutParameters && baseMethod.GetParameters().Length > 0)
						{
							generatedMethods.Add(this.GenerateMethodWithNoRefOutParameters(
								baseMethod, methodInformation.DelegateCast, argumentNameList, outInitializers, methodInformation.DescriptionWithOverride,
								visibility));
						}
						else
						{
							generatedMethods.Add(this.GenerateMethodWithRefOutOrNoParameters(
								baseMethod, methodInformation.DelegateCast, argumentNameList, outInitializers, methodInformation.DescriptionWithOverride,
								visibility));

							if (methodInformation.ContainsRefAndOrOutParameters)
							{
								this.HandleRefOutMethod(baseMethod, methodInformation);
							}
						}
					}
					else
					{
						generatedMethods.Add(CodeTemplates.GetRefOutNotImplementedMethodTemplate(methodInformation.DescriptionWithOverride));
					}

					this.RequiresObsoleteSuppression |= baseMethod.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
				else if (!baseMethod.IsPrivate && baseMethod.IsAbstract)
				{
					var visibility = CodeTemplates.GetVisibility(baseMethod.IsFamily, baseMethod.IsFamilyOrAssembly);

					generatedMethods.Add(baseMethod.ReturnType != typeof(void) ?
						CodeTemplates.GetNonPublicFunctionImplementationTemplate(visibility, methodInformation.Description,
							outInitializers, $"{baseMethod.ReturnType.GetSafeName()}{baseMethod.ReturnType.GetGenericArguments(this.Namespaces).Arguments}") :
						CodeTemplates.GetNonPublicActionImplementationTemplate(visibility, methodInformation.Description,
							outInitializers));

					this.RequiresObsoleteSuppression |= baseMethod.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return generatedMethods;
		}

		protected class MethodInformation
		{
			public bool ContainsRefAndOrOutParameters { get; set; }
			public string DelegateCast { get; set; }
			public string Description { get; set; }
			public string DescriptionWithOverride { get; set; }
		}

		protected abstract MethodInformation GetMethodInformation(MockableResult<MethodInfo> baseMethod);

		private string GenerateMethodWithNoRefOutParameters(MethodInfo baseMethod, string delegateCast, string argumentNameList, string outInitializers, string methodDescriptionWithOverride,
			string visibility)
		{
			var expectationChecks = baseMethod.GetExpectationChecks();
			var expectationExceptionMessage = baseMethod.GetExpectationExceptionMessage();

			if (baseMethod.ReturnType != typeof(void))
			{
				return baseMethod.ReturnType.IsValueType ||
					(baseMethod.ReturnType.IsGenericParameter && (baseMethod.ReturnType.GenericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) == 0) ?
						CodeTemplates.GetFunctionWithValueTypeReturnValueMethodTemplate(
							baseMethod.MetadataToken, argumentNameList, $"{baseMethod.ReturnType.GetSafeName()}{baseMethod.ReturnType.GetGenericArguments(this.Namespaces).Arguments}",
							expectationChecks, delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride,
							visibility) :
						CodeTemplates.GetFunctionWithReferenceTypeReturnValueMethodTemplate(
							baseMethod.MetadataToken, argumentNameList, $"{baseMethod.ReturnType.GetSafeName()}{baseMethod.ReturnType.GetGenericArguments(this.Namespaces).Arguments}",
							expectationChecks, delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride,
							visibility);
			}
			else
			{
				return CodeTemplates.GetActionMethodTemplate(
					baseMethod.MetadataToken, argumentNameList, expectationChecks, delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride,
					visibility);
			}
		}

		protected virtual void HandleRefOutMethod(MethodInfo baseMethod, MethodInformation methodDescription) { }

		private string GenerateMethodWithRefOutOrNoParameters(MethodInfo baseMethod, string delegateCast, string argumentNameList, string outInitializers, string methodDescriptionWithOverride,
			string visibility)
		{
			if (baseMethod.ReturnType != typeof(void))
			{
				return baseMethod.ReturnType.IsValueType ||
					(baseMethod.ReturnType.IsGenericParameter && (baseMethod.ReturnType.GenericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) == 0) ?
						CodeTemplates.GetFunctionWithValueTypeReturnValueAndNoArgumentsMethodTemplate(
							baseMethod.MetadataToken, argumentNameList, $"{baseMethod.ReturnType.GetSafeName()}{baseMethod.ReturnType.GetGenericArguments(this.Namespaces).Arguments}",
							delegateCast, outInitializers, methodDescriptionWithOverride, visibility) :
						CodeTemplates.GetFunctionWithReferenceTypeReturnValueAndNoArgumentsMethodTemplate(
							baseMethod.MetadataToken, argumentNameList, $"{baseMethod.ReturnType.GetSafeName()}{baseMethod.ReturnType.GetGenericArguments(this.Namespaces).Arguments}",
							delegateCast, outInitializers, methodDescriptionWithOverride, visibility);
			}
			else
			{
				return CodeTemplates.GetActionMethodWithNoArgumentsTemplate(
					baseMethod.MetadataToken, argumentNameList, delegateCast, outInitializers, methodDescriptionWithOverride,
					visibility);
			}
		}

		private List<string> GetGeneratedEvents()
		{
			var generatedEvents = new List<string>();

			foreach (var @event in this.BaseType.GetMockableEvents(this.NameGenerator))
			{
				var eventHandlerType = @event.EventHandlerType;
				this.Namespaces.Add(eventHandlerType.Namespace);
				var eventMethod = @event.AddMethod;
				var methodInformation = this.GetMethodInformation(new MockableResult<MethodInfo>(eventMethod, false));
				var @override = methodInformation.DescriptionWithOverride.Contains("override") ? "override " : string.Empty;

				if (eventMethod.IsPublic)
				{
					if (eventHandlerType.IsGenericType)
					{
						var eventGenericType = eventHandlerType.GetGenericArguments()[0];
						generatedEvents.Add(CodeTemplates.GetEventTemplate(@override,
							$"EventHandler<{eventGenericType.GetSafeName()}>", @event.Name));
						this.Namespaces.Add(eventGenericType.Namespace);
					}
					else
					{
						generatedEvents.Add(CodeTemplates.GetEventTemplate(@override,
							eventHandlerType.GetSafeName(), @event.Name));
					}

					this.RequiresObsoleteSuppression |= @event.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
				else if (!eventMethod.IsPrivate && eventMethod.IsAbstract)
				{
					var visibility = CodeTemplates.GetVisibility(eventMethod.IsFamily, eventMethod.IsFamilyOrAssembly);

					if (eventHandlerType.IsGenericType)
					{
						var eventGenericType = eventHandlerType.GetGenericArguments()[0];
						generatedEvents.Add(CodeTemplates.GetNonPublicEventTemplate(visibility,
							$"EventHandler<{eventGenericType.GetSafeName()}>", @event.Name));
						this.Namespaces.Add(eventGenericType.Namespace);
					}
					else
					{
						generatedEvents.Add(CodeTemplates.GetNonPublicEventTemplate(visibility,
							eventHandlerType.GetSafeName(), @event.Name));
					}

					this.RequiresObsoleteSuppression |= @event.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return generatedEvents;
		}

		private List<string> GetGeneratedProperties()
		{
			var generatedProperties = new List<string>();

			foreach (var property in this.BaseType.GetMockableProperties(this.NameGenerator))
			{
				var baseProperty = property.Value;

				this.Namespaces.Add(baseProperty.PropertyType.Namespace);
				var indexers = baseProperty.GetIndexParameters();
				var propertyMethod = baseProperty.GetDefaultMethod();
				var methodInformation = this.GetMethodInformation(new MockableResult<MethodInfo>(propertyMethod, false));
				var @override = methodInformation.DescriptionWithOverride.Contains("override") ? "override " : string.Empty;

				if (propertyMethod.IsPublic)
				{
					var propertyImplementations = new List<string>();

					if (property.Accessors == PropertyAccessors.Get || property.Accessors == PropertyAccessors.GetAndSet)
					{
						var getMethod = baseProperty.GetMethod;
						var getVisibility = getMethod.IsPublic ? string.Empty : CodeTemplates.GetVisibility(getMethod.IsFamily, getMethod.IsFamilyOrAssembly);
						var getArgumentNameList = getMethod.GetArgumentNameList();
						var getDelegateCast = getMethod.GetDelegateCast();

						if (getMethod.GetParameters().Length > 0)
						{
							var getExpectationChecks = getMethod.GetExpectationChecks();
							var getExpectationExceptionMessage = getMethod.GetExpectationExceptionMessage();
							propertyImplementations.Add(getMethod.ReturnType.IsValueType ?
								CodeTemplates.GetPropertyGetWithValueTypeReturnValueTemplate(
									getMethod.MetadataToken, getArgumentNameList, $"{getMethod.ReturnType.GetSafeName()}{getMethod.ReturnType.GetGenericArguments(this.Namespaces).Arguments}",
									getExpectationChecks, getDelegateCast, getExpectationExceptionMessage, getVisibility) :
								CodeTemplates.GetPropertyGetWithReferenceTypeReturnValueTemplate(
									getMethod.MetadataToken, getArgumentNameList, $"{getMethod.ReturnType.GetSafeName()}{getMethod.ReturnType.GetGenericArguments(this.Namespaces).Arguments}",
									getExpectationChecks, getDelegateCast, getExpectationExceptionMessage, getVisibility));
						}
						else
						{
							propertyImplementations.Add(getMethod.ReturnType.IsValueType ?
								CodeTemplates.GetPropertyGetWithValueTypeReturnValueAndNoIndexersTemplate(
									getMethod.MetadataToken, getArgumentNameList,
									$"{getMethod.ReturnType.GetSafeName()}{getMethod.ReturnType.GetGenericArguments(this.Namespaces).Arguments}", getDelegateCast, getVisibility) :
								CodeTemplates.GetPropertyGetWithReferenceTypeReturnValueAndNoIndexersTemplate(
									getMethod.MetadataToken, getArgumentNameList,
									$"{getMethod.ReturnType.GetSafeName()}{getMethod.ReturnType.GetGenericArguments(this.Namespaces).Arguments}", getDelegateCast, getVisibility));
						}
					}

					if (property.Accessors == PropertyAccessors.Set || property.Accessors == PropertyAccessors.GetAndSet)
					{
						var setMethod = baseProperty.SetMethod;
						var setVisibility = setMethod.IsPublic ? string.Empty : CodeTemplates.GetVisibility(setMethod.IsFamily, setMethod.IsFamilyOrAssembly);
						var setArgumentNameList = setMethod.GetArgumentNameList();
						var setDelegateCast = setMethod.GetDelegateCast();

						if (setMethod.GetParameters().Length > 0)
						{
							var setExpectationChecks = setMethod.GetExpectationChecks();
							var setExpectationExceptionMessage = setMethod.GetExpectationExceptionMessage();
							propertyImplementations.Add(CodeTemplates.GetPropertySetTemplate(
								setMethod.MetadataToken, setArgumentNameList, setExpectationChecks, setDelegateCast, setExpectationExceptionMessage, setVisibility));
						}
						else
						{
							propertyImplementations.Add(CodeTemplates.GetPropertySetAndNoIndexersTemplate(
								setMethod.MetadataToken, setArgumentNameList, setDelegateCast, setVisibility));
						}
					}

					if (indexers.Length > 0)
					{
						var parameters = string.Join(", ",
							from indexer in indexers
							let _ = this.Namespaces.Add(indexer.ParameterType.Namespace)
							select $"{indexer.ParameterType.Name} {indexer.Name}");

						// Indexer
						generatedProperties.Add(CodeTemplates.GetPropertyIndexerTemplate(
							$"{@override}{baseProperty.PropertyType.GetSafeName()}{baseProperty.PropertyType.GetGenericArguments(this.Namespaces).Arguments}", parameters,
							string.Join(Environment.NewLine, propertyImplementations)));
					}
					else
					{
						// Normal
						generatedProperties.Add(CodeTemplates.GetPropertyTemplate(
							$"{@override}{baseProperty.PropertyType.GetSafeName()}{baseProperty.PropertyType.GetGenericArguments(this.Namespaces).Arguments}", baseProperty.Name,
							string.Join(Environment.NewLine, propertyImplementations)));
					}

					this.RequiresObsoleteSuppression |= baseProperty.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
				else if (!propertyMethod.IsPrivate && propertyMethod.IsAbstract)
				{
					var propertyImplementations = new List<string>();
					var visibility = CodeTemplates.GetVisibility(propertyMethod.IsFamily, propertyMethod.IsFamilyOrAssembly);

					if (property.Accessors == PropertyAccessors.Get || property.Accessors == PropertyAccessors.GetAndSet)
					{
						var getVisibility = CodeTemplates.GetVisibility(baseProperty.GetMethod.IsFamily, baseProperty.GetMethod.IsFamilyOrAssembly);

						if (getVisibility == visibility)
						{
							getVisibility = string.Empty;
						}

						propertyImplementations.Add(CodeTemplates.GetNonPublicPropertyGetTemplate(getVisibility));
					}

					if (property.Accessors == PropertyAccessors.Set || property.Accessors == PropertyAccessors.GetAndSet)
					{
						var setVisibility = CodeTemplates.GetVisibility(baseProperty.SetMethod.IsFamily, baseProperty.SetMethod.IsFamilyOrAssembly);

						if (setVisibility == visibility)
						{
							setVisibility = string.Empty;
						}

						propertyImplementations.Add(CodeTemplates.GetNonPublicPropertySetTemplate(setVisibility));
					}

					if (indexers.Length > 0)
					{
						var parameters = string.Join(", ",
							from indexer in indexers
							let _ = this.Namespaces.Add(indexer.ParameterType.Namespace)
							select $"{indexer.ParameterType.Name} {indexer.Name}");

						// Indexer
						generatedProperties.Add(CodeTemplates.GetNonPublicPropertyIndexerTemplate(visibility,
							$"{baseProperty.PropertyType.GetSafeName()}{baseProperty.PropertyType.GetGenericArguments(this.Namespaces).Arguments}", parameters,
							string.Join(Environment.NewLine, propertyImplementations)));
					}
					else
					{
						// Normal
						generatedProperties.Add(CodeTemplates.GetNonPublicPropertyTemplate(visibility,
							$"{baseProperty.PropertyType.GetSafeName()}{baseProperty.PropertyType.GetGenericArguments(this.Namespaces).Arguments}", baseProperty.Name,
							string.Join(Environment.NewLine, propertyImplementations)));
					}

					this.RequiresObsoleteSuppression |= baseProperty.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return generatedProperties;
		}

		protected string GetTypeNameWithNoGenerics() => this.TypeName.Split('<').First();

		protected string GetTypeNameWithGenericsAndNoTextFormatting() => $"{this.TypeName.Replace("<", string.Empty).Replace(">", string.Empty).Replace(", ", string.Empty)}";

		private string MakeCode()
		{
			this.RequiresObsoleteSuppression |= this.BaseType.GetCustomAttribute<ObsoleteAttribute>() != null;
			var methods = this.GetGeneratedMethods();
			var constructors = this.GetGeneratedConstructors();
			var properties = this.GetGeneratedProperties();
			var events = this.GetGeneratedEvents();

			this.Namespaces.Add(this.BaseType.Namespace);
			this.Namespaces.Add(typeof(ExpectationException).Namespace);
			this.Namespaces.Add(typeof(IMock).Namespace);
			this.Namespaces.Add(typeof(HandlerInformation).Namespace);
			this.Namespaces.Add(typeof(string).Namespace);
			this.Namespaces.Add(typeof(ReadOnlyDictionary<,>).Namespace);
			this.Namespaces.Add(typeof(BindingFlags).Namespace);

			var baseTypeGenericArguments = this.BaseType.GetGenericArguments(this.Namespaces);

			var @class = CodeTemplates.GetClassTemplate(
				string.Join(Environment.NewLine,
					(from @namespace in this.Namespaces
					 select $"using {@namespace};")),
				this.TypeName, $"{this.BaseType.GetSafeName()}{baseTypeGenericArguments.Arguments}",
				string.Join(Environment.NewLine, methods),
				string.Join(Environment.NewLine, properties),
				string.Join(Environment.NewLine, events),
				string.Join(Environment.NewLine, constructors),
				this.BaseType.Namespace,
				this.Options.Serialization == SerializationOptions.Supported ?
					"[Serializable]" : string.Empty,
				this.Options.Serialization == SerializationOptions.Supported ?
					CodeTemplates.GetConstructorNoArgumentsTemplate(this.GetTypeNameWithNoGenerics()) : string.Empty,
				this.GetAdditionNamespaceCode(),
				this.IsUnsafe, baseTypeGenericArguments.Constraints);

			if (this.RequiresObsoleteSuppression)
			{
				@class =
$@"#pragma warning disable CS0618
#pragma warning disable CS0672
{@class}
#pragma warning restore CS0672
#pragma warning restore CS0618";
			}

			return @class;
		}

		private SyntaxTree MakeTree()
		{
			var @class = this.MakeCode();
			SyntaxTree tree = null;

			if (this.Options.CodeFile == CodeFileOptions.Create)
			{
				Directory.CreateDirectory(this.GetDirectoryForFile());
				var fileName = Path.Combine(this.GetDirectoryForFile(),
					$"{this.GetTypeNameWithGenericsAndNoTextFormatting()}.cs");
				tree = SyntaxFactory.SyntaxTree(
					SyntaxFactory.ParseSyntaxTree(@class)
						.GetCompilationUnitRoot().NormalizeWhitespace(),
					path: fileName, encoding: new UTF8Encoding(false, true));
				File.WriteAllText(fileName, tree.GetText().ToString());
			}
			else
			{
				tree = SyntaxFactory.ParseSyntaxTree(@class);
			}

			return tree;
		}

		protected abstract string GetDirectoryForFile();
		protected virtual string GetAdditionNamespaceCode() => string.Empty;

		internal Options Options { get; }
		internal SyntaxTree Tree { get; private set; }
		internal Type BaseType { get; }
		internal bool IsUnsafe { get; private set; }
		internal ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> Handlers { get; }
		internal SortedSet<string> Namespaces { get; }
		internal string TypeName { get; set; }
		private bool RequiresObsoleteSuppression { get; set; }
		protected NameGenerator NameGenerator { get; private set; }
	}
}
