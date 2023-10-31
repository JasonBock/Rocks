Things to try:

* In `GetCompilationOptionsFromCompiler()`, make `"/nowarn:ROCK10"` is added
    * compilationOption - Suppress
    * optionsProvider - null
* In calling `RunAsync()`, add `ROCK10` to `disabledDiagnostics`
    * compilationOption - Suppress
    * optionsProvider - null
* Add `[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "ROCK10", Scope = "module")]`
    * compilationOption - null
    * optionsProvider - null
* Add `[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "ROCK10")]` to the containing member that is doing `Rock.Create<>()`
    * compilationOption - null
    * optionsProvider - null
* Add `#pragma warning disable ROCK10` around the location of the diagnostic (which means I need to fix how the diagnostic is located)
    * compilationOption - null
    * optionsProvider - null
* Add `dotnet_diagnostic.ROCK10.severity = none` or `silent` or `suggestion` to `.editorconfig` (have I done that before in Rocks, or another project? https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md#access-analyzer-config-properties, but not sure how to "pass" the `.editorconfig` to a generator)
    * compilationOption - null
    * optionsProvider - null

Well, at least having the first two options are doable. I can see where this may be difficult to do, if I want to support all the options a developer can do.

I think I should at least support `nowarn` and an `.editorconfig` setting.

I'll definitely need to test in Rocks.IntegrationTests if I set ROCK10 in <NoWarn>

I'm not sure `#pragma warning disable ROCK10` is a valid option, because the severity for `ROCK10` is `Error`.

OK, reset time...

* If the mock type has a member that is considered "obsolete", then we don't create that member
* If the mock type is "obsolete", we generate our error.

Now, this still doesn't alleviate the issue, because...
* Figuring out if something is "obsolete" isn't as easy as I made it out to be. There's all the conditions I listed above to determine if the member is obsolete...or are there?

If I'm referencing a type that has an obsolete member...

Wait. I think I have been overthinking this:

```csharp
public class ImplementIHaveObsolete
    : IHaveObsolete
{
    public void NotObsolete()
    {
        throw new NotImplementedException();
    }

    public void Obsolete()
    {
        throw new NotImplementedException();
    }
}

public class ImplementHaveObsolete
    : HaveObsolete
{
    [Obsolete("Old", true)]
    public override void Obsolete()
    {
        base.Obsolete();
    }
}

public interface IHaveObsolete
{
    void NotObsolete();

    [Obsolete("Old", true)]
    void Obsolete();
}

public class HaveObsolete
{
    public virtual void NotObsolete() { }

    [Obsolete("Old", true)]
    public virtual void Obsolete() { }
}
```

If the member is on an interface, I can still implement it, even with `<AnalysisMode>all</AnalysisMode>` and `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`. If it's on a class, I can just put that attribute on the overriding member. If that causes a compilation error, someone can add a `[SuppressMessage]` in a GlobalSuppression.cs file, pointing at the member in the generated code. (But what if that doesn't exist?)

I think I'm going to table this for a bit...

Which diagnostics do something with obsolete members?

* `CannotMockObsoleteTypeDiagnostic` - `ROCK2`
* `MemberIsObsoleteDiagnostic` - `ROCK10`
* `MemberUsesObsoleteTypeDiagnostic` - `ROCK9`

I should never do this:

```csharp
var treatWarningsAsErrors = compilation.Options.GeneralDiagnosticOption == ReportDiagnostic.Error;
```

I don't care. If it can be a warning, the user can do whatever configuration they want to ignore it. If it's an error, then I should report a Rocks diagnostic, even if others will be gen'd by the compiler. Either way, it's wrong.

Conclusions:
* If I mock a type that has [Obsolete] on it
    * If `error` is `true`, then create `ROCK2`
    * Else, it needs to be part of the mock class definition
* If I mock a member that has [Obsolete] on it, it needs to be part of the member implementation. Doesn't matter if it's an error or warning.
* If I mock a member that uses an obsolete type.
    * If `error` is `true`, then create `ROCK9`
    * Else, it needs to be part of the mock class definition
* `ROCK10` is no longer created

TODO:
* Add test for an event using an obsolete type