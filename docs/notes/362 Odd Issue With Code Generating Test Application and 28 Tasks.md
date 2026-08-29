```
Generator task count: 28

...

Error Counts
Error:

ID: CS0433
Description: (1075,54): error CS0433: The type 'ExpressionValueProvider' exists in both 'Aspose.Email, Version=26.7.0.0, Culture=neutral, PublicKeyToken=716fcc553a201e56' and 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Code:
ExpressionValueProvider

Error:

ID: CS0433
Description: (7488,52): error CS0433: The type 'ExpressionValueProvider' exists in both 'Aspose.Email, Version=26.7.0.0, Culture=neutral, PublicKeyToken=716fcc553a201e56' and 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Code:
ExpressionValueProvider

Error:

ID: CS0433
Description: (1075,54): error CS0433: The type 'ExpressionValueProvider' exists in both 'Aspose.Email, Version=26.7.0.0, Culture=neutral, PublicKeyToken=716fcc553a201e56' and 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Code:
ExpressionValueProvider

Error:

ID: CS0433
Description: (7488,52): error CS0433: The type 'ExpressionValueProvider' exists in both 'Aspose.Email, Version=26.7.0.0, Culture=neutral, PublicKeyToken=716fcc553a201e56' and 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Code:
ExpressionValueProvider

Error:

ID: CS7036
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\AsposeEmailAliasNewtonsoft.Json.Serialization.ExpressionValueProvider_Rock_Make.g.cs(31,11): error CS7036: There is no argument given that corresponds to the required parameter 'memberInfo' of 'ExpressionValueProvider.ExpressionValueProvider(MemberInfo)'
Code:
Mock

Error:

ID: CS7036
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\AsposeEmailAliasNewtonsoft.Json.Serialization.ExpressionValueProvider_Partial_Rock_Make.g.cs(31,11): error CS7036: There is no argument given that corresponds to the required parameter 'memberInfo' of 'ExpressionValueProvider.ExpressionValueProvider(MemberInfo)'
Code:
Mock

Error:

ID: CS7036
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\AsposeEmailAliasNewtonsoft.Json.Serialization.ExpressionValueProvider_Rock_Make.g.cs(31,11): error CS7036: There is no argument given that corresponds to the required parameter 'memberInfo' of 'ExpressionValueProvider.ExpressionValueProvider(MemberInfo)'
Code:
Mock

Error:

ID: CS7036
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\AsposeEmailAliasNewtonsoft.Json.Serialization.ExpressionValueProvider_Partial_Rock_Make.g.cs(31,11): error CS7036: There is no argument given that corresponds to the required parameter 'memberInfo' of 'ExpressionValueProvider.ExpressionValueProvider(MemberInfo)'
Code:
Mock

        Code: CS0433, Count: 4
        Code: CS7036, Count: 4
Total Error Count: 8

Total time: 00:16:10.6437219
```

What about other processor counts?

4 - Total time: 00:06:26.6622957
1 and 2 give this odd error:

```
Unhandled exception. System.AggregateException: One or more errors occurred. (Unable to load one or more of the requested types.
Method 'get_Task' in type 'DotNext.Threading.Tasks.<TaskSchedulerExtensions_T>FEC23FF8E24C5C893F2B0DCE3A3DBD6B033A895D525ABEAE11C935B5879FC7CF4__ImmediateTask`2' from assembly 'DotNext.Threading, Version=6.6.1.0, Culture=neutral, PublicKeyToken=c8827b44e18fe572' does not have an implementation.
Method 'get_Task' in type 'DotNext.Threading.Tasks.<TaskSchedulerExtensions_T>FEC23FF8E24C5C893F2B0DCE3A3DBD6B033A895D525ABEAE11C935B5879FC7CF4__DelayedTaskStateMachine`2' from assembly 'DotNext.Threading, Version=6.6.1.0, Culture=neutral, PublicKeyToken=c8827b44e18fe572' does not have an implementation.)
 ---> System.Reflection.ReflectionTypeLoadException: Unable to load one or more of the requested types.
