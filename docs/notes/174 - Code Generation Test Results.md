So, after creating the console app and adding some other packages, I have some work to do.

* Core .NET Types
  * `Create`
    * Error - Id: CS1069, Description: Rocks\Rocks.RockCreateGenerator\UTF7Encoding_Rock_Create.g.cs(193,21): error CS1069: The type name 'InvalidEnumArgumentException' could not be found in the namespace 'System.ComponentModel'. This type has been forwarded to assembly 'System.ComponentModel.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' Consider adding a reference to that assembly.
  * `Make`
    * Warning - Id: CS8619, Description: Rocks\Rocks.RockMakeGenerator\HttpContent_Rock_Make.g.cs(54,12): warning CS8619: Nullability of reference types in value of type 'Task<Stream?>' doesn't match target type 'Task<Stream>'.
    * Warning - Id: CS8765, Description: Rocks\Rocks.RockMakeGenerator\StreamWriter_Rock_Make.g.cs(297,5): warning CS8765: Nullability of type of parameter 'value' doesn't match overridden member (possibly because of nullability attributes).
* ComputeSharp.D2D1
  * `Create` and `Make`
    * Error - Id: CS0315, Description: Rocks\Rocks.RockCreateGenerator\ID2D1TransformMapperOfint_Rock_Create.g.cs(213,101): error CS0315: The type 'int' cannot be used as type parameter 'T' in the generic type or method 'ID2D1TransformMapper<T>'. There is no boxing conversion from 'int' to 'ComputeSharp.D2D1.ID2D1PixelShader'.
    * Error - Id: CS0315, Description: (10,70): error CS0315: The type 'int' cannot be used as type parameter 'T' in the generic type or method 'ID2D1TransformMapperFactory<T>'. There is no boxing conversion from 'int' to 'ComputeSharp.D2D1.ID2D1PixelShader'.
    * Error - Id: CS0449, Description: Rocks\Rocks.RockCreateGenerator\ID2D1PixelShader_Rock_Create.g.cs(190,51): error CS0449: The 'class', 'struct', 'unmanaged', 'notnull', and 'default' constraints cannot be combined or duplicated, and must be specified first in the constraints list.
* CSLA
  * `Create` and `Make`
    * Error - Id: CS0311, Description: (82,57): error CS0311: The type 'object' cannot be used as type parameter 'T' in the generic type or method 'MinValue<T>'. There is no implicit reference conversion from 'object' to 'System.IComparable'.


Note that Moq and ImageSharp have runtime errors:

