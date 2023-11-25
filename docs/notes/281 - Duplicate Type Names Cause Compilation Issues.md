```
Error:

ID: CS0433
Description: Rocks\Rocks.RockCreateGenerator\DnsClient.IDnsQueryResponse_Rock_Create.g.cs(243,29): error CS0433: The type 'DnsQuerySettings' exists in both 'Aspose.Email, Version=23.10.0.0, Culture=neutral, PublicKeyToken=716fcc553a201e56' and 'DnsClient, Version=1.7.0.0, Culture=neutral, PublicKeyToken=4574bb5573c51424'
Code:
DnsQuerySettings
```

Need to create a test to reproduce this. Look at the internal/non-public tests.

Will need to get the .Aliases (can there be more than one?) and then gen `extern alias` code for these.

With the `Aliases` property for a `PackageReference` (https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#packagereference-aliases), I'm guessing that multiple aliases can be defined. What delimiter should be used if I want to specify more than one alias? My assumption would be a comma or a semicolon, but....it's not specified in the docs, so, not sure what to use here.

Also, if I define multiple aliases, I'm guessing I can use any one of them for `extern alias`. Is there a reason to be able to define more than one alias for a package?

Also also, is the alias uses like `global` is? [This artile](https://jeanarjean.com/blog/2021-03-10-how-to-create-alias-property-in-your-csproj) and [this article](https://learn.microsoft.com/en-us/archive/blogs/ansonh/extern-alias-walkthrough) and [this article](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/extern-alias) suggests that:

```csharp
extern alias MyAlias;

using MyAlias::MyNamespace.MyClass
```

But [the docs](https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#packagereference-aliases) suggest otherwise.

I guess, create a simple solution with three projects:

* ClassLibrary1
* ClassLibrary2
* ClassLibrary3, which references the other two:
    * First, use `CL1` and `CL2` aliases for the references
    * Then, use multiple, like `CL1,MyCL1` and `CL2, MyCL2`

So, either `typeof(ProducerOne.MyNamespace.MyClass)` or `typeof(ProducerOne::MyNamespace.MyClass)` works.

https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/extern-alias
https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#packagereference-aliases
https://learn.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.projectreference.aliases?view=roslyn-dotnet-4.7.0

`ITypeSymbolExtensions.GetFullyQualifiedName()` looks like the logical place to do this, but it requires a `Compilation` object. When I set this with `TypeReferenceModel`, it's stored in a property, but that method is called in multiple places. I'll have to see just how pervasive this would be...

OK, in the code gen test app, I'll need to find which assemblies have an alias, add that to the top of the file that does code gen, **and** add that to any type I'm passing to `Rock.Create()` and `Rock.Make()`.