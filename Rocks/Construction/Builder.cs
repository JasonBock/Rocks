using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Construction.Generators;
using Rocks.Extensions;
using Rocks.Options;
using Rocks.Templates;
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
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction
{
	internal abstract class Builder<TInformationBuilder>
		where TInformationBuilder : MethodInformationBuilder
	{
		internal Builder(Type baseType,
			ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, RockOptions options, NameGenerator generator,
			TInformationBuilder informationBuilder, TypeNameGenerator typeNameGenerator,
			bool isMake)
		{
			this.BaseType = baseType;
			this.IsUnsafe = this.BaseType.IsUnsafeToMock();
			this.Handlers = handlers;
			this.Namespaces = namespaces;
			this.Options = options;
			this.NameGenerator = generator;
			this.InformationBuilder = informationBuilder;
			this.TypeName = typeNameGenerator.Generate(baseType);
			this.IsMake = isMake;
		}

		internal virtual void Build() => this.Tree = this.MakeTree();

		private GenerateResults GetGeneratedEvents() =>
			new EventsGenerator().Generate(this.BaseType, this.Namespaces,
				this.NameGenerator, this.InformationBuilder);

		private GenerateResults GetGeneratedConstructors() =>
			new ConstructorsGenerator().Generate(this.BaseType,
				this.Namespaces, this.NameGenerator, this.GetTypeNameWithNoGenerics());

		private GenerateResults GetGeneratedMethods(bool hasEvents) =>
			new MethodsGenerator().Generate(this.BaseType,
				this.Namespaces, this.NameGenerator, this.InformationBuilder,
				this.IsMake, this.HandleRefOutMethod, hasEvents);

		private GenerateResults GetGeneratedProperties(bool hasEvents) =>
			new PropertiesGenerator().Generate(this.BaseType,
				this.Namespaces, this.NameGenerator, this.InformationBuilder,
				this.IsMake, hasEvents);

		protected virtual void HandleRefOutMethod(MethodInfo baseMethod, MethodInformation methodDescription) { }

		protected string GetTypeNameWithNoGenerics() => this.TypeName.Split('<').First();

		protected string GetTypeNameWithGenericsAndNoTextFormatting() => $"{this.TypeName.Replace("<", string.Empty).Replace(">", string.Empty).Replace(", ", string.Empty)}";

		private string MakeCode()
		{
			var hasEvents = this.BaseType.HasEvents(this.NameGenerator);
			var methods = this.GetGeneratedMethods(hasEvents);
			var constructors = this.GetGeneratedConstructors();
			var properties = this.GetGeneratedProperties(hasEvents);
			var events = this.GetGeneratedEvents();

			this.IsUnsafe |= constructors.IsUnsafe || events.IsUnsafe || methods.IsUnsafe || properties.IsUnsafe;
			this.RequiresObsoleteSuppression |= this.BaseType.GetTypeInfo().GetCustomAttribute<ObsoleteAttribute>() != null ||
				constructors.RequiresObsoleteSuppression || events.RequiresObsoleteSuppression ||
				methods.RequiresObsoleteSuppression || properties.RequiresObsoleteSuppression;

			this.Namespaces.Remove(this.BaseType.Namespace);

			var baseTypeGenericArguments = this.BaseType.GetGenericArguments(this.Namespaces);

#if NETCOREAPP1_1
			// The template automatically adds this namespace
			// so remove it.
			this.Namespaces.Remove("System.Reflection");
#endif

			var namespaces = string.Join(Environment.NewLine,
				(from @namespace in this.Namespaces
				 select $"using {@namespace};"));

#if !NETCOREAPP1_1
			var @class = ClassTemplates.GetClass(namespaces,
				this.TypeName, this.BaseType.GetFullName(),
				methods.Result, properties.Result, events.Result, constructors.Result,
				this.BaseType.Namespace,
				this.Options.Serialization == SerializationOptions.Supported ?
					"[Serializable]" : string.Empty,
				this.Options.Serialization == SerializationOptions.Supported ?
					ConstructorTemplates.GetConstructorWithNoArguments(this.GetTypeNameWithNoGenerics()) : string.Empty,
				this.GetAdditionNamespaceCode(),
				this.IsUnsafe, baseTypeGenericArguments.Constraints,
				hasEvents ? "R.IMockWithEvents" : "R.IMock",
				hasEvents ? ClassTemplates.GetRaiseImplementation() : string.Empty);
#else
			var @class = ClassTemplates.GetClass(namespaces,
				this.TypeName, this.BaseType.GetFullName(),
				methods.Result, properties.Result, events.Result, constructors.Result,
				this.BaseType.Namespace,
				string.Empty,
				string.Empty,
				this.GetAdditionNamespaceCode(),
				this.IsUnsafe, baseTypeGenericArguments.Constraints,
				hasEvents ? "R.IMockWithEvents" : "R.IMock",
				hasEvents ? ClassTemplates.GetRaiseImplementation() : string.Empty);
#endif
			if (this.RequiresObsoleteSuppression)
			{
				@class = ClassTemplates.GetClassWithObsoleteSuppression(@class);
			}

			return @class;
		}

		private SyntaxTree MakeTree()
		{
			var @class = this.MakeCode();
			SyntaxTree tree = null;

			if (this.Options.CodeFile == CodeFileOptions.Create)
			{
				var fileDirectory = this.GetDirectoryForFile();
				Directory.CreateDirectory(fileDirectory);
				var fileName = Path.Combine(fileDirectory,
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

		internal RockOptions Options { get; }
		internal SyntaxTree Tree { get; private set; }
		internal Type BaseType { get; }
		internal bool IsUnsafe { get; private set; }
		internal ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> Handlers { get; }
		internal SortedSet<string> Namespaces { get; }
		internal string TypeName { get; set; }
		private bool RequiresObsoleteSuppression { get; set; }
		protected NameGenerator NameGenerator { get; private set; }
		internal TInformationBuilder InformationBuilder { get; private set; }
		internal bool IsMake { get; }
	}
}