Unhandled exception. System.AggregateException: One or more errors occurred. (Sequence contains more than one element) (Sequence contains more than one element) (Sequence contains more than one element) (Sequence contains more than one element)
 ---> System.InvalidOperationException: Sequence contains more than one element
   at System.Linq.ThrowHelper.ThrowMoreThanOneElementException()
   at System.Linq.Enumerable.TryGetSingle[TSource](IEnumerable`1 source, Boolean& found)
   at System.Linq.Enumerable.Single[TSource](IEnumerable`1 source)
   at System.Linq.ImmutableArrayExtensions.SingleOrDefault[T](ImmutableArray`1 immutableArray, Func`2 predicate)
   at Rocks.Extensions.IAssemblySymbolExtensions.ExposesInternalsTo(IAssemblySymbol self, IAssemblySymbol other) in /_/src/Rocks/Extensions/IAssemblySymbolExtensions.cs:line 11
   at Rocks.Extensions.ISymbolExtensions.CanBeSeenByContainingAssembly(ISymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol) in /_/src/Rocks/Extensions/ISymbolExtensions.cs:line 22
   at Rocks.Extensions.ITypeSymbolExtensions.<>c__DisplayClass10_0.<GetMockableProperties>b__5(IPropertySymbol _) in /_/src/Rocks/Extensions/ITypeSymbolExtensions.cs:line 531
   at System.Linq.Enumerable.WhereEnumerableIterator`1.MoveNext()
   at Rocks.Extensions.ITypeSymbolExtensions.GetMockableProperties(ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol, HashSet`1 shims, UInt32& memberIdentifier) in /_/src/Rocks/Extensions/ITypeSymbolExtensions.cs:line 530
   at Rocks.MockInformation.Validate(ITypeSymbol typeToMock, BuildType buildType) in /_/src/Rocks/MockInformation.cs:line 74
   at Rocks.MockInformation..ctor(ITypeSymbol typeToMock, IAssemblySymbol containingAssemblyOfInvocationSymbol, SemanticModel model, ConfigurationValues configurationValues, BuildType buildType) in /_/src/Rocks/MockInformation.cs:line 17
   at Rocks.CodeGenerationTest.Extensions.IsValidTarget(Type self) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\Extensions.cs:line 68
   at Rocks.CodeGenerationTest.TestGenerator.<>c__DisplayClass0_0.<Generate>b__1(Type _) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\TestGenerator.cs:line 20
   at System.Threading.Tasks.Parallel.<>c__DisplayClass44_0`2.<PartitionerForEachWorker>b__1(IEnumerator& partitionState, Int32 timeout, Boolean& replicationDelegateYieldedBeforeCompletion)
--- End of stack trace from previous location ---
   at System.Threading.Tasks.Parallel.<>c__DisplayClass44_0`2.<PartitionerForEachWorker>b__1(IEnumerator& partitionState, Int32 timeout, Boolean& replicationDelegateYieldedBeforeCompletion)
   at System.Threading.Tasks.TaskReplicator.Replica`1.ExecuteAction(Boolean& yieldedBeforeCompletion)
   at System.Threading.Tasks.TaskReplicator.Replica.Execute()
   --- End of inner exception stack trace ---
   at System.Threading.Tasks.TaskReplicator.Run[TState](ReplicatableUserAction`1 action, ParallelOptions options, Boolean stopOnFirstFailure)
   at System.Threading.Tasks.Parallel.PartitionerForEachWorker[TSource,TLocal](Partitioner`1 source, ParallelOptions parallelOptions, Action`1 simpleBody, Action`2 bodyWithState, Action`3 bodyWithStateAndIndex, Func`4 bodyWithStateAndLocal, Func`5 bodyWithEverything, Func`1 localInit, Action`1 localFinally)
