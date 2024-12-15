* Change the version number to 9.0.1
* Copy the `GenerateWhenBothCompilationsUseRocksAndSourceMakesInternalsVisibleAsync()` and set up a test to show that a `public` type for a parameter will work, but an `internal` type is currently failing.
* Figure it out.

In `IAssemblySymbolExtensions.ExposesInternalsTo()`, the problem is that these two values don't equal:

```
?self.GetAttributes()[4].AttributeClass.ContainingAssembly.Name
"System.Runtime"
?internalsVisibleToAttribute.Assembly.GetName().Name
"mscorlib"
```

And so this:

```c#
_.AttributeClass.ContainingAssembly.Name == internalsVisibleToAttribute.Assembly.GetName().Name
```

**FAILS**

Note that in a unit test:

```
?self.GetAttributes()[0].AttributeClass.ContainingAssembly.Name
"System.Private.CoreLib"
?internalsVisibleToAttribute.Assembly.GetName().Name
"System.Private.CoreLib"
```

They end up being the same so the test will pass.

TODO:
* DONE - Create "make" integration tests
* DONE - Update version to 9.0.1
* DONE - Update changelog
* Publish to NuGet
* Close issue
* Party