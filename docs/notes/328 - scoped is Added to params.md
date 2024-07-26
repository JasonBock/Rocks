Current error count is 8465. Hopefully it's reduced once the fix is in.

The random exception I'm getting from CodeGenerationTests:

```
Getting target types for MassTransit.Abstractions
Testing MassTransit.Abstractions - Create
Unhandled exception. System.NullReferenceException: Object reference not set to an instance of an object.
   at Microsoft.CodeAnalysis.CSharp.Binder.BindConstructorInitializerCoreContinued(Boolean found, ArgumentListSyntax initializerArgumentListOpt, MethodSymbol constructor, AnalyzedArguments analyzedArguments, TypeSymbol constructorReturnType, NamedTypeSymbol initializerType, Boolean isBaseConstructorInitializer, CSharpSyntaxNode nonNullSyntax, Location errorLocation, Boolean enableCallerInfo, MemberResolutionResult`1 memberResolutionResult, ImmutableArray`1 candidateConstructors, CompoundUseSiteInfo`1& overloadResolutionUseSiteInfo, BindingDiagnosticBag diagnostics)
   at Microsoft.CodeAnalysis.CSharp.Binder.BindConstructorInitializerCore(ArgumentListSyntax initializerArgumentListOpt, MethodSymbol constructor, BindingDiagnosticBag diagnostics)
   at Microsoft.CodeAnalysis.CSharp.Binder.BindImplicitConstructorInitializer(MethodSymbol constructor, BindingDiagnosticBag diagnostics, CSharpCompilation compilation)
   at Microsoft.CodeAnalysis.CSharp.MethodCompiler.BindMethodBody(MethodSymbol method, TypeCompilationState compilationState, BindingDiagnosticBag diagnostics, Boolean includeInitializersInBody, BoundNode initializersBody, Boolean reportNullableDiagnostics, ImportChain& importChain, Boolean& originalBodyNested, Boolean& prependedDefaultValueTypeConstructorInitializer, InitialState& forSemanticModel)
   at Microsoft.CodeAnalysis.CSharp.MethodCompiler.CompileMethod(MethodSymbol methodSymbol, Int32 methodOrdinal, ProcessedFieldInitializers& processedInitializers, SynthesizedSubmissionFields previousSubmissionFields, TypeCompilationState compilationState)
   at Microsoft.CodeAnalysis.CSharp.MethodCompiler.CompileNamedType(NamedTypeSymbol containingType)
   at Microsoft.CodeAnalysis.CSharp.MethodCompiler.<>c__DisplayClass25_0.<CompileNamedTypeAsync>b__0()
   at System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(Thread threadPoolThread, ExecutionContext executionContext, ContextCallback callback, Object state)