--- End of stack trace from previous location ---
   at System.Threading.Tasks.Parallel.ThrowSingleCancellationExceptionOrOtherException(ICollection exceptions, CancellationToken cancelToken, Exception otherException)
   at System.Threading.Tasks.Parallel.PartitionerForEachWorker[TSource,TLocal](Partitioner`1 source, ParallelOptions parallelOptions, Action`1 simpleBody, Action`2 bodyWithState, Action`3 bodyWithStateAndIndex, Func`4 bodyWithStateAndLocal, Func`5 bodyWithEverything, Func`1 localInit, Action`1 localFinally)
   at System.Threading.Tasks.Parallel.ForEachWorker[TSource,TLocal](IEnumerable`1 source, ParallelOptions parallelOptions, Action`1 body, Action`2 bodyWithState, Action`3 bodyWithStateAndIndex, Func`4 bodyWithStateAndLocal, Func`5 bodyWithEverything, Func`1 localInit, Action`1 localFinally)
   at System.Threading.Tasks.Parallel.ForEach[TSource](IEnumerable`1 source, Action`1 body)
   at Rocks.CodeGenerationTest.TestGenerator.Generate(IIncrementalGenerator generator, HashSet`1 targetAssemblies) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\TestGenerator.cs:line 17
   at Program.<Main>$(String[] args) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\Program.cs:line 34
 ---> (Inner Exception #1) System.InvalidOperationException: Sequence contains more than one element
   at System.Linq.ThrowHelper.ThrowMoreThanOneElementException()
   at System.Linq.Enumerable.TryGetSingle[TSource](IEnumerable`1 source, Boolean& found)
   at System.Linq.Enumerable.Single[TSource](IEnumerable`1 source)
   at System.Linq.ImmutableArrayExtensions.SingleOrDefault[T](ImmutableArray`1 immutableArray, Func`2 predicate)
   at Rocks.Extensions.IAssemblySymbolExtensions.ExposesInternalsTo(IAssemblySymbol self, IAssemblySymbol other) in /_/src/Rocks/Extensions/IAssemblySymbolExtensions.cs:line 11
   at Rocks.Extensions.ISymbolExtensions.CanBeSeenByContainingAssembly(ISymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol) in /_/src/Rocks/Extensions/ISymbolExtensions.cs:line 22
   at Rocks.Extensions.ITypeSymbolExtensions.<>c__DisplayClass10_0.<GetMockableProperties>b__5(IPropertySymbol _) in /_/src/Rocks/Extensions/ITypeSymbolExtensions.cs:line 531
   at System.Linq.Enumerable.WhereEnumerableIterator`1.MoveNext()
   at Rocks.Extensions.ITypeSymbolExtensions.GetMockableProperties(ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol, HashSet`1 shims, UInt32& memberIdentifier) in /_/src/Rocks/Extensions/ITypeSymbolExtensions.cs:line 530
   at Rocks.MockInformation.Validate(ITypeSymbol typeToMock, BuildType buildType) in /_/src/Rocks/MockInformation.cs:line 74
   at Rocks.MockInformation..ctor(ITypeSymbol typeToMock, IAssemblySymbol containingAssemblyOfInvocationSymbol, SemanticModel model, ConfigurationValues configurationValues, BuildType buildType) in /_/src/Rocks/MockInformation.cs:line 17
   at Rocks.CodeGenerationTest.Extensions.IsValidTarget(Type self) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\Extensions.cs:line 68
   at Rocks.CodeGenerationTest.TestGenerator.<>c__DisplayClass0_0.<Generate>b__1(Type _) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\TestGenerator.cs:line 20
   at System.Threading.Tasks.Parallel.<>c__DisplayClass44_0`2.<PartitionerForEachWorker>b__1(IEnumerator& partitionState, Int32 timeout, Boolean& replicationDelegateYieldedBeforeCompletion)
--- End of stack trace from previous location ---
   at System.Threading.Tasks.Parallel.<>c__DisplayClass44_0`2.<PartitionerForEachWorker>b__1(IEnumerator& partitionState, Int32 timeout, Boolean& replicationDelegateYieldedBeforeCompletion)
   at System.Threading.Tasks.TaskReplicator.Replica`1.ExecuteAction(Boolean& yieldedBeforeCompletion)
   at System.Threading.Tasks.TaskReplicator.Replica.Execute()<---

 ---> (Inner Exception #2) System.InvalidOperationException: Sequence contains more than one element
   at System.Linq.ThrowHelper.ThrowMoreThanOneElementException()
   at System.Linq.Enumerable.TryGetSingle[TSource](IEnumerable`1 source, Boolean& found)
   at System.Linq.Enumerable.Single[TSource](IEnumerable`1 source)
   at System.Linq.ImmutableArrayExtensions.SingleOrDefault[T](ImmutableArray`1 immutableArray, Func`2 predicate)
   at Rocks.Extensions.IAssemblySymbolExtensions.ExposesInternalsTo(IAssemblySymbol self, IAssemblySymbol other) in /_/src/Rocks/Extensions/IAssemblySymbolExtensions.cs:line 11
   at Rocks.Extensions.ISymbolExtensions.CanBeSeenByContainingAssembly(ISymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol) in /_/src/Rocks/Extensions/ISymbolExtensions.cs:line 22
   at Rocks.Extensions.ITypeSymbolExtensions.<>c__DisplayClass9_0.<GetMockableMethods>b__8(IMethodSymbol _) in /_/src/Rocks/Extensions/ITypeSymbolExtensions.cs:line 371
   at System.Linq.Enumerable.WhereEnumerableIterator`1.MoveNext()
   at Rocks.Extensions.ITypeSymbolExtensions.GetMockableMethods(ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol, HashSet`1 shims, Compilation compilation, UInt32& memberIdentifier) in /_/src/Rocks/Extensions/ITypeSymbolExtensions.cs:line 370
   at Rocks.MockInformation.Validate(ITypeSymbol typeToMock, BuildType buildType) in /_/src/Rocks/MockInformation.cs:line 72
   at Rocks.MockInformation..ctor(ITypeSymbol typeToMock, IAssemblySymbol containingAssemblyOfInvocationSymbol, SemanticModel model, ConfigurationValues configurationValues, BuildType buildType) in /_/src/Rocks/MockInformation.cs:line 17
   at Rocks.CodeGenerationTest.Extensions.IsValidTarget(Type self) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\Extensions.cs:line 68
   at Rocks.CodeGenerationTest.TestGenerator.<>c__DisplayClass0_0.<Generate>b__1(Type _) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\TestGenerator.cs:line 20
   at System.Threading.Tasks.Parallel.<>c__DisplayClass44_0`2.<PartitionerForEachWorker>b__1(IEnumerator& partitionState, Int32 timeout, Boolean& replicationDelegateYieldedBeforeCompletion)
--- End of stack trace from previous location ---
   at System.Threading.Tasks.Parallel.<>c__DisplayClass44_0`2.<PartitionerForEachWorker>b__1(IEnumerator& partitionState, Int32 timeout, Boolean& replicationDelegateYieldedBeforeCompletion)
   at System.Threading.Tasks.TaskReplicator.Replica`1.ExecuteAction(Boolean& yieldedBeforeCompletion)
   at System.Threading.Tasks.TaskReplicator.Replica.Execute()<---

 ---> (Inner Exception #3) System.InvalidOperationException: Sequence contains more than one element
   at System.Linq.ThrowHelper.ThrowMoreThanOneElementException()
   at System.Linq.Enumerable.TryGetSingle[TSource](IEnumerable`1 source, Boolean& found)
   at System.Linq.Enumerable.Single[TSource](IEnumerable`1 source)
   at System.Linq.ImmutableArrayExtensions.SingleOrDefault[T](ImmutableArray`1 immutableArray, Func`2 predicate)
   at Rocks.Extensions.IAssemblySymbolExtensions.ExposesInternalsTo(IAssemblySymbol self, IAssemblySymbol other) in /_/src/Rocks/Extensions/IAssemblySymbolExtensions.cs:line 11
   at Rocks.Extensions.ISymbolExtensions.CanBeSeenByContainingAssembly(ISymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol) in /_/src/Rocks/Extensions/ISymbolExtensions.cs:line 22
   at Rocks.Extensions.ITypeSymbolExtensions.<>c__DisplayClass10_0.<GetMockableProperties>b__5(IPropertySymbol _) in /_/src/Rocks/Extensions/ITypeSymbolExtensions.cs:line 531
   at System.Linq.Enumerable.WhereEnumerableIterator`1.MoveNext()
   at Rocks.Extensions.ITypeSymbolExtensions.GetMockableProperties(ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol, HashSet`1 shims, UInt32& memberIdentifier) in /_/src/Rocks/Extensions/ITypeSymbolExtensions.cs:line 530
   at Rocks.MockInformation.Validate(ITypeSymbol typeToMock, BuildType buildType) in /_/src/Rocks/MockInformation.cs:line 74
   at Rocks.MockInformation..ctor(ITypeSymbol typeToMock, IAssemblySymbol containingAssemblyOfInvocationSymbol, SemanticModel model, ConfigurationValues configurationValues, BuildType buildType) in /_/src/Rocks/MockInformation.cs:line 17
   at Rocks.CodeGenerationTest.Extensions.IsValidTarget(Type self) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\Extensions.cs:line 68
   at Rocks.CodeGenerationTest.TestGenerator.<>c__DisplayClass0_0.<Generate>b__1(Type _) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\TestGenerator.cs:line 20
   at System.Threading.Tasks.Parallel.<>c__DisplayClass44_0`2.<PartitionerForEachWorker>b__1(IEnumerator& partitionState, Int32 timeout, Boolean& replicationDelegateYieldedBeforeCompletion)
--- End of stack trace from previous location ---
   at System.Threading.Tasks.Parallel.<>c__DisplayClass44_0`2.<PartitionerForEachWorker>b__1(IEnumerator& partitionState, Int32 timeout, Boolean& replicationDelegateYieldedBeforeCompletion)
   at System.Threading.Tasks.TaskReplicator.Replica`1.ExecuteAction(Boolean& yieldedBeforeCompletion)
   at System.Threading.Tasks.TaskReplicator.Replica.Execute()<---