Method 'get_Task' in type 'DotNext.Threading.Tasks.<TaskSchedulerExtensions_T>FEC23FF8E24C5C893F2B0DCE3A3DBD6B033A895D525ABEAE11C935B5879FC7CF4__ImmediateTask`2' from assembly 'DotNext.Threading, Version=6.6.1.0, Culture=neutral, PublicKeyToken=c8827b44e18fe572' does not have an implementation.
Method 'get_Task' in type 'DotNext.Threading.Tasks.<TaskSchedulerExtensions_T>FEC23FF8E24C5C893F2B0DCE3A3DBD6B033A895D525ABEAE11C935B5879FC7CF4__DelayedTaskStateMachine`2' from assembly 'DotNext.Threading, Version=6.6.1.0, Culture=neutral, PublicKeyToken=c8827b44e18fe572' does not have an implementation.
   at System.Reflection.RuntimeModule.GetDefinedTypes()
   at System.Reflection.RuntimeModule.GetTypes()
   at Rocks.CodeGenerationTest.TestGenerator.<>c__DisplayClass2_0.<GetTargetsAsync>b__0(Assembly _) in C:\Users\jason\source\repos\JasonBock\Rocks\src\Rocks.CodeGenerationTest\TestGenerator.cs:line 88
   at System.Linq.Enumerable.SelectManySingleSelectorIterator`2.MoveNext()
   at System.Linq.Enumerable.DistinctByIterator[TSource,TKey](IEnumerable`1 source, Func`2 keySelector, IEqualityComparer`1 comparer)+MoveNext()
   at System.Linq.Enumerable.<ToArray>g__EnumerableToArray|333_0[TSource](IEnumerable`1 source)
   at System.Collections.Immutable.ImmutableArray.CreateRange[T](IEnumerable`1 items)
   at Rocks.CodeGenerationTest.TestGenerator.GetTargetsAsync(HashSet`1 targetAssemblies, ImmutableArray`1 typesToIgnore, ImmutableArray`1 typesToLoadAssembliesFrom, String[] aliases) in C:\Users\jason\source\repos\JasonBock\Rocks\src\Rocks.CodeGenerationTest\TestGenerator.cs:line 87
   at Program.<<Main>$>g__Generate|0_7(TypeAliasesMapping typeAliasesMapping, ImmutableArray`1 typesToLoadAssembliesFrom, CodeAccessibility codeAccessibility) in C:\Users\jason\source\repos\JasonBock\Rocks\src\Rocks.CodeGenerationTest\Program.cs:line 302
System.TypeLoadException: Method 'get_Task' in type 'DotNext.Threading.Tasks.<TaskSchedulerExtensions_T>FEC23FF8E24C5C893F2B0DCE3A3DBD6B033A895D525ABEAE11C935B5879FC7CF4__ImmediateTask`2' from assembly 'DotNext.Threading, Version=6.6.1.0, Culture=neutral, PublicKeyToken=c8827b44e18fe572' does not have an implementation.
System.TypeLoadException: Method 'get_Task' in type 'DotNext.Threading.Tasks.<TaskSchedulerExtensions_T>FEC23FF8E24C5C893F2B0DCE3A3DBD6B033A895D525ABEAE11C935B5879FC7CF4__DelayedTaskStateMachine`2' from assembly 'DotNext.Threading, Version=6.6.1.0, Culture=neutral, PublicKeyToken=c8827b44e18fe572' does not have an implementation.
   --- End of inner exception stack trace ---
   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Program.<<Main>$>g__TestWithTypes|0_4() in C:\Users\jason\source\repos\JasonBock\Rocks\src\Rocks.CodeGenerationTest\Program.cs:line 327
   at Program.<Main>$(String[] args) in C:\Users\jason\source\repos\JasonBock\Rocks\src\Rocks.CodeGenerationTest\Program.cs:line 21
```

* Why is `DotNext.Threading.Tasks.<TaskSchedulerExtensions_T>FEC23FF8E24C5C893F2B0DCE3A3DBD6B033A895D525ABEAE11C935B5879FC7CF4__ImmediateTask``2'` even showing up? It's `internal`, it's `sealed`, its name isn't "speakable" ... why?
* Why are we getting that weird `get_Name` not having an implementation?
* Why is there a conflict with Aspose and Newtonsoft.Json?