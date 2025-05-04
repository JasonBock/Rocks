When I was typing `var exp = new ISuppressValueTypeInReturnValue` in Rocks.Scenarios, I got this exception in the refactoring:

```
System.NullReferenceException : Object reference not set to an instance of an object.
   at Rocks.Completions.Extensions.SyntaxNodeExtensions.FindParentSymbol(SyntaxNode self,SemanticModel model,CancellationToken cancellationToken)
   at async Rocks.Completions.AddRockAttributeRefactoring.ComputeRefactoringsAsync(<Unknown Parameters>)
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   at async Microsoft.CodeAnalysis.CodeRefactorings.CodeRefactoringService.<>c__DisplayClass12_0.<GetRefactoringFromProviderAsync>b__0(<Unknown Parameters>)
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   at async Microsoft.CodeAnalysis.Extensions.IExtensionManagerExtensions.PerformFunctionAsync[T](<Unknown Parameters>)
```

Note that if I had a class:

```c#
public class Mockable
{
	public virtual void DoSomething() { }
}
```

And I typed `var exp = new Mockable`, that works.

Side note: Maybe add `Rocks.Completions` to `Rocks.