--- End of stack trace from previous location ---
   at System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(Thread threadPoolThread, ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Threading.Tasks.Task.ExecuteWithThreadLocal(Task& currentTaskSlot, Thread threadPoolThread)
--- End of stack trace from previous location ---
   at Microsoft.CodeAnalysis.Compilation.Emit(Stream peStream, Stream metadataPEStream, Stream pdbStream, Stream xmlDocumentationStream, Stream win32Resources, IEnumerable`1 manifestResources, EmitOptions options, IMethodSymbol debugEntryPoint, Stream sourceLinkStream, IEnumerable`1 embeddedTexts, RebuildData rebuildData, CompilationTestData testData, CancellationToken cancellationToken)
   at Microsoft.CodeAnalysis.Compilation.Emit(Stream peStream, Stream pdbStream, Stream xmlDocumentationStream, Stream win32Resources, IEnumerable`1 manifestResources, EmitOptions options, IMethodSymbol debugEntryPoint, Stream sourceLinkStream, IEnumerable`1 embeddedTexts, Stream metadataPEStream, RebuildData rebuildData, CancellationToken cancellationToken)
   at Microsoft.CodeAnalysis.Compilation.Emit(Stream peStream, Stream pdbStream, Stream xmlDocumentationStream, Stream win32Resources, IEnumerable`1 manifestResources, EmitOptions options, IMethodSymbol debugEntryPoint, Stream sourceLinkStream, IEnumerable`1 embeddedTexts, Stream metadataPEStream, CancellationToken cancellationToken)
   at Rocks.CodeGenerationTest.TestGenerator.Generate(IIncrementalGenerator generator, Type[] targetTypes, Type[] typesToLoadAssembliesFrom, String[] aliases, BuildType buildType) in C:\Users\jason\source\repos\JasonBock\Rocks\src\Rocks.CodeGenerationTest\TestGenerator.cs:line 201
   at Program.<<Main>$>g__TestWithTypes|0_4() in C:\Users\jason\source\repos\JasonBock\Rocks\src\Rocks.CodeGenerationTest\Program.cs:line 255
   at Program.<Main>$(String[] args) in C:\Users\jason\source\repos\JasonBock\Rocks\src\Rocks.CodeGenerationTest\Program.cs:line 18
```

Another one:

```
Getting target types for Avalonia.Controls
Testing Avalonia.Controls - Create
Fatal error. System.AccessViolationException: Attempted to read or write protected memory. This is often an indication that other memory is corrupt.
   at Microsoft.CodeAnalysis.CSharp.Binder.BindConstructorInitializerCoreContinued(Boolean, Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax, Microsoft.CodeAnalysis.CSharp.Symbols.MethodSymbol, Microsoft.CodeAnalysis.CSharp.AnalyzedArguments, Microsoft.CodeAnalysis.CSharp.Symbols.TypeSymbol, Microsoft.CodeAnalysis.CSharp.Symbols.NamedTypeSymbol, Boolean, Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, Microsoft.CodeAnalysis.Location, Boolean, Microsoft.CodeAnalysis.CSharp.MemberResolutionResult`1<Microsoft.CodeAnalysis.CSharp.Symbols.MethodSymbol>, System.Collections.Immutable.ImmutableArray`1<Microsoft.CodeAnalysis.CSharp.Symbols.MethodSymbol>, Microsoft.CodeAnalysis.CompoundUseSiteInfo`1<Microsoft.CodeAnalysis.CSharp.Symbols.AssemblySymbol> ByRef, Microsoft.CodeAnalysis.CSharp.BindingDiagnosticBag)
   at Microsoft.CodeAnalysis.CSharp.Binder.BindConstructorInitializerCore(Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax, Microsoft.CodeAnalysis.CSharp.Symbols.MethodSymbol, Microsoft.CodeAnalysis.CSharp.BindingDiagnosticBag)
   at Microsoft.CodeAnalysis.CSharp.Binder.BindConstructorBody(Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax, Microsoft.CodeAnalysis.CSharp.BindingDiagnosticBag)
   at Microsoft.CodeAnalysis.CSharp.Binder.BindMethodBody(Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, Microsoft.CodeAnalysis.CSharp.BindingDiagnosticBag)
   at Microsoft.CodeAnalysis.CSharp.Binder.BindWithLambdaBindingCountDiagnostics[[System.__Canon, System.Private.CoreLib, Version=9.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.__Canon, System.Private.CoreLib, Version=9.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.__Canon, System.Private.CoreLib, Version=9.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]](System.__Canon, System.__Canon, Microsoft.CodeAnalysis.CSharp.BindingDiagnosticBag, System.Func`5<Microsoft.CodeAnalysis.CSharp.Binder,System.__Canon,System.__Canon,Microsoft.CodeAnalysis.CSharp.BindingDiagnosticBag,System.__Canon>)
   at Microsoft.CodeAnalysis.CSharp.MethodCompiler.BindMethodBody(Microsoft.CodeAnalysis.CSharp.Symbols.MethodSymbol, Microsoft.CodeAnalysis.CSharp.TypeCompilationState, Microsoft.CodeAnalysis.CSharp.BindingDiagnosticBag, Boolean, Microsoft.CodeAnalysis.CSharp.BoundNode, Boolean, Microsoft.CodeAnalysis.CSharp.ImportChain ByRef, Boolean ByRef, Boolean ByRef, InitialState ByRef)
   at Microsoft.CodeAnalysis.CSharp.MethodCompiler.CompileMethod(Microsoft.CodeAnalysis.CSharp.Symbols.MethodSymbol, Int32, ProcessedFieldInitializers ByRef, Microsoft.CodeAnalysis.CSharp.SynthesizedSubmissionFields, Microsoft.CodeAnalysis.CSharp.TypeCompilationState)
   at Microsoft.CodeAnalysis.CSharp.MethodCompiler.CompileNamedType(Microsoft.CodeAnalysis.CSharp.Symbols.NamedTypeSymbol)
   at Microsoft.CodeAnalysis.CSharp.MethodCompiler+<>c__DisplayClass25_0.<CompileNamedTypeAsync>b__0()
   at System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(System.Threading.Thread, System.Threading.ExecutionContext, System.Threading.ContextCallback, System.Object)
   at System.Threading.Tasks.Task.ExecuteWithThreadLocal(System.Threading.Tasks.Task ByRef, System.Threading.Thread)
   at System.Threading.ThreadPoolWorkQueue.Dispatch()
   at System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
```